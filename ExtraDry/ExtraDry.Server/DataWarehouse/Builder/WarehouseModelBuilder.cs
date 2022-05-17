using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        // Load dimensions, needed before facts are loaded.
        foreach(var entity in entityTypes) {
            var dimensionAttribute = entity.GetCustomAttribute<DimensionTableAttribute>();
            if(dimensionAttribute != null) {
                LoadClassDimension(entity);
            }
        }

        // Finally load facts and their foreign keys to dimensions.
        foreach(var entity in entityTypes) {
            var factAttribute = entity.GetCustomAttribute<FactTableAttribute>();
            if(factAttribute != null) {
                LoadClassFact(entity);
            }
        }
    }

    private void LoadClassDimension(Type entity)
    {
        // To activate, need type, args and indicator that the constructor is not public, tricky to get right overload...
        var dimensionTableBuilderType = typeof(DimensionTableBuilder<>).MakeGenericType(entity);
        var args = new object[] { this, entity };
        var binding = BindingFlags.NonPublic | BindingFlags.Instance;
        var binder = (Binder?)null;
        var culture = CultureInfo.InvariantCulture;
        try {
            var dimensionTableBuilder = 
                Activator.CreateInstance(dimensionTableBuilderType, binding, binder, args, culture) as DimensionTableBuilder
                ?? throw new DryException("Couldn't create instance of DimensionBuilderTable");
            DimensionTables.Add(entity, dimensionTableBuilder);
        }
        catch(TargetInvocationException ex) {
            // Unwrap invocation and throw as if reflection wasn't being used.
            if(ex.InnerException is DryException dryEx) {
                throw dryEx;
            }
            else {
                throw;
            }
        }
    }

    private void LoadClassFact(Type entity)
    {
        // To activate, need type, args and indicator that the constructor is not public, tricky to get right overload...
        var factTableBuilderType = typeof(FactTableBuilder<>).MakeGenericType(entity);
        var args = new object[] { this, entity };
        var binding = BindingFlags.NonPublic | BindingFlags.Instance;
        var binder = (Binder?)null;
        var culture = CultureInfo.InvariantCulture;
        try {
            var factTableBuilder = 
                Activator.CreateInstance(factTableBuilderType, binding, binder, args, culture) as FactTableBuilder
                ?? throw new DryException("Couldn't create instance of FactBuilderTable");
            FactTables.Add(entity, factTableBuilder);
        }
        catch(TargetInvocationException ex) {
            // Unwrap invocation and throw as if reflection wasn't being used.
            if(ex.InnerException is DryException dryEx) {
                throw dryEx;
            }
            else {
                throw;
            }
        }
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
        FactTables.Values.Any(e => string.Compare(e.TableName, name, StringComparison.InvariantCultureIgnoreCase) == 0) ||
        DimensionTables.Values.Any(e => string.Compare(e.TableName, name, StringComparison.InvariantCultureIgnoreCase) == 0);

    public FactTableBuilder<T> Fact<T>() where T : class
    {
        return FactTables[typeof(T)] as FactTableBuilder<T> ?? throw new DryException($"No Fact table of type {typeof(T).Name} was defined.");
    }

    public DimensionTableBuilder<T> Dimension<T>() where T : class
    {
        return DimensionTables[typeof(T)] as DimensionTableBuilder<T> ?? throw new DryException($"No Dimension table of type {typeof(T).Name} was defined.");
    }

    private Dictionary<Type, FactTableBuilder> FactTables { get; } = new();

    private Dictionary<Type, DimensionTableBuilder> DimensionTables { get; } = new();

    public WarehouseModel Build()
    {
        var model = new WarehouseModel();
        foreach(var dimensionBuilder in DimensionTables.Values) {
            model.Dimensions.Add(dimensionBuilder.Build());
        }
        foreach(var factBuilder in FactTables.Values) {
            model.Facts.Add(factBuilder.Build());
        }
        return model;
    }

}
