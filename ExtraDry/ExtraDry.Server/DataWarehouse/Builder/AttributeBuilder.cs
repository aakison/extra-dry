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

        var type = PropertyInfo.PropertyType;
        if(type == typeof(int)) {
            SetType(ColumnType.Integer);
            SetDefault(0);
        }
        else if(type == typeof(DateTime)) {
            SetType(ColumnType.Integer);
            SetConverter(e => StandardConversions.DateTimeToSequence((DateTime)e));
            SetDefault(0);
        }
        else if(type == typeof(DateOnly)) {
            SetType(ColumnType.Date);
            //SetDefault(new DateOnly(1970, 1, 1));
        }
        else if(type == typeof(TimeOnly)) {
            SetType(ColumnType.Time);
            //SetDefault(new TimeOnly(0));
        }
        else {
            SetType(ColumnType.Text);
            SetDefault("_NULL_");
        }

        var notMapped = propertyInfo.GetCustomAttribute<NotMappedAttribute>();
        if(notMapped != null && AttributeAttribute == null) {
            // [NotMapped] will ignore unless a [Attribute] attribute is also present.
            SetIncluded(false);
        }
        var ignored = propertyInfo.GetCustomAttribute<AttributeIgnoreAttribute>();
        if(ignored != null) {
            // [MeasureIgnore] will always ignore, even if [Measure] is present.
            SetIncluded(false); 
        }

        HasLength(propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength
            ?? propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Length);
        if(type.IsEnum && ColumnType == ColumnType.Text) {
            var stats = new EnumStats(type);
            HasLength(stats.DisplayNameMaxLength());
        }
        else if(Length == null && propertyInfo.PropertyType == typeof(Guid)) {
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

    public AttributeBuilder HasConversion(Func<object, object> converter)
    {
        SetConverter(converter);
        return this;
    }

    public AttributeBuilder HasDefault(object @default)
    {
        SetDefault(@default);
        return this;
    }

    public AttributeBuilder IsIncluded(bool included)
    {
        SetIncluded(included);
        return this;
    }

    internal override Column Build()
    {
        return new Column(ColumnType, ColumnName, Converter) {
            Nullable = false,
            PropertyInfo = PropertyInfo,
            Length = Length,
            Default = Default,
        };
    }

    internal static bool IsAttribute(PropertyInfo property)
    {
        var isAttribute = attributeTypes.Contains(property.PropertyType);
        // Add in explicit attributes.
        if(property.GetCustomAttribute<AttributeAttribute>() != null) {
            isAttribute = true;
        }
        if(property.PropertyType.IsEnum) {
            isAttribute = true;
        }
        return isAttribute;
    }

    // Int is interesting here, considering not including it as a valid attribute ever, but then came across 'SortOrder', snap.
    // Not including decimal, float, long, etc unless an example justifying their use is identified.
    private static readonly Type[] attributeTypes = new Type[] { typeof(string), typeof(Uri), typeof(Guid), typeof(int), typeof(DateOnly), typeof(DateTime) };

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Text || type == ColumnType.Integer || type == ColumnType.Date || type == ColumnType.Time;
    }

    private AttributeAttribute? AttributeAttribute { get; set; }

    private const int MaxGuidLength = 36;

    // http://net-informations.com/q/mis/len.html
    private const int MaxUriLength = 2083;

}
