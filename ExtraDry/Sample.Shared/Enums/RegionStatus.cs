namespace Sample.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Dimension("Region Status")]
public enum RegionStatus {

    [Display(Order = 0, Description = "Region is active")]
    Active = 0,

    [Display(Order = 2, Description = "Region is not currently used, but still exists.")]
    Inactive = 1,

    [Display(Order = 1, Description = "Region no longer exists, but is linked to historic records.")]
    Deleted = 2,

}
