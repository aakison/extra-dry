using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class DimensionTableBuilder : TableBuilder {

    internal DimensionTableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity) 
        : base(warehouseBuilder, entity)
    {
        DimensionTableAttribute = entity.GetCustomAttribute<DimensionTableAttribute>()!;

        var factTable = entity.GetCustomAttribute<FactTableAttribute>();

        var name = DimensionTableAttribute?.Name ?? DataConverter.CamelCaseToTitleCase(entity.Name);
        var key = $"{name} ID";
        if(factTable != null && factTable.Name == null && DimensionTableAttribute?.Name == null) {
            // Both fact and dimension without explicit names, avoid table name collision.
            name += " Details";
            // Note: key name doesn't change so they align between the fact and dimension.
        }

        HasName(name);
        HasKey().HasName(key);

        LoadClassAttributes();
    }

    /// <summary>
    /// Dimensions can reference other dimensions, so need to first load all the dimension,
    /// then in a second pass load the spokes/references between them.
    /// </summary>
    internal void LoadSpokesDeferred()
    {
        var factTable = TableEntityType.GetCustomAttribute<FactTableAttribute>();
        if(factTable == null) {
            // Only load spokes once, if this table is also a fact table, it will get the spokes instead.
            LoadSpokeBuilders();
        }
    }

    public Table Build()
    {
        var table = new Table(TableEntityType, TableName) {
            SourceProperty = Source?.PropertyInfo,
        };
        table.Columns.Add(KeyBuilder.Build());
        table.Columns.AddRange(SpokeBuilders.Values.Where(e => e.Included).Select(e => e.Build()));
        table.Columns.AddRange(AttributeBuilders.Values.Where(e => e.Included).Select(e => e.Build()));
        foreach(var row in baseData) {
            var outputDict = row.ToDictionary(e => e.Key.ColumnName, e => e.Value);
            table.Data.Add(outputDict);
        }
        return table;
    }

    public virtual DimensionTableBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public void HasData(Dictionary<ColumnBuilder, object> data)
    {
        baseData.Add(data);
    }

    public AttributeBuilder Attribute(string name)
    {
        return AttributeBuilders[name];
    }

    public ReadOnlyCollection<Dictionary<ColumnBuilder, object>> Data {
        get => new(baseData);
    }
    private readonly List<Dictionary<ColumnBuilder, object>> baseData = new();

    internal override bool HasColumnNamed(string name) => 
        KeyBuilder?.ColumnName == name || AttributeBuilders.Values.Any(e => e.ColumnName == name);

    private void LoadClassAttributes()
    {
        var attributeProperties = GetAttributeProperties();
        foreach(var attribute in attributeProperties) {
            LoadAttribute(attribute);
        }
    }

    private void LoadAttribute(PropertyInfo attribute)
    {
        var builder = new AttributeBuilder(this, TableEntityType, attribute);
        AttributeBuilders.Add(attribute.Name, builder);
    }

    private IEnumerable<PropertyInfo> GetAttributeProperties()
    {
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => AttributeBuilder.IsAttribute(e) && e != KeyBuilder.PropertyInfo);
    }

    // May be null when subclassed by DateDimensionTable
    private DimensionTableAttribute? DimensionTableAttribute { get; set; }

    private Dictionary<string, AttributeBuilder> AttributeBuilders { get; } = new();

}

public class DimensionTableBuilder<T> : DimensionTableBuilder {

    internal DimensionTableBuilder(WarehouseModelBuilder warehouseModel, Type entity) 
        : base(warehouseModel, entity)
    { 
    }

    public AttributeBuilder Attribute<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        var property = ExtractPropertyName(propertyExpression);
        return Attribute(property.Name);
    }

    public SpokeBuilder Spoke<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        var property = ExtractPropertyName(propertyExpression);
        return Spoke(property.Name);
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
