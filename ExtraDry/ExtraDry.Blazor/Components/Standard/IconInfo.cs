namespace ExtraDry.Blazor;

/// <summary>
/// Describes an Icon that can be used in the system, indexed by a `Key`.
/// Use the key in the Icon tag or anywhere an Icon property is available.
/// The icon info will resolve the icon's name to a physical location and will provide
/// alternate text and CSS classes for the img tag.
/// The IconInfo's should be registered with the Theme tag in the main layout.
/// </summary>
public class IconInfo {

    /// <summary>
    /// Create a icon indexed by Key with the indicated relative path, alternate text, and optional CSS class.
    /// </summary>
    /// <param name="key">The string key to index into the icon, case-insensitive.</param>
    /// <param name="imagePath">The path to the icon, typically an SVG in the /img/ directory.</param>
    /// <param name="alternateText">The alternate text that describes the icon for screen readers.</param>
    /// <param name="cssClass">An optional additional class to apply to `img` tags, such as 'icon' or 'glyph'.</param>
    /// <param name="svgRenderType">An optional instruction on how to render SVG images.</param>
    public IconInfo(string key, string imagePath, string alternateText, string? cssClass = null, SvgRenderType svgRenderType = SvgRenderType.Document)
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
    /// <param name="iconClass">A required class to apply to `i` tags, such as 'fas fa-edit'.</param>
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
    public SvgRenderType SvgRenderType { get; set; } = SvgRenderType.Document;

    /// <summary>
    /// If the icon is an SVG, this is the body of the SVG as it would appear inside the `svg` tag
    /// of the database.  The outer tag is therefore `subject` and not `svg`.
    /// </summary>
    /// <remarks>
    /// This is not defined by the user when creating a IconInfo.  Instead when the theme is loaded
    /// all the SVG files are identified, loaded and this property is populated.
    /// </remarks>
    internal string SvgDatabaseBody { get; set; } = string.Empty;

    /// <summary>
    /// If the icon is an SVG, this is the reformatted SVG body to insert into the page that uses a
    /// reference to the database version of the SVG.
    /// </summary>
    internal string SvgInlineBody { get; set; } = string.Empty;

}
