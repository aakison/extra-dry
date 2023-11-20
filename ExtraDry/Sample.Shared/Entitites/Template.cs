using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

/// <summary>
/// Represents a Template for user defined fields.
/// </summary>
[DimensionTable]
[DeleteRule(DeleteAction.Recycle, nameof(State), TemplateState.Inactive, TemplateState.Active)]
public class Template : IResourceIdentifiers {

    /// <summary>
    /// A locally unique identifier, internal use only.
    /// </summary>
    [Key]
    [Rules(RuleAction.Ignore)]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The globally unique identifier for this template.
    /// </summary>
    [Rules(RuleAction.Ignore)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The Target Type of the template
    /// </summary>
    /// <example>Company</example>
    [Required, StringLength(50)]
    [Display(Name = "Title", ShortName = "Title")]
    [Filter(FilterType.Contains)]
    public string Title { get => Schema.TargetType; set { Schema.TargetType = value; } }

    /// <summary>
    /// State of the Template.
    /// </summary>
    public TemplateState State { get; set; }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

    [NotMapped]
    public string Slug { get => Title.ToLower(); set { } }

    /// <inheritdoc cref="ExpandoSchema"/>
    public ExpandoSchema Schema { get; set; } = new();
}
