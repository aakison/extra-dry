using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ExtraDry.Blazor;

/// <summary>
/// Provides a cascading theme mechanism to configure portions of ExtraDry.
/// Note: this does not cover styles like colors which should be done in CSS.
/// </summary>
[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Theme does not render a root tag, purpose is to register cascading values.")]
public partial class Theme(
    ILogger<Theme> Logger,
    HttpClient Http)
    : ComponentBase
{
    /// <summary>
    /// The content that this theme applies to.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// The collection of icons that are used for this theme. See IconInfo for details on creating
    /// the icons, then register them here. The icons must always be available and should be stored
    /// in a static backing field to ensure they are available, especially when the site navigates
    /// to a missing page and back. It is recommended they are part of the AppViewModel registered
    /// as a Singleton.
    /// </summary>
    [Parameter]
    public IEnumerable<IconInfo>? Icons { get; set; }

    /// <summary>
    /// If set, will append this version string to any SVG icons loaded. Combine this with
    /// server-side versioning that adds cache control directives.  I.e. works when server has
    /// `CacheControlMiddleware` registered.
    /// </summary>
    [Parameter]
    public string Version { get; set; } = "";

    /// <summary>
    /// When the `Version` is set, this prefix is applied before the version string. Defaults to "v".
    /// </summary>
    [Parameter]
    public string VersionPrefix { get; set; } = "v";

    /// <inheritdoc cref="Blazor.ThemeInfo" />
    [CascadingParameter]
    protected ThemeInfo ThemeInfo { get; set; } = new();

    /// <summary>
    /// A custom error component that is applied and used on any DryErrorBoundary instead of the
    /// default.
    /// </summary>
    [Parameter]
    public Type? ErrorComponent { get; set; }

    /// <summary>
    /// A custom validation error component that is applied and used on any ValidationBoundary
    /// instead of the default.
    /// </summary>
    [Parameter]
    public Type? ValidationMessageComponent { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Error" />
    /// <see cref="Suspense{TModel}" />
    [Parameter]
    public RenderFragment<IndicatorContext>? SuspenseError { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Timeout" />
    /// <see cref="Suspense{TModel}" />
    [Parameter]
    public RenderFragment<IndicatorContext>? SuspenseTimeout { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Fallback" />
    /// <see cref="Suspense{TModel}" />
    [Parameter]
    public RenderFragment<IndicatorContext>? SuspenseFallback { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if(ErrorComponent != null) {
            ThemeInfo.ErrorComponent = ErrorComponent;
        }

        if(ValidationMessageComponent != null) {
            ThemeInfo.ValidationMessageComponent = ValidationMessageComponent;
        }

        if(SuspenseError != null) {
            ThemeInfo.SuspenseError = SuspenseError;
        }

        if(SuspenseTimeout != null) {
            ThemeInfo.SuspenseTimeout = SuspenseTimeout;
        }

        if(SuspenseFallback != null) {
            ThemeInfo.SuspenseFallback = SuspenseFallback;
        }

        ThemeInfo.Version = Version;

        if(!loaded) {
            LoadSvgReferencesAsync();
        }

        // Always cascade the cached icons. Skipping this when `loaded` is true left ThemeInfo.Icons
        // empty on later prerender passes so icons fell through to missing-image paths.
        ThemeInfo.Icons = CachedIcons.ToDictionary(e => e.Key, e => e.Value);

        if(OperatingSystem.IsBrowser()) {
            // WASM: load atlas into the DOM once and point <use> at local #ids (unchanged).
            if(!loaded) {
                await SetSvgAtlasViewboxesAsync();
            }
        }
        else {
            // Prerender/server: never use in-DOM #ids — the atlas is not in the HTML. Keep the
            // classic /bundles/atlas.svg#key references that work without client JS.
            ApplyServerAtlasReferences();
        }

        loaded = true;
    }

    private void LoadSvgReferencesAsync()
    {
        Icons ??= [];

        // Ensure we have all icons, including fallbacks, but do not duplicate any, prefering user-supplied instead of fallback.
        var fallbackIcons = IconInfo.FallbackIcons.Values.Where(e => !Icons.Any(i => i.Key == e.Key));
        var allIcons = Icons.Union(fallbackIcons);
        foreach(var icon in allIcons) {
            LoadIcon(icon);
        }
    }

    /// <summary>
    /// WASM-only: load the atlas SVG into the document and rewrite atlas icons to local
    /// <c>#key</c> references so each icon does not fetch the atlas again.
    /// </summary>
    private async Task SetSvgAtlasViewboxesAsync()
    {
        Dictionary<string, string>? viewBoxManifest = null;
        try {
            var atlasUrl = $"/bundles/atlas.svg?v={Version}";
            var atlasContent = await Http.GetStringAsync(atlasUrl);
            InlineSvgAtlas = (MarkupString)atlasContent;
            viewBoxManifest = SymbolViewBoxRegex().Matches(atlasContent)
                .ToDictionary(
                    m => m.Groups[1].Value,
                    m => m.Groups[2].Value);
        }
        catch(Exception ex) {
            Logger.LogWarning(ex, "Failed to load SVG atlas.");
        }

        // Force a re-render so that the InlineSvgAtlas is updated and available to downstream components.
        StateHasChanged();
        await Task.Yield();

        foreach(var icon in Icons!) {
            if(icon.SvgRenderType == SvgRenderType.Atlas) {
                var viewBox = (viewBoxManifest?.TryGetValue(icon.Key, out var vb) ?? false) ? $" {vb}" : "";
                var classes = DataConverter.JoinNonEmpty(" ", icon.CssClass, "additional-classes");
                icon.SvgInlineBody = $@"<svg class=""{classes}""{viewBox}><title>{icon.AlternateText}</title><use href=""#{icon.Key}""></use></svg>";
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// Prerender/server: point atlas icons at the hosted atlas.svg so <c>&lt;use&gt;</c> works
    /// without an in-DOM sprite.
    /// </summary>
    private void ApplyServerAtlasReferences()
    {
        var version = $"?v={Version}";
        foreach(var icon in CachedIcons.Values) {
            if(icon.SvgRenderType != SvgRenderType.Atlas) {
                continue;
            }
            icon.SvgInlineBody = $@"<svg class=""{icon.CssClass} additional-classes""><title>{icon.AlternateText}</title><use href=""/bundles/atlas.svg{version}#{icon.Key}""></use></svg>";
        }
    }


    private void LoadIcon(IconInfo icon)
    {
        if(CachedIcons.ContainsKey(icon.Key)) {
            Logger.LogDuplicateIcon(icon.Key);
            return;
        }
        if(!CachedIcons.TryAdd(icon.Key, icon)) {
            Logger.LogConsoleError("Failed to add icon to the cache concurrently.");
            return;
        }
        try {
            if(icon == null || icon.ImagePath == null || icon.SvgRenderType == SvgRenderType.Reference
                || !icon.ImagePath.EndsWith(".svg", StringComparison.InvariantCultureIgnoreCase)) {
                return;
            }
            Logger.LogLoadingIcon(icon.Key, icon.ImagePath);
            var path = string.IsNullOrWhiteSpace(Version)
                ? icon.ImagePath
                : $"{icon.ImagePath}?{VersionPrefix}={Version}";

            if(icon.SvgRenderType == SvgRenderType.Atlas) {
                var version = $"?v={Version}";
                icon.SvgInlineBody = $@"<svg class=""{icon.CssClass} additional-classes"" data-viewbox><title>{icon.AlternateText}</title><use href=""/bundles/atlas.svg{version}#{icon.Key}""></use></svg>";
            }
        }
        catch(Exception ex) {
            Logger.LogIconFailed(icon.Key, icon.ImagePath, ex);
        }
    }

    private MarkupString InlineSvgAtlas { get; set; } = (MarkupString)"<div style=\"display:none\">SVG Atlas Goes here</div>";

    private static bool loaded;

    private static readonly ConcurrentDictionary<string, IconInfo> CachedIcons = new();

    [GeneratedRegex(@"<symbol\b[^>]*\bid=""([^""]+)""[^>]*\b(viewBox=""[^""]+"")")]
    private partial Regex SymbolViewBoxRegex();

    [GeneratedRegex(@"<svg[^>]*>")]
    private partial Regex SvgTagRegex();

    [GeneratedRegex(@"viewBox=""[\d\s.]*""")]
    private partial Regex ViewBoxRegex();

    [GeneratedRegex(@"<defs.*</defs>", RegexOptions.Singleline)]
    private partial Regex DefsRegex();

}
