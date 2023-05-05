namespace ExtraDry.Core;

/// <summary>
/// Represents a resource that has identifiers that are common on the web, in particular through 
/// RESTful APIs.
/// </summary>
public interface IResourceIdentifiers {

    /// <summary>
    /// A user readable reference to the created resource. Used in the URL to access the new 
    /// resource, but may change.
    /// </summary>
    /// <example>widget</example>
    public string Slug { get; set; }

    /// <summary>
    /// A universally unique identifier for this new resource. It is used in the permalink.
    /// </summary>
    /// <example>e8b79f39-3398-4aed-9339-7250166204e5</example>
    public Guid Uuid { get; set; }
}
