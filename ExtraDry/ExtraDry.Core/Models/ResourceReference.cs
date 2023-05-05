namespace ExtraDry.Core;

/// <summary>
/// A reference to a resource suitable for sending through an API, as for example the return value 
/// of a Create method.
/// </summary>
public class ResourceReference : IResourceIdentifiers {

    /// <summary>
    /// The type of resource that has been created.
    /// </summary>
    /// <example>Trade</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// A user readable reference to the created resource. Used in the URL to access the new resource, but may change.
    /// </summary>
    /// <example>widget</example>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// A universally unique identifier for this new resource. It is used in the permalink.
    /// </summary>
    /// <example>e8b79f39-3398-4aed-9339-7250166204e5</example>
    public Guid Uuid { get; set; } = Guid.Empty;
}

/// <summary>
/// A strongly typed version of an ApiReference to a resource suitable for sending through an API, 
/// as for example the return value of a Create method.
/// </summary>
public class ResourceReference<T> : ResourceReference where T : IResourceIdentifiers {

    /// <summary>
    /// Create a reference to an entity that implements IWebIdentifier
    /// </summary>
    public ResourceReference(T entity)
    {
        Slug = entity.Slug;
        Uuid = entity.Uuid;
        Type = entity.GetType().Name;
    }

}
