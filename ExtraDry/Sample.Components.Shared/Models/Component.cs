using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;

namespace Sample.Components;

/// <summary>
/// The component is a type of cross-functional entity that supports common functional
/// requirements of Attachments, Conversations, Searching and Tagging.
/// </summary>
public class Component : IResourceIdentifiers, ITenanted, IAudited, IRevisioned, IAttributed
{
    /// <inheritdoc/>
    public string Partition { 
        get => Tenant;
        set { }
    }

    /// <summary>
    /// The tenant that the component is associated with.
    /// </summary>
    public string Tenant { get; set; } = "";

    /// <inheritdoc/>
    [Key]
    public Guid Uuid { get; set; }

    /// <inheritdoc/>
    public string Slug { get; set; } = "";

    /// <inheritdoc/>
    public string Title { get; set; } = "";

    /// <summary>
    /// The description of the Component, useful as a sub-title or summary in a list, so 
    /// denormalized version is also captured here.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// The type of the component that is stored, useful for finding the source microservice for
    /// the component.  (Same concept as a 'discriminator' in EF Core, but that name is reserved.)
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// A collection of attributes that are associated with the component.  These will vary by
    /// `Type` and are required for ABAC security rules.  
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; } = [];

    /// <inheritdoc/>
    public UserTimestamp Audit { get; set; } = new();

    /// <inheritdoc/>
    public UserTimestamp Revision { get; set; } = new();

}
