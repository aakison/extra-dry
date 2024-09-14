namespace ExtraDry.Core;

/// <summary>
/// Represents a resource that has identifiers that are common on the web, in particular through 
/// RESTful APIs.
/// </summary>
public interface IResourceIdentifiers : IUniqueIdentifier
{

    /// <summary>
    /// A user readable reference to the created resource. Used in the URL to access the new 
    /// resource, but may change.
    /// </summary>
    /// <example>acme-widget</example>
    public string Slug { get; set; }

    /// <summary>
    /// The title of the resource that is suitable for displaying to users as a named reference. 
    /// </summary>
    /// <remarks>
    /// While this property is not strictly part of a reference, it massively improves the overall 
    /// performance of the system.  Resource references are typically listed within a enclosing 
    /// resource which needs to display this information to users through a UI.  In these cases
    /// having the information available and preventing potentially dozens of additional API calls
    /// is worth the trade-off.
    /// </remarks>
    /// <example>Acme Widget</example>
    public string Title { get; set; }

}
