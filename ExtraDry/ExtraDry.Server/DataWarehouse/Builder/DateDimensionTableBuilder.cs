using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class DateDimensionTableBuilder : DimensionTableBuilder {

    internal DateDimensionTableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity) 
        : base(warehouseBuilder, entity)
    {
        DateDimensionTableAttribute = entity.GetCustomAttribute<DateDimensionTableAttribute>()!;
    }

    public override DateDimensionTableBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public DateDimensionTableBuilder HasMinimumValue(DateTime from)
    {
        minimumValue = from;
        return this;
    }

    public DateDimensionTableBuilder HasMaximumValue(DateTime to)
    {
        maximumValue = to;
        return this;
    }

    public DateTime MinimumValue => minimumValue;
    private DateTime minimumValue = new DateTime(DateTime.UtcNow.Year, 1, 1);

    public DateTime MaximumValue => maximumValue;
    private DateTime maximumValue = new DateTime(DateTime.UtcNow.Year, 12, 31);

    private DateDimensionTableAttribute DateDimensionTableAttribute { get; set; }

}

public class DateDimensionTableBuilder<T> : DateDimensionTableBuilder {

    internal DateDimensionTableBuilder(WarehouseModelBuilder warehouseModel, Type entity) 
        : base(warehouseModel, entity)
    { 
    }

    public AttributeBuilder Attribute<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        var property = ExtractPropertyName(propertyExpression);
        return Attribute(property.Name);
    }

    private static PropertyInfo ExtractPropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        if(propertyExpression.Body is not MemberExpression member) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        if(member.Member is not PropertyInfo property) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        return property;
    }

}
