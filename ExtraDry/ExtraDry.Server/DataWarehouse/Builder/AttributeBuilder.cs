using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class AttributeBuilder : ColumnBuilder {

    internal AttributeBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) 
        : base(tableBuilder, entityType, propertyInfo)
    {
        AttributeAttribute = propertyInfo.GetCustomAttribute<AttributeAttribute>();
        if(AttributeAttribute?.Name != null) {
            SetName(AttributeAttribute.Name);
        }

        if(PropertyInfo.PropertyType == typeof(string)) {
            SetType(ColumnType.Text);
        }
        else {
            throw new NotImplementedException();
        }

        var notMapped = propertyInfo.GetCustomAttribute<NotMappedAttribute>();
        if(notMapped != null && AttributeAttribute == null) {
            // [NotMapped] will ignore unless a [Attribute] attribute is also present.
            SetIgnore(true);
        }
        var ignored = propertyInfo.GetCustomAttribute<AttributeIgnoreAttribute>();
        if(ignored != null) {
            // [MeasureIgnore] will always ignore, even if [Measure] is present.
            SetIgnore(true); 
        }
    }

    public AttributeBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public AttributeBuilder HasColumnType(ColumnType type)
    {
        SetType(type);
        return this;
    }

    public AttributeBuilder HasIgnore(bool ignore = true)
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
        return type == ColumnType.Text;
    }

    private AttributeAttribute? AttributeAttribute { get; set; }

}
