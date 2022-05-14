using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class FactTableBuilder : TableBuilder {

    internal FactTableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity) 
        : base(warehouseBuilder, entity)
    { 
        FactTableAttribute = entity.GetCustomAttribute<FactTableAttribute>()!;

        if(FactTableAttribute.Name != null) {
            HasName(FactTableAttribute.Name);
            HasKey().HasName($"{FactTableAttribute.Name} ID");
        }

        var measureProperties = GetMeasureProperties();
        foreach(var measure in measureProperties) {
            LoadMeasure(measure);
        }
    }

    public Table Build()
    {
        var table = new Table(TableEntityType, TableName);
        table.Columns.Add(KeyBuilder.Build());
        table.Columns.AddRange(MeasureBuilders.Values.Where(e => !e.Ignore).Select(e => e.Build()));
        return table;
    }

    public FactTableBuilder HasName(string name)
    {
        SetName(name);
        return this;
    }

    public MeasureBuilder Measure(string name)
    {
        return MeasureBuilders[name];
    }

    internal override bool HasColumnNamed(string name) => 
        KeyBuilder?.ColumnName == name || MeasureBuilders.Values.Any(e => e.ColumnName == name);

    private bool IsMeasureProperty(PropertyInfo property)
    {
        var isMeasure = measureTypes.Contains(property.PropertyType);
        // Add in explicit measures.
        if(property.GetCustomAttribute<MeasureAttribute>() != null) {
            isMeasure = true;
        }
        return isMeasure;
    }

    private readonly Type[] measureTypes = new Type[] { typeof(decimal), typeof(float), typeof(int),
        typeof(double), typeof(long), typeof(short), typeof(uint), typeof(sbyte) };

    private FactTableAttribute FactTableAttribute { get; set; }

    private Dictionary<string, MeasureBuilder> MeasureBuilders { get; } = new Dictionary<string, MeasureBuilder>();

    private void LoadMeasure(PropertyInfo measure)
    {
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
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => IsMeasureProperty(e));
    }

}

public class FactTableBuilder<T> : FactTableBuilder {

    internal FactTableBuilder(WarehouseModelBuilder warehouseModel, Type entity) 
        : base(warehouseModel, entity)
    { 
    }

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
