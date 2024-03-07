using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Components;

/// <summary>
/// The component is a type of cross-functional entity that supports common functional
/// requirements of Attachments, Conversations, Searching and Tagging.
/// </summary>
public class Component : IResourceIdentifiers, ITenanted, ICreatingCallback
{
    /// <inheritdoc/>
    [JsonIgnore]
    public string Partition { get; set; } = "";

    /// <inheritdoc/>
    [Key]
    public Guid Uuid { get; set; }

    /// <inheritdoc/>
    public string Slug { get; set; } = "";

    public string Code { get; set; } = "";

    /// <inheritdoc/>
    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public string? Keywords { get; set; }

    public string? FullText { get; set; }

    public string Discriminator { get; set; } = "";

    [JsonIgnore]
    public Guid TenantUuid { get; set; }

    public Dictionary<string, string> Attributes { get; set; } = [];

    public Task OnCreatingAsync()
    {
        if(Uuid == Guid.Empty) {
            Uuid = Guid.NewGuid();
        }
        return Task.CompletedTask;
    }

}
