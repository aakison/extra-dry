using Microsoft.EntityFrameworkCore;
using Sample.Shared.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

[Format(Icon = "building")]
[FactTable, DimensionTable]
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
    [Required, StringLength(80)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Code", GroupName = "Summary")]
    [Filter(FilterType.Equals)]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Required, StringLength(24)]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Status", ShortName = "Status", GroupName = "Status")]
    [Rules(RuleAction.Allow)]
    [Filter]
    public CompanyStatus Status { get; set; }

    [Display]
    [MaxLength(1000)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Description { get; set; } = string.Empty;

    [Display]
    [Rules(RuleAction.Link)]
    [JsonConverter(typeof(SectorInfoJsonConverter))]
    public Sector? PrimarySector { get; set; }

    [Display]
    [Rules(RuleAction.Link)]
    public List<Sector> AdditionalSectors { get; set; } = new();

    [Precision(18, 2)]
    public decimal AnnualRevenue { get; set; }

    [Precision(18, 2)]
    public decimal SalesMargin { get; set; }

    public DateTime IncorporationDate { get; set; }

    [Display]
    [Rules(RuleAction.Allow)]
    public BankingDetails BankingDetails { get; set; } = new BankingDetails();

    //[Display]
    //[Rules(RuleAction.Recurse)]
    //public ICollection<Video> Videos { get; set; } = new Collection<Video>();

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [Display(GroupName = "Status")]
    [Rules(RuleAction.Block)]
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

}
