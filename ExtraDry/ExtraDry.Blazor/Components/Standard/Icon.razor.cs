#nullable enable

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

    private string IconClass => ThemeInfo?.Icons.ContainsKey(Key) ?? false ? ThemeInfo.Icons[Key].CssClass : "";

    private string? IconAlternateText => Alt ?? (ThemeInfo?.Icons.ContainsKey(Key) ?? false ? ThemeInfo.Icons[Key].AlternateText : "");

    private string? ImagePath {
        get {
            if(Key.Contains('/')) {
                return Key;
            }
            else if(ThemeInfo == null || !ThemeInfo.Icons.Any()) {
                NoThemeError();
                return $"/img/themeless/{Key}.svg";
            }
            else if(ThemeInfo.Icons.ContainsKey(Key)) {
                return ThemeInfo.Icons[Key].ImagePath;
            }
            else {
                Logger.LogWarning("Icon '{icon}' not registered, add an entry for icon to the `Icons` attribute of the `Theme` component.", Key);
                return $"/img/no-icon-for-{Key}.svg";
            }
        }
    }
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


}

