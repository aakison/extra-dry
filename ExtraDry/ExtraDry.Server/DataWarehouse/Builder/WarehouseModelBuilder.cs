using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class WarehouseModelBuilder {

    public void LoadSchema<T>(string? group = null) where T : DbContext
    {
        LoadSchema(typeof(T), group);
    }

    public void LoadSchema(Type contextType, string? group = null)
    {
        var entitySources = GetEntitySources(contextType);
        EntityContextType = contextType;

        var factsAndDimensions = GetWarehouseTables(entitySources.Select(e => e.EntityType));

        var dimensions = factsAndDimensions.Where(e => e.GetCustomAttribute<DimensionTableAttribute>() != null);
        var nonEntityDimensions = dimensions.Except(entitySources.Select(e => e.EntityType));

        // Load dimensions that have static or generated content, not sourced from a EF entity.
        foreach(var dimension in nonEntityDimensions) {
            if(dimension.IsEnum) {
                LoadEnumDimension(dimension, dimension.GetCustomAttribute<DimensionTableAttribute>()!);
            }
            else {
                LoadClassDimension(dimension, null);
            }
        }

        // Load dimensions, needed before facts are loaded.
        foreach(var source in entitySources) {
            var dimensionAttribute = source.EntityType.GetCustomAttribute<DimensionTableAttribute>();
            if(dimensionAttribute != null) {
                LoadClassDimension(source.EntityType, source);
            }
        }

        // After dimensions are loaded, load spokes between them
        foreach(var dimension in DimensionTables.Values) {
            dimension.LoadSpokesDeferred();
        }

        // Finally load facts and their foreign keys to dimensions.
        foreach(var source in entitySources) {
            var factAttribute = source.EntityType.GetCustomAttribute<FactTableAttribute>();
            if(factAttribute?.MatchesGroup(group) ?? false) {
                LoadClassFact(source);
            }
        }
    }

    private static List<Type> GetWarehouseTables(IEnumerable<Type> enumerable)
    {
        var tableClasses = new List<Type>() { typeof(Date), typeof(Time) };
        var rejectedClasses = new List<Type>();
        foreach(var candidate in enumerable) {
            ExpandTables(candidate);
        }
        return tableClasses;

        void ExpandTables(Type candidate)
        {
            if(rejectedClasses.Contains(candidate) || tableClasses.Contains(candidate)) {
                return;
            }
            if(candidate.GetCustomAttribute<FactTableAttribute>() != null || candidate.GetCustomAttribute<DimensionTableAttribute>() != null) {
                tableClasses.Add(candidate);
            }
            else {
                rejectedClasses.Add(candidate);
                return;
            }
            var properties = candidate.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var property in properties) {
                ExpandTables(property.PropertyType);
            }
        }

    }

    public FactTableBuilder<T> Fact<T>() where T : class
    {
        try {
            return FactTables[typeof(T)] as FactTableBuilder<T> ?? throw new KeyNotFoundException();
        }
        catch(KeyNotFoundException) {
            throw new DryException($"No Fact table of type '{typeof(T).Name}' was defined.");
        }
    }

    public DimensionTableBuilder Dimension(Type type)
    {
        try {
            return DimensionTables[type] ?? throw new KeyNotFoundException();
        }
        catch(KeyNotFoundException) {
            throw new DryException($"No Dimension table of type '{type.Name}' was defined.");
        }
    }

    public DimensionTableBuilder<T> Dimension<T>() where T : class
    {
        try {
            return DimensionTables[typeof(T)] as DimensionTableBuilder<T> ?? throw new KeyNotFoundException();
        }
        catch(KeyNotFoundException) {
            throw new DryException($"No Dimension table of type '{typeof(T).Name}' was defined.");
        }
    }

    public DimensionTableBuilder<EnumDimension> EnumDimension<T>() where T : Enum
    {
        return EnumDimension(typeof(T));
    }

    public bool HasDimension(Type type) => DimensionTables.ContainsKey(type);

    public DimensionTableBuilder<EnumDimension> EnumDimension(Type type) 
    {
        try {
            return DimensionTables[type] as DimensionTableBuilder<EnumDimension> ?? throw new KeyNotFoundException();
        }
        catch(KeyNotFoundException) {
            throw new DryException($"No Dimension table of type '{type.Name}' was defined.");
        }
    }

    internal WarehouseModel Build()
    {
        var type = EntityContextType ?? throw new DryException("Can't generate warehouse model without first loading schema.");
        return new WarehouseModel(this, type, Group);
    }

    internal IEnumerable<Table> BuildDimensions()
    {
        foreach(var dimensionBuilder in DimensionTables.Values) {
            yield return dimensionBuilder.Build();
        }
    }

    internal IEnumerable<Table> BuildFacts()
    {
        foreach(var factBuilder in FactTables.Values) {
            yield return factBuilder.Build();
        }
    }

    internal bool HasTableNamed(string name) =>
        FactTables.Values.Any(e => string.Compare(e.TableName, name, StringComparison.InvariantCultureIgnoreCase) == 0) ||
        DimensionTables.Values.Any(e => string.Compare(e.TableName, name, StringComparison.InvariantCultureIgnoreCase) == 0);

    private void LoadEnumDimension(Type enumType, DimensionTableAttribute dimension)
    {
        if(LoadViaConstructor(typeof(DimensionTableBuilder<>), typeof(Builder.EnumDimension)) is DimensionTableBuilder builder) {
            ConfigureEnumDimension(enumType, dimension, builder);
            DimensionTables.Add(enumType, builder);
        }
    }

    private static void ConfigureEnumDimension(Type enumType, DimensionTableAttribute dimension, DimensionTableBuilder builder)
    {
        var name = dimension.Name ?? DataConverter.CamelCaseToTitleCase(enumType.Name);
        builder.HasName(name);
        builder.HasKey().HasName($"{name} ID");

        var stats = new EnumStats(enumType);

        builder.Attribute(nameof(Builder.EnumDimension.Name)).HasLength(stats.DisplayNameMaxLength());

        if(stats.HasShortName()) {
            builder.Attribute(nameof(Builder.EnumDimension.ShortName)).HasLength(stats.ShortNameMaxLength());
        }
        else {
            builder.Attribute(nameof(Builder.EnumDimension.ShortName)).IsIncluded(false);
        }

        if(stats.HasGroupName()) {
            builder.Attribute(nameof(Builder.EnumDimension.GroupName)).HasLength(stats.GroupNameMaxLength());
        }
        else {
            builder.Attribute(nameof(Builder.EnumDimension.GroupName)).IsIncluded(false);
        }

        if(stats.HasDescription()) {
            builder.Attribute(nameof(Builder.EnumDimension.Description)).HasLength(stats.DescriptionMaxLength());
        }
        else {
            builder.Attribute(nameof(Builder.EnumDimension.Description)).IsIncluded(false);
        }

        if(!stats.HasOrder()) {
            builder.Attribute(nameof(Builder.EnumDimension.Order)).IsIncluded(false);
        }

        LoadEnumBaseData(builder, stats);
    }

    private static void LoadEnumBaseData(DimensionTableBuilder builder, EnumStats enumStats)
    {
        var keyBuilder = builder.HasKey();
        var nameBuilder = builder.Attribute(nameof(Builder.EnumDimension.Name));
        var shortNameBuilder = builder.Attribute(nameof(Builder.EnumDimension.ShortName));
        var descriptionBuilder = builder.Attribute(nameof(Builder.EnumDimension.Description));
        var groupNameBuilder = builder.Attribute(nameof(Builder.EnumDimension.GroupName));
        var orderBuilder = builder.Attribute(nameof(Builder.EnumDimension.Order));
        foreach(var enumField in enumStats.Fields) {
            var data = new Dictionary<ColumnBuilder, object> {
                { keyBuilder, enumField.Value },
                { nameBuilder, enumField.DisplayName }
            };
            if(enumStats.HasShortName()) {
                data.Add(shortNameBuilder, enumField.ShortName ?? string.Empty);
            }
            if(enumStats.HasDescription()) {
                data.Add(descriptionBuilder, enumField.Description ?? string.Empty);
            }
            if(enumStats.HasGroupName()) {
                data.Add(groupNameBuilder, enumField.GroupName ?? string.Empty);
            }
            if(orderBuilder.Included) {
                // Default per https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.order?view=net-6.0
                data.Add(orderBuilder, enumField.Order ?? 10000);
            }
            builder.HasData(data);
        }
    }

    private void LoadClassDimension(Type type, EntitySource? entitySource)
    {
        if(LoadViaConstructor(typeof(DimensionTableBuilder<>), type) is DimensionTableBuilder builder) {
            builder.Source = entitySource;
            DimensionTables.Add(type, builder);
        }
    }

    private void LoadClassFact(EntitySource entitySource)
    {
        if(LoadViaConstructor(typeof(FactTableBuilder<>), entitySource.EntityType) is FactTableBuilder builder) {
            builder.Source = entitySource;
            FactTables.Add(entitySource.EntityType, builder);
        }
    }

    private object LoadViaConstructor(Type generic, Type entity) 
    {
        // To activate, need type, args and indicator that the constructor is not public, tricky to get right overload...
        var factTableBuilderType = generic.MakeGenericType(entity);
        var args = new object[] { this, entity };
        var binding = BindingFlags.NonPublic | BindingFlags.Instance;
        var binder = (Binder?)null;
        var culture = CultureInfo.InvariantCulture;
        try {
            return Activator.CreateInstance(factTableBuilderType, binding, binder, args, culture) 
                ?? throw new DryException("Couldn't create instance of FactBuilderTable");
        }
        catch(TargetInvocationException ex) when (ex.InnerException is DryException dryEx) {
            throw dryEx;
        }
    }

    private static IEnumerable<EntitySource> GetEntitySources(Type dbContextType)
    {
        var properties = dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach(var property in properties) {
            var type = property.PropertyType;
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DbSet<>)) {
                var entityType = type.GetGenericArguments()[0];
                yield return new EntitySource(entityType) { ContextType = dbContextType, PropertyInfo = property };
            }
        }
    }

    private Type? EntityContextType { get; set;  }

    private string? Group { get; set; }

    private Dictionary<Type, FactTableBuilder> FactTables { get; } = new();

    private Dictionary<Type, DimensionTableBuilder> DimensionTables { get; } = new();

}
