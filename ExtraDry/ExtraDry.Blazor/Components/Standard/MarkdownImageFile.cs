namespace ExtraDry.Blazor;

/// <summary>
/// Represents an image file available for insertion into a Markdown editor. Used to populate
/// the image picker shown when the user clicks the insert-image button in Block mode.
/// </summary>
public class MarkdownImageFile
{
    /// <summary>
    /// The accessible title / alt text for the image, displayed in the picker and written as
    /// the alt attribute of the inserted &lt;img&gt; tag.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// The relative or absolute URL of the image, written as the src attribute of the inserted
    /// &lt;img&gt; tag.
    /// </summary>
    public string Url { get; set; } = "";
}
