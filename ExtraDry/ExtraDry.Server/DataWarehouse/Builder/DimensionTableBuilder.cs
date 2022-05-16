using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class DimensionTableBuilder : TableBuilder {

    internal DimensionTableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity) 
        : base(warehouseBuilder, entity)
    { 
        DimensionTableAttribute = entity.GetCustomAttribute<DimensionTableAttribute>()!;

        if(DimensionTableAttribute.Name != null) {
            HasName(DimensionTableAttribute.Name);
            HasKey().HasName($"{DimensionTableAttribute.Name} ID");
        }

        var attributeProperties = GetAttributeProperties();
        foreach(var attribute in attributeProperties) {
            LoadAttribute(attribute);
        }
    }

    public Table Build()
    {
        var table = new Table(TableEntityType, TableName);
        table.Columns.Add(KeyBuilder.Build());
        table.Columns.AddRange(AttributeBuilders.Values.Where(e => !e.Ignore).Select(e => e.Build()));
        return table;
    }

    public DimensionTableBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public AttributeBuilder Attribute(string name)
    {
        return AttributeBuilders[name];
    }

    internal override bool HasColumnNamed(string name) => 
        KeyBuilder?.ColumnName == name || AttributeBuilders.Values.Any(e => e.ColumnName == name);

    private DimensionTableAttribute DimensionTableAttribute { get; set; }

    private Dictionary<string, AttributeBuilder> AttributeBuilders { get; } = new();

    private void LoadAttribute(PropertyInfo attribute)
    {
        var builder = new AttributeBuilder(this, TableEntityType, attribute);
        AttributeBuilders.Add(attribute.Name, builder);
    }

    private IEnumerable<PropertyInfo> GetAttributeProperties()
    {
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => AttributeBuilder.IsAttribute(e));
    }

}

public class DimensionTableBuilder<T> : DimensionTableBuilder {

    internal DimensionTableBuilder(WarehouseModelBuilder warehouseModel, Type entity) 
        : base(warehouseModel, entity)
    { 
    }

    public AttributeBuilder Attribute<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        if(propertyExpression.Body is not MemberExpression member) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        if(member.Member is not PropertyInfo property) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        return Attribute(property.Name);
    }

}
