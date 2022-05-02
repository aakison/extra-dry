#nullable disable // EF Model Class

using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

/// <summary>
/// Represents a service that a company may provide.
/// This is for properties that may appear as Enums, but have additional data associated with them.
/// </summary>
[Dimension]    
public class Sector : INamedSubject {

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
    [Required]
    [MaxLength(50)]
    [Display(Name = "Title", ShortName = "Title")]
    [Filter(FilterType.Contains)]
    public string Title { get; set; }

    /// <summary>
    /// The description of the sector.
    /// </summary>
    /// <example>Provides licensed electrical works for commercial facilities.</example>
    [MaxLength(250)]
    [Display(Name = "Description")]
    [Filter(FilterType.Contains)]
    public string Description { get; set; }

    [NotMapped]
    public string Caption => $"Sector {Code}";

    /// <summary>
    /// The current status of the sector.
    /// </summary>
    [Rules(DeleteValue = SectorState.Inactive)]
    [Filter(FilterType.Equals)]
    public SectorState State { get; set;  }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

    /// <summary>
    /// Display title for the sector.
    /// </summary>
    public override string ToString() => Title;

    /// <summary>
    /// Entity equality comparer, as uniquely defined by the `Uuid`.
    /// </summary>
    public override bool Equals(object obj) => (obj as Sector)?.Uuid == Uuid;

    /// <summary>
    /// Entity hash code, as uniquely defined by the `Uuid`.
    /// </summary>
    public override int GetHashCode() => Uuid.GetHashCode();

}
