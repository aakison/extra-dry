namespace Sample.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
[DimensionTable("Company Status")]
public enum CompanyStatus {

    [Display(Order = 0, Description = "Company is active")]
    Active = 0,

    [Display(Order = 2, Description = "Company not currently used, but still exists.")]
    Inactive = 1,

    [Display(Order = 1, Description = "Company doesn't exist, but is linked to historic records.", AutoGenerateField = false)]
    Deleted = 2,

    [Display(Description = "Trying to get money out of a turnip.")]
    InArbitration = 3,

}
