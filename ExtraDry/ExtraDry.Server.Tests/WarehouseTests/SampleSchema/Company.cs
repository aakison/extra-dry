#nullable disable // EF Model Class

using ExtraDry.Core.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

[FactTable]
public class Company : INamedSubject {

    [Key]
    [JsonIgnore]
    [Rules(RuleAction.Ignore)]
    public int Id { get; set; }

    [Rules(RuleAction.Ignore)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [NotMapped]
    [Display(GroupName = "Summary")]
    public string Caption => $"Company {Code}";

    [Display(Name = "Name", ShortName = "Name", GroupName = "Summary")]
    [Filter(FilterType.Contains)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Title { get; set; }

    [Display(Name = "Code", GroupName = "Summary")]
    [Filter(FilterType.Equals)]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Measure("For testing, should be ignored as type is `string`.")]
    public string Code { get; set; }

    [Display(Name = "Status", ShortName = "Status", GroupName = "Status")]
    [Rules(RuleAction.Allow)]
    [Filter]
    public CompanyStatus Status { get; set; }

    [Display]
    [MaxLength(1000)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Description { get; set; }

    public decimal Gross { get; set; }

    public decimal SalesMargin { get; set; }

    [Measure("Big Bucks")]
    public decimal AnnualRevenue { get; set; }

    public decimal GrossSalesLessCOGS { get; set; }

    [Display]
    [Rules(RuleAction.Allow)]
    public BankingDetails BankingDetails { get; set; } = new BankingDetails();

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [Display(GroupName = "Status")]
    [Rules(RuleAction.Block)]
    public VersionInfo Version { get; set; } = new VersionInfo();

}
