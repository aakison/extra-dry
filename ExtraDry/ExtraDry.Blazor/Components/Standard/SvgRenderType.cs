namespace ExtraDry.Blazor;

/// <summary>
/// For SVG in the IconInfo, determines how the icon is rendered to the page.
/// </summary>
public enum SvgRenderType
{
    /// <summary>
    /// Renders the the SVG as the SRC target of an IMG tag.  This will fetch the SVG from the 
    /// server using the browser's caching rules.  This is useful for large images that are only
    /// used on a few pages, e.g. a 404 image.
    /// </summary>
    ImgReference,

    /// <summary>
    /// Renders the SVG inline in the page.  This will fetch the SVG from the server using a
    /// local in-app cache which is fetched at startup.  
    /// </summary>
    Inline,

    /// <summary>
    /// Downloads the SVG from the server and stores the image in a non-visibl SVG tag on the page.
    /// Each time the image it is rendered inline as an SVG with an embedded USE tag which 
    /// references common content from the global SVG.  This is useful for small images that are
    /// used repeatedly across pages, e.g. a dropdown chevron.
    /// </summary>
    SymbolDatabase,
}
