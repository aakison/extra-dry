namespace ExtraDry.Core;

/// <summary>
/// For SVG in the IconInfo, determines how the icon is rendered to the page.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SvgRenderType
{
    /// <summary>
    /// Generates a "atlas.svg" file on the server that contains all atlas SVGs.  Each time the
    /// image is used, it is rendered inline as an SVG with a USE element which references common
    /// content from the global SVG. This is useful for small images that are used repeatedly
    /// across pages, e.g. a dropdown chevron.
    /// </summary>
    Atlas,

    /// <summary>
    /// Renders the SVG as the reference SRC target of an IMG tag. This will fetch the SVG from the
    /// server using the browser's caching rules. This is useful for large images that are only
    /// used on a few pages, e.g. a 404 image.
    /// </summary>
    Reference,

}
