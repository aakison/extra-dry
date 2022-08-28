#nullable enable

namespace ExtraDry.Core;

/// <summary>
/// Represents a property or method that defines a navigation link for a ViewModel.
/// Properties are the most common and will return the URI for the navigation link.
/// These URIs can be either static or dynamic, such as a property that has an ID in the link.
/// Methods can also be used for navigation, where instead of changing the URI to the link
/// the method is executed, allowing any advanced use-case for navigation.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class NavigationAttribute : Attribute {

    /// <summary>
    /// Create a navigation link with a required title.
    /// If a title is not provided, then the member name is used to construct the title
    /// by converting PascalCase to Title Case.
    /// </summary>
    public NavigationAttribute(string title = "")
    {
        Title = title;
    }

    /// <summary>
    /// Navigations, when there are many, can be organized into separate groups.
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// The optional icon to render when creating the navigation link.
    /// Icon details are looked up in by key the `Theme` Icons collection, or can be a URL.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// The title that is displayed for the navigation link.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The optional subtitle for the displayed link, 
    /// typically only used for top-most navigations.
    /// </summary>
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// The order that the Navigation should be displayed in.  
    /// If not provided, then defaults to order defined in the class.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// An optional regex string that determines if a CSS class of "active" is applied
    /// to the navigation link based on the current URI.
    /// </summary>
    public string? ActiveMatch { get; set; }

}
