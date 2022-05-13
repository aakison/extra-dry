using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class MeasureBuilder : ColumnBuilder {

    internal MeasureBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) : base(tableBuilder, entityType, propertyInfo)
    {
        MeasureAttribute = propertyInfo.GetCustomAttribute<MeasureAttribute>();
        ColumnName = MeasureAttribute?.Name ?? DataConverter.CamelCaseToTitleCase(propertyInfo.Name);
        if(PropertyInfo.PropertyType == typeof(decimal)) {
            ColumnType = ColumnType.Decimal;
        }
        else if(PropertyInfo.PropertyType == typeof(double) || PropertyInfo.PropertyType == typeof(float)) {
            ColumnType = ColumnType.Double;
        }
        else {
            ColumnType = ColumnType.Integer;
        }
    }

    public MeasureBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public MeasureBuilder HasColumnType(ColumnType type)
    {
        SetType(type);
        return this;
    }

    internal override Column Build()
    {
        return new Column(ColumnType, ColumnName) {
            Nullable = false,
            PropertyInfo = PropertyInfo,
        };
    }

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Decimal || type == ColumnType.Double || type == ColumnType.Integer;
    }

    private MeasureAttribute? MeasureAttribute { get; set; }

}
