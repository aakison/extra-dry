using ExtraDry.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Components;

/// <summary>
/// A set of Metadata that are associated with a component.
/// </summary>
public class Metadata : ITenanted, IAudited, IRevisioned
{
    /// <inheritdoc />
    [JsonIgnore]
    public string Tenant { get; set; } = "";

    public string TenantId { get; set; } = "";

    /// <summary>
    /// The UUID for the Metadata which is aligned with the Component.
    /// </summary>
    [Key]
    public Guid Uuid { get; set; }

    /// <summary>
    /// The list of Metadata Tags.
    /// </summary>
    public Collection<string> Tags { get; set; } = [];

    /// <inheritdoc />
    public UserTimestamp Audit { get; set; } = new();

    /// <inheritdoc />
    public UserTimestamp Revision { get; set; } = new();
}
