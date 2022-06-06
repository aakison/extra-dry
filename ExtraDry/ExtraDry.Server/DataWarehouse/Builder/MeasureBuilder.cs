using Microsoft.EntityFrameworkCore;
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
            SetIncluded(false);
        }
        var ignored = propertyInfo.GetCustomAttribute<MeasureIgnoreAttribute>();
        if(ignored != null) {
            // [MeasureIgnore] will always ignore, even if [Measure] is present.
            SetIncluded(false);
        }

        var precisionAttribute = propertyInfo.GetCustomAttribute<PrecisionAttribute>();
        if(precisionAttribute != null) {
            SetPrecision(precisionAttribute.Precision, precisionAttribute.Scale ?? 2);
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

    /// <summary>
    /// Set the precision for Decimal data types
    /// </summary>
    /// <param name="precision">The total number of decimal digits both before and after the decimal point.</param>
    /// <param name="scale">The number of decimal digits after the decimal point.</param>
    public MeasureBuilder HasPrecision(int precision, int scale)
    {
        SetPrecision(precision, scale);
        return this;
    }

    public MeasureBuilder IsIncluded(bool included)
    {
        SetIncluded(included);
        return this;
    }

    internal override Column Build()
    {
        var column = new Column(ColumnType, ColumnName) {
            Nullable = false,
            PropertyInfo = PropertyInfo,
            Length = Length,
        };
        if(ColumnType == ColumnType.Decimal) {
            column.Precision = $"{Precision.precision},{Precision.scale}";
        }
        return column;
    }

    internal static bool IsMeasure(PropertyInfo property)
    {
        var isMeasure = measureTypes.Contains(property.PropertyType);
        // Add in explicit measures.
        if(property.GetCustomAttribute<MeasureAttribute>() != null) {
            isMeasure = true;
        }
        return isMeasure;
    }

    private static readonly Type[] measureTypes = new Type[] { typeof(decimal), typeof(float), typeof(int),
        typeof(double), typeof(long), typeof(short), typeof(uint), typeof(sbyte) };

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Decimal || type == ColumnType.Double || type == ColumnType.Integer;
    }

    private MeasureAttribute? MeasureAttribute { get; set; }

}
