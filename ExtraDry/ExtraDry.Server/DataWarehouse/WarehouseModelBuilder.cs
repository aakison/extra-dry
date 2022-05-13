using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class WarehouseModelBuilder {

    public void LoadSchema<T>() where T : DbContext
    {
        LoadSchema(typeof(T));
    }

    public void LoadSchema(Type contextType)
    {
        var entityTypes = GetEntities(contextType);

        //var assemblies = entityTypes.Select(e => e.Assembly).Distinct().ToList();

        //// Load enums, they're never dependent on anything.
        //foreach(var enumType in GetEnums(assemblies)) {
        //    var dimensionAttribute = enumType.GetCustomAttribute<DimensionTableAttribute>();
        //    if(dimensionAttribute != null) {
        //        LoadEnumDimension(enumType, dimensionAttribute);
        //    }
        //}

        //// Load dimensions, needed before facts are loaded.
        //foreach(var entity in entityTypes) {
        //    var dimensionAttribute = entity.GetCustomAttribute<DimensionTableAttribute>();
        //    if(dimensionAttribute != null) {
        //        LoadClassDimension(entity, dimensionAttribute);
        //    }
        //}

        // Finally load facts and their foreign keys to dimensions.
        foreach(var entity in entityTypes) {
            var factAttribute = entity.GetCustomAttribute<FactTableAttribute>();
            if(factAttribute != null) {
                LoadClassFact(entity);
            }
        }
    }

    private void LoadClassFact(Type entity)
    {
        Type factTableBuilderType = typeof(FactTableBuilder<>).MakeGenericType(entity);
        var factTableBuilder = Activator.CreateInstance(factTableBuilderType, true) as FactTableBuilder 
            ?? throw new DryException("Couldn't create instance of FactBuilderTable");
        factTableBuilder.LoadFactTable(this, entity);
        FactTables.Add(entity, factTableBuilder);
    }

    private static IEnumerable<Type> GetEntities(Type tableType)
    {
        var properties = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach(var property in properties) {
            var type = property.PropertyType;
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DbSet<>)) {
                var entityType = type.GetGenericArguments()[0];
                yield return entityType;
            }
        }
    }

    internal bool HasTableNamed(string name) =>
        FactTables.Values.Any(e => string.Compare(e.TableName, name, StringComparison.InvariantCultureIgnoreCase) == 0);

    public FactTableBuilder<T> FactTable<T>() where T : class
    {
        return FactTables[typeof(T)] as FactTableBuilder<T> ?? throw new DryException($"No Fact table of type {typeof(T).Name} was defined.");
    }

    internal Dictionary<Type, FactTableBuilder> FactTables { get; } = new Dictionary<Type, FactTableBuilder>();

    public WarehouseModel Build()
    {
        var model = new WarehouseModel();
        foreach(var factBuilder in FactTables.Values) {
            model.Facts.Add(factBuilder.Build());
        }
        return model;
    }

}
