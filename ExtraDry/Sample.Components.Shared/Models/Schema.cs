using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Components;

/// <summary>
/// Represents a schema for the component services. In particular, stores a list of all known tags,
/// which is useful for auto-complete.
/// </summary>
public class Schema : ITenanted
{
    /// <inheritdoc />
    [JsonIgnore]
    public string Tenant { get; set; } = "";

    /// <inheritdoc />
    [Key]
    public Guid Uuid { get; set; }

    /// <summary>
    /// List of all known tags. This does not represent a list of all tags for all components, just
    /// ones that have been added to the schema and not deleted.
    /// </summary>
    public List<string> Tags { get; set; } = [];
}
