namespace ExtraDry.Core;

/// <summary>
/// Describes an Icon that can be used in the system, indexed by a `Key`. Use the key in the Icon
/// tag or anywhere an Icon property is available. The icon info will resolve the icon's name to a
/// physical location and will provide alternate text and CSS classes for the img tag. The
/// IconInfo's should be registered with the Theme tag in the main layout.
/// </summary>
public class IconInfo
{
    /// <summary>
    /// Create a icon indexed by Key with the indicated relative path, alternate text, and optional
    /// CSS class.
    /// </summary>
    /// <param name="key">The string key to index into the icon, case-insensitive.</param>
    /// <param name="imagePath">
    /// The path to the icon, typically an SVG in the /img/ directory.
    /// </param>
    /// <param name="alternateText">
    /// The alternate text that describes the icon for screen readers.
    /// </param>
    /// <param name="cssClass">
    /// An optional additional class to apply to `img` tags, such as 'icon' or 'glyph'.
    /// </param>
    /// <param name="svgRenderType">An optional instruction on how to render SVG images.</param>
    public IconInfo(string key, string imagePath, string alternateText, string? cssClass = null, SvgRenderType svgRenderType = SvgRenderType.Atlas)
    {
        Key = key;
        ImagePath = imagePath;
        AlternateText = alternateText;
        CssClass = cssClass ?? string.Empty;
        SvgRenderType = svgRenderType;
    }

    /// <summary>
    /// Create a icon indexed by Key with the indicated class for decorating a `i` tag.
    /// </summary>
    /// <param name="key">The string key to index into the icon, case-insensitive.</param>
    /// <param name="iconClass">
    /// A required class to apply to `i` tags, such as 'fas fa-edit'.
    /// </param>
    public IconInfo(string key, string iconClass)
    {
        Key = key;
        CssClass = iconClass;
    }

    /// <summary>
    /// The key for the icon
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The relative URI path to the image for the icon.
    /// </summary>
    public string? ImagePath { get; set; }

    /// <summary>
    /// The altnerate text applied to the image.
    /// </summary>
    public string? AlternateText { get; set; }

    /// <summary>
    /// Any additional CSS classes to apply when this icon is used.
    /// </summary>
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// If the icon is an SVG, the type of rendering to use.
    /// </summary>
    public SvgRenderType SvgRenderType { get; set; } = SvgRenderType.Atlas;

    /// <summary>
    /// If the icon is an SVG, this is the reformatted SVG body to insert into the page that uses a
    /// reference to either the atlas version or the file version of the SVG.
    /// </summary>
    internal string SvgInlineBody { get; set; } = string.Empty;

    private static readonly string glyphPath = "/_content/ExtraDry.Blazor/img/glyphs";

    public static readonly Dictionary<string, IconInfo> FallbackIcons = (new IconInfo[] {
            new("search", $"{glyphPath}/magnifying-glass-regular.svg", "Search", "affordance"),
            new("select", $"{glyphPath}/chevron-down-regular.svg", "Select", "affordance"),
            new("clear", $"{glyphPath}/xmark-regular.svg", "Clear", "affordance"),
            new("expand", $"{glyphPath}/chevron-right-regular.svg", "Expand", "affordance"),
            new("collapse", $"{glyphPath}/chevron-down-regular.svg", "Collapse", "affordance"),
            new("select-date", $"{glyphPath}/calendar-day-light.svg", "Select Date", "affordance"),
            new("select-datetime", $"{glyphPath}/calendar-clock-light.svg", "Select Date/Time", "affordance"),
            new("select-time", $"{glyphPath}/clock-light.svg", "Select Time", "affordance"),

            new("currency", $"{glyphPath}/dollar-sign-light.svg", "Enter dollar amount", "affordance"),

            new("is-required", $"{glyphPath}/asterisk-alone-full.svg", "Required", "icon"),
            new("has-description", $"{glyphPath}/info-alone-full.svg", "See Description", "icon"),

            new("collection-empty", $"{glyphPath}/xmark-regular.svg", "No Items", "icon"),

            new("copy", $"{glyphPath}/copy-light-full.svg", "Copy", "affordance"),
            new("show", $"{glyphPath}/eye-light-full.svg", "Show", "affordance"),
            new("hide", $"{glyphPath}/eye-slash-light-full.svg", "Hide", "affordance"),

        }).ToDictionary(e => e.Key, e => e);
}
