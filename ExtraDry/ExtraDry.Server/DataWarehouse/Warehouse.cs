using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class Warehouse {

    public void CreateSchema(DbContext context)
    {
        var entityTypes = GetEntities(context);

        var assemblies = entityTypes.Select(e => e.Assembly).Distinct().ToList();

        // Load enums, they're never dependent on anything.
        foreach(var enumType in GetEnums(assemblies)) {
            var dimensionAttribute = enumType.GetCustomAttribute<DimensionTableAttribute>();
            if(dimensionAttribute != null) {
                LoadEnumDimension(enumType, dimensionAttribute);
            }
        }

        // Load dimensions, needed before facts are loaded.
        foreach(var entity in entityTypes) {
            var dimensionAttribute = entity.GetCustomAttribute<DimensionTableAttribute>();
            if(dimensionAttribute != null) {
                LoadClassDimension(entity, dimensionAttribute);
            }
        }

        // Finally load facts and their foreign keys to dimensions.
        foreach(var entity in entityTypes) {
            var factAttribute = entity.GetCustomAttribute<FactTableAttribute>();
            if(factAttribute != null) {
                LoadClassFact(entity, factAttribute);
            }
        }

    }

    private void LoadClassDimension(Type entity, DimensionTableAttribute dimensionAttribute)
    {
        var table = new Table(entity, dimensionAttribute.Name ?? entity.Name);
        Dimensions.Add(table);
        var factAttribute = entity.GetCustomAttribute<FactTableAttribute>();
        if(factAttribute != null) {
            // Some dimensions are also facts, class gets split into two, use fact naming scheme for ID
            var factName = factAttribute?.Name ?? entity.Name;
            AddKeyColumn(entity, table, $"{factName} ID");
            // Also, ensure the dimension table has a unique name
            if(factName == table.Name) {
                table.Name += " Details";
            }
        }
        else {
            AddKeyColumn(entity, table, $"{table.Name} ID");
        }

        var attributes = GetAttributedProperties<AttributeAttribute>(entity);
        foreach(var attribute in attributes) {
            var name = attribute.Value.Name
                ?? attribute.Key.PropertyType.GetCustomAttribute<DimensionTableAttribute>()?.Name
                ?? attribute.Key.Name;
            var column = EntityPropertyToColumn(entity, name, attribute.Key, true);
            table.Columns.Add(column);
        }
    }

    private void LoadClassFact(Type entity, FactTableAttribute factAttribute)
    {
        var table = new Table(entity, factAttribute.Name ?? entity.Name);
        Facts.Add(table);

        AddKeyColumn(entity, table, $"{table.Name} ID");

        // TODO: Check against [Key] not an integer

        var measures = GetAttributedProperties<MeasureAttribute>(entity);
        foreach(var measure in measures) {
            var name = measure.Value.Name
                ?? measure.Key.PropertyType.GetCustomAttribute<DimensionTableAttribute>()?.Name
                ?? measure.Key.Name;
            var column = EntityPropertyToColumn(entity, name, measure.Key, false);
            table.Columns.Add(column);
        }
    }

    private Column EntityPropertyToColumn(Type entity, string name, PropertyInfo property, bool allowString)
    {
        var column = new Column(TypeToColumnType(property, allowString), name) {
            PropertyInfo = property,
        };
        if(property.PropertyType.IsEnum) {
            var dimension = Dimensions.FirstOrDefault(e => e.EntityType == property.PropertyType);
            if(dimension == null) {
                throw new DryException($"Can't create a foreign key reference {entity.Name}/{property.Name} as target is not configured as a Dimension");
            }
            var dimensionKey = dimension?.Columns.FirstOrDefault(e => e.ColumnType == ColumnType.Key);
            if(dimensionKey == null) {
                throw new DryException($"Can't create a foreign key reference {entity.Name}/{property.Name} as target dimension does not have a Key attribute");
            }
            column.Name += " ID";
            column.Reference = new Reference(dimension!, dimensionKey);
        }
        if(property.PropertyType == typeof(string)) {
            column.Length = property.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength
                ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                ?? 0;
        }
        return column;
    }

    private static void AddKeyColumn(Type entity, Table table, string columnName)
    {
        var properties = entity.GetProperties();
        var keyProperty = properties.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null) ??
            properties.FirstOrDefault(e => string.Compare(e.Name, "Id", StringComparison.OrdinalIgnoreCase) == 0) ??
            properties.FirstOrDefault(e => string.Compare(e.Name, $"{entity.Name}Id", StringComparison.OrdinalIgnoreCase) == 0);
        if(keyProperty == null) {
            throw new DryException("Fact and Dimension tables must have primary keys.");
        }
        table.Columns.Add(new Column(ColumnType.Key, columnName) {
            PropertyInfo = keyProperty
        });
    }

    private static Dictionary<PropertyInfo, T> GetAttributedProperties<T>(Type entity) where T : Attribute
    {
        var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var attributes = properties.Where(e => e.GetCustomAttribute<T>() != null);
        return attributes.ToDictionary(e => e, e => e.GetCustomAttribute<T>()!);
    }

    private static ColumnType TypeToColumnType(PropertyInfo propertyInfo, bool supportString)
    {
        var type = propertyInfo.PropertyType;
        if(type.IsAssignableTo(typeof(long))) {
            return ColumnType.Integer;
        }
        else if(type.IsAssignableTo(typeof(double))) {
            return ColumnType.Double;
        }
        else if(supportString && type.IsAssignableTo(typeof(string))) {
            return ColumnType.Text;
        }
        else if(type.IsAssignableTo(typeof(Enum))) {
            return ColumnType.Integer; // Foreign Key
        }
        else {
            throw new DryException($"Data type '{type.Name}' is not supported for a Measure/Attribute {propertyInfo.DeclaringType?.Name}/{propertyInfo.Name}", "Internal error mapping to data warehouse 0x0F68CC98");
        }
    }

    private void LoadEnumDimension(Type enumType, DimensionTableAttribute dimensionAttribute)
    {
        var table = new Table(enumType, dimensionAttribute.Name ?? enumType.Name);
        var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

        var keyColumn = $"{table.Name} ID";
        var elements = fields.ToDictionary(k => k, e => e.GetCustomAttribute<DisplayAttribute>());

        table.Columns.Add(new Column(ColumnType.Key, keyColumn));
        table.Columns.Add(new Column(ColumnType.Text, "Title"));

        if(elements.Values.Any(e => e?.ShortName != null)) {
            table.Columns.Add(new Column(ColumnType.Text, "Short Name"));
        }

        if(elements.Values.Any(e => e?.Description != null)) {
            table.Columns.Add(new Column(ColumnType.Text, "Description"));
        }

        if(elements.Values.Any(e => e?.GroupName != null)) {
            table.Columns.Add(new Column(ColumnType.Text, "Group"));
        }

        var hasOrder = false;
        if(elements.Values.Any(e => e?.Order != null)) {
            table.Columns.Add(new Column(ColumnType.Integer, "Order"));
            hasOrder = true;
        }

        foreach(var element in elements) {
            var data = new Dictionary<string, object> {
                { keyColumn, (int)(element.Key.GetValue(null) ?? 0) },
                { "Title", element.Value?.Name ?? element.Key.Name }
            };
            if(element.Value?.ShortName != null) {
                data.Add("Short Name", element.Value.ShortName);
            }
            if(element.Value?.Description != null) {
                data.Add("Description", element.Value.Description);
            }
            if(element.Value?.GroupName != null) {
                data.Add("Group", element.Value.GroupName);
            }
            if(hasOrder) {
                // Default per https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.order?view=net-6.0
                data.Add("Order", element.Value?.GetOrder() ?? 10000);
            }
            table.Data.Add(data);
        }

        Dimensions.Add(table);
    }

    private static IEnumerable<Type> GetEnums(List<Assembly> assemblies)
    {
        foreach(var assembly in assemblies) {
            foreach(var type in assembly.GetTypes()) {
                if(type.IsEnum) {
                    yield return type;
                }
            }
        }
    }

    private static IEnumerable<Type> GetEntities(DbContext context)
    {
        var properties = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach(var property in properties) {
            var type = property.PropertyType;
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DbSet<>)) {
                var entityType = type.GetGenericArguments()[0];
                yield return entityType;
            }
        }
    }

    public IList<Table> Facts { get; set; } = new List<Table>();

    public IList<Table> Dimensions { get; set; } = new List<Table>();

    public string GenerateSql() => new SqlGenerator().Generate(this);

}

