using ExtraDry.Core.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

[JsonConverter(typeof(JsonStringEnumConverter))]
[DimensionTable("Geo Status")]
public enum RegionStatus
{
    [Display(ShortName = "ACT", Description = "Region is active.")]
    Active = 0,

    [Display(ShortName = "INA", Description = "Region is not currently used, but still exists.")]
    Inactive = 1,

    [Display(ShortName = "DEL", Description = "Region no longer exists, but is linked to historic records.")]
    Deleted = 2,
}
