using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class FactTableBuilder : TableBuilder {

    internal FactTableBuilder() { }

    public void LoadFactTable(WarehouseModelBuilder warehouseBuilder, Type entity)
    {
        WarehouseBuilder = warehouseBuilder;
        TableEntityType = entity;

        FactTableAttribute = entity.GetCustomAttribute<FactTableAttribute>() ??
            throw new DryException($"Entity {entity.Name} must have a [FactTable] attribute.");

        HasName(FactTableAttribute.Name ?? DataConverter.CamelCaseToTitleCase(entity.Name));

        var keyProperty = GetKeyProperty();
        LoadKeyColumn(keyProperty);

        var measureProperties = GetMeasureProperties();
        foreach(var measure in measureProperties) {
            if(measure.PropertyType != keyProperty.PropertyType) {
                LoadMeasure(measure);
            }
        }
    }

    private FactTableAttribute? FactTableAttribute { get; set; }

    private Dictionary<string, MeasureBuilder> MeasureBuilders { get; } = new Dictionary<string, MeasureBuilder>();

    private void LoadMeasure(PropertyInfo measure)
    {
        if(TableEntityType == null) {
            throw new DryException("Must load schema first.");
        }
        var builder = new MeasureBuilder(this, TableEntityType, measure);
        MeasureBuilders.Add(measure.Name, builder);
        //    Measure(measure.Name);
        //var name = measure.Value.Name
        //    ?? measure.Key.PropertyType.GetCustomAttribute<DimensionTableAttribute>()?.Name
        //    ?? measure.Key.Name;
        //var column = EntityPropertyToColumn(entity, name, measure.Key, false);
        //table.Columns.Add(column);
    }

    private IEnumerable<PropertyInfo> GetMeasureProperties()
    {
        if(TableEntityType == null) {
            throw new DryException("Must load a schema first");
        }
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => IsMeasureType(e.PropertyType));
    }

    private bool IsMeasureType(Type type) => measureTypes.Contains(type);

    private readonly Type[] measureTypes = new Type[] { typeof(decimal), typeof(float), typeof(int), 
        typeof(double), typeof(long), typeof(short), typeof(uint), typeof(sbyte) };

    public Table Build()
    {
        if(TableEntityType == null || TableName == null || KeyBuilder == null) {
            throw new DryException("Must load a schema first");
        }
        var table = new Table(TableEntityType, TableName);
        table.Columns.Add(KeyBuilder.Build());
        table.Columns.AddRange(MeasureBuilders.Values.Select(e => e.Build()));
        return table;
    }

    public FactTableBuilder HasName(string name)
    {
        if(WarehouseBuilder == null) {
            throw new DryException("Must load a schema first");
        }
        if(string.IsNullOrWhiteSpace(name)) {
            throw new DryException("Name must not be empty.");
        }
        if(WarehouseBuilder.HasTableNamed(name)) {
            throw new DryException("Names for tables must be unique, {name} is duplicated.");
        }
        TableName = name;
        return this;
    }

    public MeasureBuilder Measure(string name)
    {
        return MeasureBuilders[name];
    }
}

public class FactTableBuilder<T> : FactTableBuilder {

    internal FactTableBuilder() { }

    public MeasureBuilder Measure<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        if(propertyExpression.Body is not MemberExpression member) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        if(member.Member is not PropertyInfo property) {
            throw new DryException("Expression should be a property expression such as `e => e.Property`.");
        }
        return Measure(property.Name);
    }

}
