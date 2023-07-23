namespace ExtraDry.Core;

/// <summary>
/// A reference to a resource suitable for sending through an API, as for example the return value 
/// of a Create method.
/// </summary>
public class ResourceReference {

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference() { }

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference(IResourceIdentifiers target)
    {
        Slug = target.Slug;
        Uuid = target.Uuid;
        Title = target.Title;
    }

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
    /// <remarks>No setter as titles should appear in OpenAPI as readonly.</remarks>
    public string Title { get; } = string.Empty;
}

/// <summary>
/// A strongly typed version of a ResourceReference to a resource suitable for sending through an 
/// API, as for example the return value of a Create method.
/// </summary>
public class ResourceReference<T> : ResourceReference where T : IResourceIdentifiers {

    /// <summary>
    /// Create a reference to an entity that implements IWebIdentifier
    /// </summary>
    public ResourceReference(T entity) : base(entity) { }

}
