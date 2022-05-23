using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class SpokeBuilder : ColumnBuilder {

    internal SpokeBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) 
        : base(tableBuilder, entityType, propertyInfo) 
    {
        TargetDimension = tableBuilder.WarehouseBuilder.Dimension(propertyInfo.PropertyType);
        SetName(TargetDimension.HasKey().ColumnName);
        SetType(ColumnType.Integer);
    }

    public SpokeBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Integer;
    }

    internal override Column Build()
    {
        return new Column(ColumnType.Integer, ColumnName) {
            Nullable = false,
            PropertyInfo = PropertyInfo,
            Reference = new Reference(TargetDimension.TableName, TargetDimension.HasKey().ColumnName),
        };
    }

    public DimensionTableBuilder TargetDimension { get; set; }

}
