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
            new(CoreIcons.Search, $"{glyphPath}/magnifying-glass-regular.svg", "Search", "affordance"),
            new(CoreIcons.Select, $"{glyphPath}/chevron-down-regular.svg", "Select", "affordance"),
            new(CoreIcons.Clear, $"{glyphPath}/xmark-regular.svg", "Clear", "affordance"),
            new(CoreIcons.Expand, $"{glyphPath}/chevron-right-regular.svg", "Expand", "affordance"),
            new(CoreIcons.Collapse, $"{glyphPath}/chevron-down-regular.svg", "Collapse", "affordance"),
            new(CoreIcons.SelectDate, $"{glyphPath}/calendar-day-light.svg", "Select Date", "affordance"),
            new(CoreIcons.SelectDateTime, $"{glyphPath}/calendar-clock-light.svg", "Select Date/Time", "affordance"),
            new(CoreIcons.SelectTime, $"{glyphPath}/clock-light.svg", "Select Time", "affordance"),

            new(CoreIcons.Currency, $"{glyphPath}/dollar-sign-light.svg", "Enter dollar amount", "affordance"),

            new(CoreIcons.IsRequired, $"{glyphPath}/asterisk-alone-full.svg", "Required", "icon"),
            new(CoreIcons.HasDescription, $"{glyphPath}/info-alone-full.svg", "See Description", "icon"),

            new(CoreIcons.CollectionEmpty, $"{glyphPath}/xmark-regular.svg", "No Items", "icon"),

            new(CoreIcons.Copy, $"{glyphPath}/copy-light-full.svg", "Copy", "affordance"),
            new(CoreIcons.Show, $"{glyphPath}/eye-light-full.svg", "Show", "affordance"),
            new(CoreIcons.Hide, $"{glyphPath}/eye-slash-light-full.svg", "Hide", "affordance"),

            new(CoreIcons.PicturePlaceholder, $"{glyphPath}/camera-on-slide.svg", "Picture Placeholder", "icon"),
            new(CoreIcons.OpenCamera, $"{glyphPath}/camera-light-full.svg", "Open Camera", "affordance"),
            new(CoreIcons.OpenFolder, $"{glyphPath}/folder-open-light-full.svg", "Open Folder", "affordance"),
        }).ToDictionary(e => e.Key, e => e);
}

public static class CoreIcons
{
    public const string Search = "core-search";
    public const string Select = "core-select";
    public const string Clear = "core-clear";
    public const string Expand = "core-expand";
    public const string Collapse = "core-collapse";
    public const string SelectDate = "core-select-date";
    public const string SelectDateTime = "core-select-datetime";
    public const string SelectTime = "core-select-time";
    public const string Currency = "core-currency";
    public const string IsRequired = "core-is-required";
    public const string HasDescription = "core-has-description";
    public const string CollectionEmpty = "core-collection-empty";
    public const string Copy = "core-copy";
    public const string Show = "core-show";
    public const string Hide = "core-hide";
    public const string PicturePlaceholder = "core-picture-placeholder";
    public const string OpenCamera = "core-open-camera";
    public const string OpenFolder = "core-open-folder";
}
