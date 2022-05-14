using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class MeasureBuilder : ColumnBuilder {

    internal MeasureBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) : base(tableBuilder, entityType, propertyInfo)
    {
        MeasureAttribute = propertyInfo.GetCustomAttribute<MeasureAttribute>();
        if(MeasureAttribute?.Name != null) {
            SetName(MeasureAttribute.Name);
        }

        if(PropertyInfo.PropertyType == typeof(decimal)) {
            SetType(ColumnType.Decimal);
        }
        else if(PropertyInfo.PropertyType == typeof(double) || PropertyInfo.PropertyType == typeof(float)) {
            SetType(ColumnType.Double);
        }
        else {
            SetType(ColumnType.Integer);
        }

        var notMapped = propertyInfo.GetCustomAttribute<NotMappedAttribute>();
        if(notMapped != null && MeasureAttribute == null) {
            // [NotMapped] will ignore unless a [Measure] attribute is also present.
            SetIgnore(true);
        }
        var ignored = propertyInfo.GetCustomAttribute<MeasureIgnoreAttribute>();
        if(ignored != null) {
            // [MeasureIgnore] will always ignore, even if [Measure] is present.
            SetIgnore(true);
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

    public MeasureBuilder HasIgnore(bool ignore = true)
    {
        SetIgnore(ignore);
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
