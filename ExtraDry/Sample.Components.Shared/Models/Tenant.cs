using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Components;

/// <summary>
/// Represents a single tenant in a multi-tenanted system.  These are facades 
/// </summary>
public class Tenant : IResourceIdentifiers, ITenanted, IAudited, IRevisioned
{

    /// <inheritdoc/>
    [JsonIgnore]
    public string Partition {
        get => Slug;
        set => Slug = value;
    }

    /// <inheritdoc/>
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.IgnoreDefaults), Required, Key]
    public Guid Uuid { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(StringLength.Word), Slug]
    public string Slug { get; set; } = "";

    /// <inheritdoc/>
    [Required, StringLength(StringLength.Line)]
    public string Title { get; set; } = "";

    /// <summary>
    /// The plan that the tenant is subscribed to.  This is a string value that is for ABAC 
    /// authorization of APIs based on a product bundle.
    /// </summary>
    [Filter(FilterType.Equals), StringLength(StringLength.Words)]
    public string Plan { get; set; } = "";

    /// <inheritdoc/>
    [JsonIgnore]
    [Rules(RuleAction.Block)]
    public UserTimestamp Audit { get; set; } = new();

    /// <inheritdoc/>
    [Rules(RuleAction.Ignore)]
    public UserTimestamp Revision { get; set; } = new();

}
