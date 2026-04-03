namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents an icon in the system, rendered using a global repository of icons which are either
/// images or styles on `i` tags.
/// </summary>
public partial class Icon : ComponentBase
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// A key for the icon. The key is typically be registered with an enclosing `Theme` tag. If
    /// the key is a Uri, then the Uri is used directly.
    /// </summary>
    [Parameter, EditorRequired]
    public string Key { get; set; } = null!;

    /// <summary>
    /// Alt text that is applied to `img` tags when icon is an image. This overrides the default
    /// "alt" text which is registered with the `Theme`.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <inheritdoc cref="Button.OnClick"/>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <inheritdoc cref="Blazor.ThemeInfo" />
    [CascadingParameter]
    protected ThemeInfo? ThemeInfo { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, IconClass);

    private string IconClass => IconInfo.CssClass;

    private string? IconAlternateText => Alt ?? IconInfo.AlternateText;

    private IconInfo IconInfo {
        get {
            if(Key.Contains('/')) {
                return new IconInfo(Key, "image") { ImagePath = Key, AlternateText = "" };
            }
            else if(ThemeInfo?.Icons?.TryGetValue(Key, out var themeIcon) ?? false) {
                return themeIcon;
            }
            else if(IconInfo.FallbackIcons.TryGetValue(Key, out var icon)) {
                return icon;
            }
            else {
                if(ThemeInfo == null) {
                    LogNoThemeErrorOnce();
                    return new IconInfo(Key, "no-theme") { ImagePath = $"/img/themeless/{Key}.svg", AlternateText = "" };
                }
                else {
                    Logger.LogMissingIcon(Key);
                    return new IconInfo(Key, "no-key") { ImagePath = $"/img/no-icon-for-{Key}.svg", AlternateText = "" };
                }
            }
        }
    }

    private string? ImagePath => IconInfo.ImagePath;

    private string? VersionImagePath => (ThemeInfo?.Version, ImagePath) switch {
        (null, _) => ImagePath,
        (_, null) => null,
        ("", _) => ImagePath,
        _ => $"{ImagePath}{(ImagePath.Contains('?') ? '&' : '?')}v={ThemeInfo.Version}"
    };

    private bool RenderSvg => IconInfo.SvgRenderType != SvgRenderType.Reference && !string.IsNullOrEmpty(IconInfo.SvgInlineBody);

    private MarkupString Svg => (MarkupString)IconInfo.SvgInlineBody.Replace("additional-classes", CssClasses);

    private void LogNoThemeErrorOnce()
    {
        if(noThemeErrorIssued) {
            Logger.LogConsoleError("Icons must be used within a `Theme` component.  Create a `Theme` component, typically in the MainLayout.blazor component that wraps the site.  Then add a collection of `IconInfo` to the `Icons` property to register the key of the icon with an image or a font glyph.");
            noThemeErrorIssued = false;
        }
    }

    private static bool noThemeErrorIssued = true;

    [Inject]
    private ILogger<Icon> Logger { get; set; } = null!;

}
