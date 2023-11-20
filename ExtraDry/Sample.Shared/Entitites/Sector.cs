using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

/// <summary>
/// Represents a service that a company may provide.
/// This is for properties that may appear as Enums, but have additional data associated with them.
/// </summary>
[DimensionTable]
[DeleteRule(DeleteAction.Recycle, nameof(State), SectorState.Inactive, SectorState.Active)]
public class Sector : IResourceIdentifiers {

    private const string DefaultGroup = "Not a group";

    /// <summary>
    /// A locally unique identifier, internal use only.
    /// </summary>
    [Key]
    [Rules(RuleAction.Ignore)]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The globally unique identifier for this sector.
    /// </summary>
    [Rules(RuleAction.Ignore)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The code for the sector, which is trivially the Uuid.
    /// </summary>
    public string Code => Uuid.ToString();

    /// <summary>
    /// The title of the sector
    /// </summary>
    /// <example>Commerical Electrical Services</example>
    [Required, StringLength(50)]
    [Display(Name = "Title", ShortName = "Title")]
    [Filter(FilterType.Contains)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// A group of sectors that logically go together for reporting.
    /// </summary>
    /// <example>Fire, Safety &amp; Security</example>
    [Required, StringLength(50)]
    [Display(Name = "Group", ShortName = "Group")]
    [Filter(FilterType.Contains)]
    [Statistics(Stats.Distribution)]
    [DefaultValue(DefaultGroup)]
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// The description of the sector.
    /// </summary>
    /// <example>Provides licensed electrical works for commercial facilities.</example>
    [StringLength(250)]
    [Display(Name = "Description")]
    [Filter(FilterType.Contains)]
    public string Description { get; set; } = string.Empty;

    [NotMapped]
    public string Caption => $"Sector {Code}";

    /// <summary>
    /// The current status of the sector.
    /// </summary>
    [Filter(FilterType.Equals)]
    [Statistics(Stats.Distribution)]
    public SectorState State { get; set;  }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

    [NotMapped]
    public string Slug { get => Title.ToLower().Replace(' ', '-'); set { } }

    /// <summary>
    /// Display title for the sector.
    /// </summary>
    public override string ToString() => Title;

    /// <summary>
    /// Entity equality comparer, as uniquely defined by the `Uuid`.
    /// </summary>
    public override bool Equals(object? obj) => (obj as Sector)?.Uuid == Uuid;

    /// <summary>
    /// Entity hash code, as uniquely defined by the `Uuid`.
    /// </summary>
    public override int GetHashCode() => Uuid.GetHashCode();

}
