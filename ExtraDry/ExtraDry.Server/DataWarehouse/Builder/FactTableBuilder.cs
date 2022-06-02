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

        LoadMeasureBuilders();
        LoadSpokeBuilders();
    }

    public Table Build()
    {
        var table = new Table(TableEntityType, TableName) {
            SourceProperty = Source?.PropertyInfo,
        };
        table.Columns.Add(KeyBuilder.Build());
        table.Columns.AddRange(SpokeBuilders.Values.Where(e => e.Included).Select(e => e.Build()));
        table.Columns.AddRange(MeasureBuilders.Values.Where(e => e.Included).Select(e => e.Build()));
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

    private void LoadMeasureBuilders()
    {
        var measureProperties = GetMeasureProperties();
        foreach(var measure in measureProperties) {
            LoadMeasure(measure);
        }
    }

    private void LoadMeasure(PropertyInfo measure)
    {
        var builder = new MeasureBuilder(this, TableEntityType, measure);
        MeasureBuilders.Add(measure.Name, builder);
    }

    private IEnumerable<PropertyInfo> GetMeasureProperties()
    {
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => MeasureBuilder.IsMeasure(e) && e != KeyBuilder.PropertyInfo);
    }

    private FactTableAttribute FactTableAttribute { get; set; }

    private Dictionary<string, MeasureBuilder> MeasureBuilders { get; } = new();

}

public class FactTableBuilder<T> : FactTableBuilder {

    internal FactTableBuilder(WarehouseModelBuilder warehouseModel, Type entity) 
        : base(warehouseModel, entity)
    { 
    }

    public MeasureBuilder Measure<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        var property = ExtractPropertyName(propertyExpression);
        return Measure(property.Name);
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
