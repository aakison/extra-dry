using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OwnershipStructure
{
    Private,

    Public,

    Hybrid
}

[Format(Icon = "company")]
[FactTable, DimensionTable]
[DeleteRule(DeleteAction.Recycle, nameof(Status), CompanyStatus.Deleted, CompanyStatus.Active)]
public class Company : IResourceIdentifiers
{
    [Display(AutoGenerateField = false)]
    [Key]
    [JsonIgnore]
    [Rules(RuleAction.Ignore)]
    public int Id { get; set; }

    [Display(AutoGenerateField = false)]
    [Rules(RuleAction.Ignore)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Display(Name = "Code", GroupName = "Summary", Order = 1)]
    [Filter(FilterType.Equals)]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Required, StringLength(24)]
    public string Slug { get; set; } = string.Empty;

    [NotMapped]
    [Display(GroupName = "Summary")]
    public string Caption => $"Company {Slug}";

    /// <summary>
    /// Official incorporated name of company, as listed in Dun &amp; Bradstreet
    /// </summary>
    /// <example>Alphabet, Inc.</example>
    [Display(Name = "Name", ShortName = "Name", GroupName = "Summary")]
    [Filter(FilterType.Contains)]
    [Rules(RuleAction.IgnoreDefaults)]
    [Required, StringLength(80)]
    public string Title { get; set; } = "";

    [Display(Name = "Status", ShortName = "Status", GroupName = "Status")]
    [Rules(RuleAction.Allow)]
    [Filter]
    public CompanyStatus Status { get; set; }

    [Display(AutoGenerateField = false)]
    [StringLength(500)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Description { get; set; } = "";

    [Display(Name = "Primary Sector", ShortName = "Sector")]
    [Rules(RuleAction.Link)]
    [JsonConverter(typeof(ResourceReferenceConverter<Sector>))]
    public Sector? PrimarySector { get; set; }

    [Display]
    [Rules(RuleAction.Link)]
    public List<Sector> AdditionalSectors { get; set; } = [];

    [Rules(RuleAction.Allow)]
    [Filter]
    public OwnershipStructure Ownership { get; set; }

    [Display]
    [Phone, StringLength(24)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string ContactPhone { get; set; } = "";

    [EmailAddress, StringLength(100)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string ContactEmail { get; set; } = "";

    [Precision(18, 2)]
    [InputFormat(Icon = "currency")]
    public decimal AnnualRevenue { get; set; }

    [Display(Prompt = "0.00", Description = "Clamped to range [0, 120]")]
    [Range(0, 120)]
    [Precision(18, 2)]
    public decimal? SalesMargin { get; set; }

    [Filter]
    [Display(Name = "Incorporation Date", ShortName = "Inc Date", Description = "Date stored as DateTime, informed by InputFormat")]
    [InputFormat(DataTypeOverride = typeof(DateOnly))]
    public DateTime IncorporationDate { get; set; }

    [Display(Name = "Dissolution Date", ShortName = "Diss Date", Description = "Date stored as DateTime?, informed by InputFormat")]
    public DateTime? DissolutionDate { get; set; }

    [Display(Name = "Start of Business Hours", ShortName = "Opens", Description = "Time stored as TimeOnly, informed by type")]
    public TimeOnly StartOfBusinessHours { get; set; }

    [Display(Name = "End of Business Hours", ShortName = "Closes", Description = "Time stored as TimeOnly?, informed by type")]
    public TimeOnly? EndOfBusinessHours { get; set; }

    [Display(Name = "CEO Birthday Holiday", ShortName = "CEO Bday", Description = "Date stored as DateOnly, informed by type")]
    public DateOnly CeoBirthdayHoliday { get; set; }

    public DateTime Timestamp { get; } = DateTime.UtcNow;

    [Display(Name = "Last Trademark Review", ShortName = "Trademark", Description = "Date stored as String, informed by InputFormat")]
    [InputFormat(DataTypeOverride = typeof(DateOnly))]
    public string LastTrademarkReview { get; set; } = "";

    public int NumberOfEmployees { get; set; }

    [Display(Description = "Nullable int field with 'none' as display value for null")]
    [DisplayFormat(NullDisplayText = "none")]
    public int? NumberOfContractors { get; set; }

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

    [Display(AutoGenerateField = false)]
    [JsonPropertyName("fields")]
    public ExpandoValues CustomFields { get; set; } = [];
}
