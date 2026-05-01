using System.ComponentModel;

namespace ExtraDry.Core;

/// <summary>
/// A reference to a resource suitable for sending through an API, as for example the return value
/// of a Create method.
/// </summary>
public class ResourceReference
{
    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference()
    { }

    /// <inheritdoc cref="ResourceReference" />
    public ResourceReference(IUniqueIdentifier target)
    {
        Type = target.GetType().Name;
        Uuid = target.Uuid;
        if(target is IResourceIdentifiers resource) {
            Title = resource.Title;
        }
        if(target is ISlug slug) {
            Slug = slug.Slug;
        }
        if(target is ITenanted tenanted) {
            Tenant = tenanted.Tenant;
        }
    }

    /// <summary>
    /// The type of resource that has been created.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <inheritdoc cref="ITenanted.Tenant" />
    public string? Tenant { get; set; }

    /// <inheritdoc cref="IUniqueIdentifier.Uuid" />
    public Guid Uuid { get; set; } = Guid.Empty;

    /// <inheritdoc cref="ISlug.Slug" />
    public string? Slug { get; set; }

    /// <inheritdoc cref="IResourceIdentifiers.Title" />
    public string? Title { get; set; }

}
