#nullable enable

using ExtraDry.Core.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

[FactTable("Worker Bees")]
public class Employee
{
    [Key]
    [Rules(RuleAction.Block)]
    [JsonIgnore]
    public int Id { get; set; }

    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Required, StringLength(50)]
    [Rules(RuleAction.Allow)]
    [Display(Name = "First Name", ShortName = "First Name")]
    [Filter(FilterType.Equals)]
    [Measure]
    public string? FirstName { get; set; }

    [Required, StringLength(50)]
    [Rules(RuleAction.Allow)]
    [Display(Name = "Last Name", ShortName = "Last Name")]
    [Filter(FilterType.StartsWith)]
    [Measure("Last Name")]
    public string? LastName { get; set; }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

}
