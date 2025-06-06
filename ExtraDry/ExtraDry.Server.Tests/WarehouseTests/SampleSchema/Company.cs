﻿using ExtraDry.Core.DataWarehouse;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.Tests.WarehouseTests;

[FactTable, DimensionTable]
public class Company
{
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
    [StringLength(100)]
    public string Title { get; set; } = "";

    [Display(Name = "Code", GroupName = "Summary")]
    [Filter(FilterType.Equals)]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Measure("For testing, ignored as type is `string`.")]
    public string Code { get; set; } = "";

    [Display(Name = "Status", ShortName = "Status", GroupName = "Status")]
    [Rules(RuleAction.Allow)]
    [Filter]
    public CompanyStatus Status { get; set; }

    [Display]
    [StringLength(500)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string Description { get; set; } = "";

    [Display]
    [Phone, StringLength(24)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string ContactPhone { get; set; } = "";

    [Display]
    [EmailAddress, StringLength(100)]
    [Rules(RuleAction.IgnoreDefaults)]
    public string ContactEmail { get; set; } = "";

    [Display]
    [Precision(18, 2)]
    public decimal Gross { get; set; }

    [Display]
    [Precision(18, 2)]
    public decimal SalesMargin { get; set; }

    [Display]
    [Measure("Big Bucks")]
    [Precision(18, 2)]
    public decimal AnnualRevenue { get; set; }

    [Display]
    [Rules(RuleAction.Allow)]
    public BankingDetails BankingDetails { get; set; } = new BankingDetails();

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [Display(GroupName = "Status")]
    [Rules(RuleAction.Block)]
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

    // Not a measure as it's a field and not a property.
    [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "For testing purposes.")]
    public decimal field;
}
