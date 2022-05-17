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

        // Only one type supported for attributes now, URI and Guid map here, possibly everything, always?
        SetType(ColumnType.Text);

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

        HasLength(propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength
            ?? propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Length);
        if(Length == null && propertyInfo.PropertyType == typeof(Guid)) {
            HasLength(MaxGuidLength);
        }
        else if(Length == null && propertyInfo.PropertyType == typeof(Uri)) {
            HasLength(MaxUriLength);
        }
    }

    public AttributeBuilder HasLength(int? length)
    {
        SetLength(length);
        return this;
    }

    public AttributeBuilder HasName(string name)
    {
        SetName(name);
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
            Length = Length,
        };
    }

    internal static bool IsAttribute(PropertyInfo property)
    {
        var isAttribute = attributeTypes.Contains(property.PropertyType);
        // Add in explicit attributes.
        if(property.GetCustomAttribute<AttributeAttribute>() != null) {
            isAttribute = true;
        }
        return isAttribute;
    }

    private static readonly Type[] attributeTypes = new Type[] { typeof(string), typeof(Uri), typeof(Guid) };

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Text;
    }

    private AttributeAttribute? AttributeAttribute { get; set; }

    private const int MaxGuidLength = 36;

    // http://net-informations.com/q/mis/len.html
    private const int MaxUriLength = 2083;
}
