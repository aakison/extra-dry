using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class SpokeBuilder : ColumnBuilder {

    internal SpokeBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) 
        : base(tableBuilder, entityType, propertyInfo) 
    {
        TargetDimension = tableBuilder.WarehouseBuilder.Dimension(propertyInfo.PropertyType);

        var allPropertiesOfType = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => e.PropertyType == propertyInfo.PropertyType).ToList();
        if(allPropertiesOfType.Count > 1) {
            // Multiple properties on dimension need additional clarification, not just target's primary key name.
            SetName($"{DataConverter.CamelCaseToTitleCase(propertyInfo.Name)} {TargetDimension.HasKey().ColumnName}");
        }
        else if(tableBuilder.HasKey().ColumnName == TargetDimension.HasKey().ColumnName) {
            // Self referential table, primary key can't conflict with name of dimenion key.
            SetName($"{DataConverter.CamelCaseToTitleCase(propertyInfo.Name)} {TargetDimension.HasKey().ColumnName}");
        }
        else {
            // Assign property name to match the target dimension primary key, easier for tools and people to correlate.
            SetName(TargetDimension.HasKey().ColumnName);
        }

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
