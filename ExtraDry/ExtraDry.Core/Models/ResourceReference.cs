using System.ComponentModel;

namespace ExtraDry.Core;

/// <summary>
/// A reference to a resource suitable for sending through an API, as for example the return value 
/// of a Create method.
/// </summary>
public class ResourceReference
{

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference() { }

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference(IUniqueIdentifier target)
    {
        Type = target.GetType().Name;
        Uuid = target.Uuid;
        if(target is IResourceIdentifiers resource) {
            Slug = resource.Slug;
            Title = resource.Title;
        }
        if(target is ITenanted tenanted) {
            Tenant = tenanted.Tenant;
        }
    }

    /// <summary>
    /// The type of resource that has been created.
    /// </summary>
    [ReadOnly(true)]
    public string Type { get; set; } = string.Empty;

    /// <inheritdoc />
    [ReadOnly(true)]
    public Guid Uuid { get; set; } = Guid.Empty;

    /// <inheritdoc />
    [ReadOnly(true)]
    public string? Slug { get; set; }

    /// <inheritdoc />
    [ReadOnly(true)]
    public string? Title { get; set; }

    /// <summary>
    /// The tenant that the resource is assigned to.
    /// </summary>
    public string? Tenant { get; set; }


}

/// <summary>
/// A strongly typed version of a ResourceReference to a resource suitable for sending through an 
/// API, as for example the return value of a Create method.
/// </summary>
public class ResourceReference<T> : ResourceReference where T : IUniqueIdentifier
{

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference() { }

    /// <summary>
    /// Create a reference to an entity that implements IWebIdentifier
    /// </summary>
    public ResourceReference(T entity) : base(entity) { }

}
