#nullable disable // EF Model Class

using Sample.Shared.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

[FactTable]
[Format(Icon = "building")]
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
    [Measure]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Title { get; set; }

    [Display(Name = "Code", GroupName = "Summary")]
    [Filter(FilterType.Equals)]
    [Measure]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    public string Code { get; set; }

    [Display(Name = "Status", ShortName = "Status", GroupName = "Status")]
    [Rules(RuleAction.Allow)]
    [Filter]
    [Measure]
    public CompanyStatus Status { get; set; }

    [Display]
    [MaxLength(1000)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Description { get; set; }

    [Display]
    [Rules(RuleAction.Link)]
    [JsonConverter(typeof(SectorInfoJsonConverter))]
    public Sector PrimarySector { get; set; }

    [Display]
    [Rules(RuleAction.Link)]
    public List<Sector> AdditionalSectors { get; set; }

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
    public VersionInfo Version { get; set; } = new VersionInfo();

}
