namespace ExtraDry.Core;

/// <summary>
/// Place with the first property in a group to provide logical separation between groups of properties.
/// In the UI, this will create forms that have headers between properties.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HeaderAttribute : Attribute {

    /// <summary>
    /// Create a new header with the specified title.
    /// </summary>
    public HeaderAttribute(string title)
    {
        Title = title;
    }

    /// <summary>
    /// The title that is displayed, typically in an HTML fieldset legend.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The optional description that is placed on the header in the form.
    /// </summary>
    public string? Description { get; set; } 
}
