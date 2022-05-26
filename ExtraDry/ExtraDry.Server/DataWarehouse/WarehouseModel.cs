using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.DataWarehouse;


public class WarehouseModel {

    public IList<Table> Facts { get; set; } = new List<Table>();

    public IList<Table> Dimensions { get; set; } = new List<Table>();

    public string ToSql() => SqlGenerator.Generate(this);

}
