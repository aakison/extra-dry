using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ExtraDry.Blazor;

/// <summary>
/// Provides a cascading theme mechanism to configure portions of ExtraDry.
/// Note: this does not cover styles like colors which should be done in CSS.
/// </summary>
[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Theme does not render a root tag, purpose is to register cascading values.")]
public partial class Theme(
    ILogger<Theme> Logger)
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
        await base.OnParametersSetAsync();

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

        LoadSvgReferences();
        ThemeInfo.Icons = CachedIcons.ToDictionary(e => e.Key, e => e.Value);
    }

    private void LoadSvgReferences()
    {
        if(loaded) {
            return;
        }
        Icons ??= [];

        // Ensure we have all icons, including fallbacks, but do not duplicate any, prefering user-supplied instead of fallback.
        var fallbackIcons = IconInfo.FallbackIcons.Values.Where(e => !Icons.Any(i => i.Key == e.Key));
        var allIcons = Icons.Union(fallbackIcons);
        foreach(var icon in allIcons) {
            LoadIcon(icon);
        }

        loaded = true;
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
            var onServer = !OperatingSystem.IsBrowser();
            if(onServer || icon == null || icon.ImagePath == null || icon.SvgRenderType == SvgRenderType.Reference
                || !icon.ImagePath.EndsWith(".svg", StringComparison.InvariantCultureIgnoreCase)) {
                return;
            }
            Logger.LogLoadingIcon(icon.Key, icon.ImagePath);
            var path = string.IsNullOrWhiteSpace(Version)
                ? icon.ImagePath
                : $"{icon.ImagePath}?{VersionPrefix}={Version}";

            if(icon.SvgRenderType == SvgRenderType.Atlas) {
                var version = $"?v={Version}";
                icon.SvgInlineBody = $@"<svg class=""{icon.CssClass}"" additional-classes><title>{icon.AlternateText}</title><use href=""/bundles/atlas.svg{version}#{icon.Key}""></use></svg>";
            }

        }
        catch(Exception ex) {
            Logger.LogIconFailed(icon.Key, icon.ImagePath, ex);
        }
    }

    private static bool loaded;

    private static readonly ConcurrentDictionary<string, IconInfo> CachedIcons = new();

    [GeneratedRegex(@"<svg[^>]*>")]
    private partial Regex SvgTagRegex();

    [GeneratedRegex(@"viewBox=""[\d\s.]*""")]
    private partial Regex ViewBoxRegex();

    [GeneratedRegex(@"<defs.*</defs>", RegexOptions.Singleline)]
    private partial Regex DefsRegex();

}
