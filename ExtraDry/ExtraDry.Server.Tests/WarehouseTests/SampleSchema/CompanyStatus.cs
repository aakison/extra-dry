using ExtraDry.Core.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

[JsonConverter(typeof(JsonStringEnumConverter))]
[DimensionTable]
public enum CompanyStatus {

    [Display(Order = 0, GroupName = "ForDisplay")]
    Active = 0,

    [Display(Order = 2, GroupName = "ForDisplay")]
    Inactive = 1,

    Deleted = 2,

}
