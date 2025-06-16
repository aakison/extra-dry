namespace ExtraDry.Blazor.Models;

/// <summary>
/// Context passed through to create a hyper link
/// </summary>
public class HyperlinkContext
{
    public HyperlinkContext(string href)
    {
        if(string.IsNullOrEmpty(href)) {
            throw new ArgumentNullException(nameof(href), "The href must be populated");
        }

        Href = href;
    }

    /// <summary>
    /// The Url for the hyperlink to reference. Always displayed to the user, but only used as a
    /// link if the `Action` is not set.
    /// </summary>
    public string Href { get; }

    /// <summary>
    /// The tooltip text to display on the link
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The display CSS class to attach to the hyperlink
    /// </summary>
    public string DisplayClass { get; set; } = string.Empty;

    /// <summary>
    /// If present, this will be a action that is called instead of using the default navigation.
    /// </summary>
    /// <example>
    /// async e => Console.WriteLine($"Hyperlink clicked: {e.Href}");
    /// </example>
    public Func<HyperlinkContext, Task>? Action { get; set; }
}
