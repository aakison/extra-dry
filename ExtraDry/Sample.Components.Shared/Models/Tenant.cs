using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Components;

public class Tenant : IResourceIdentifiers, ITenanted
{
    /// <inheritdoc/>
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block), Required, Key]
    public Guid Uuid { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(50)]
    public string Slug { get; set; } = "";

    /// <inheritdoc/>
    [JsonIgnore]
    public string Partition {
        get => Slug;
        set => Slug = value; 
    }

    /// <inheritdoc/>
    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    /// <summary>
    /// The plan that the tenant is subscribed to.  This is a string value that is for ABAC 
    /// authorization of APIs.
    /// </summary>
    [Filter(FilterType.Equals), StringLength(50)]
    public string Plan { get; set; } = "";

}
