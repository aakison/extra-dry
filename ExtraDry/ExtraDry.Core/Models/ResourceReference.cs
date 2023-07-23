namespace ExtraDry.Core;

/// <summary>
/// A reference to a resource suitable for sending through an API, as for example the return value 
/// of a Create method.
/// </summary>
public class ResourceReference : IResourceIdentifiers {

    /// <summary>
    /// The type of resource that has been created.
    /// </summary>
    [Obsolete("Not strictly needed, prune from existing code.")]
    public string Type { get; set; } = string.Empty;

    /// <inheritdoc cref="IResourceIdentifiers.Slug" />
    public string Slug { get; set; } = string.Empty;

    /// <inheritdoc cref="IResourceIdentifiers.Uuid" />
    public Guid Uuid { get; set; } = Guid.Empty;

    /// <inheritdoc cref="IResourceIdentifiers.Title" />
    public string Title { get; set; } = string.Empty;
}

/// <summary>
/// A strongly typed version of a ResourceReference to a resource suitable for sending through an 
/// API, as for example the return value of a Create method.
/// </summary>
public class ResourceReference<T> : ResourceReference where T : IResourceIdentifiers {

    /// <summary>
    /// Create a reference to an entity that implements IWebIdentifier
    /// </summary>
    public ResourceReference(T entity)
    {
        Slug = entity.Slug;
        Uuid = entity.Uuid;
        Title = entity.Title;
    }

}
