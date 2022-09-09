namespace ExtraDry.Blazor;

/// <summary>
/// Represents an icon in the system, rendered using a global repository of icons which
/// are either images or styles on `i` tags.
/// </summary>
public partial class Icon : ComponentBase, IExtraDryComponent {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// A key for the icon.  The key is typically be registered with an enclosing `Theme` tag.
    /// If the key is a Uri, then the Uri is used directly.
    /// </summary>
    [Parameter, EditorRequired]
    public string Key { get; set; } = null!;

    /// <summary>
    /// Alt text that is applied to `img` tags when icon is an image.  This overrides
    /// the default "alt" text which is registered with the `Theme`.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

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
            else if(ThemeInfo?.Icons?.ContainsKey(Key) ?? false) {
                return ThemeInfo.Icons[Key];
            }
            else if(fallbackIcons.ContainsKey(Key)) {
                return fallbackIcons[Key];
            }
            else {
                if(ThemeInfo == null) {
                    NoThemeError();
                    return new IconInfo(Key, "no-theme") { ImagePath = $"/img/themeless/{Key}.svg", AlternateText = "" };
                }
                else {
                    Logger.LogWarning("Icon '{icon}' not registered, add an entry for icon to the `Icons` attribute of the `Theme` component.", Key);
                    return new IconInfo(Key, "no-key") { ImagePath = $"/img/no-icon-for-{Key}.svg", AlternateText = "" };
                }
            }
        }
    }

    private string? ImagePath => IconInfo.ImagePath;

    private void NoThemeError()
    {
        if(noThemeErrorIssued) {
            Logger.LogError("Icons must be used within a `Theme` component.  Create a `Theme` component, typically in the MainLayout.blazor component that wraps the site.  Then add a collection of `IconInfo` to the `Icons` property to register the key of the icon with an image or a font glyph.");
            noThemeErrorIssued = false;
        }
    }
    private static bool noThemeErrorIssued = true;

    [Inject]
    private ILogger<Icon> Logger { get; set; } = null!;

    private static string glyphPath = "/_content/ExtraDry.Blazor/img/glyphs";

    private static Dictionary<string, IconInfo> fallbackIcons = (new IconInfo[] {
            new IconInfo("search", $"{glyphPath}/magnifying-glass-regular.svg", "Search", "glyph"),
            new IconInfo("select", $"{glyphPath}/chevron-down-regular.svg", "Select", "glyph"),
            new IconInfo("clear", $"{glyphPath}/xmark-regular.svg", "Clear", "glyph"),
        }).ToDictionary(e => e.Key, e => e);

}
