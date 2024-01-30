namespace ExtraDry.Blazor.Models;

/// <summary>
/// Context passed through to create a hyper link
/// </summary>
public class HyperLinkContext
{
    /// <summary>
    /// The Url for the hyperlink to reference
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The alternate text to display on the link
    /// </summary>
    public string AltText { get; set; } = string.Empty;

    /// <summary>
    /// The display CSS class to attach to the hyperlink
    /// </summary>
    public string DisplayClass { get; set; } = string.Empty;
}
