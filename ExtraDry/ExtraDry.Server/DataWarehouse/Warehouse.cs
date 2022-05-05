using ExtraDry.Core;
using ExtraDry.Core.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class Warehouse {

    public void CreateSchema(DbContext context)
    {
        var entityTypes = GetEntities(context);
        var types = new List<Type>();
        foreach(var entity in entityTypes) {
            var factAttribute = entity.GetCustomAttribute<FactAttribute>();
            if(factAttribute != null) {
                LoadClassFact(entity, factAttribute);
                types.Add(entity);
            }
            var dimensionAttribute = entity.GetCustomAttribute<DimensionAttribute>();
            if(dimensionAttribute != null) {
                var dimensionTable = new Table(entity, dimensionAttribute.Name ?? entity.Name);
                Dimensions.Add(dimensionTable);
                types.Add(entity);
            }
        }

        var assemblies = types.Select(e => e.Assembly).Distinct().ToList();
        foreach(var enumType in GetEnums(assemblies)) {
            var dimensionAttribute = enumType.GetCustomAttribute<DimensionAttribute>();
            if(dimensionAttribute != null) {
                LoadEnumDimension(enumType, dimensionAttribute);
            }
        }

    }

    private void LoadClassFact(Type entity, FactAttribute factAttribute)
    {
        var table = new Table(entity, factAttribute.Name ?? entity.Name);
        Facts.Add(table);

        var keyColumn = $"{table.Title} ID";
        table.Columns.Add(new Column(ColumnType.Integer, keyColumn));

        // TODO: Check against [Key] not an integer?

        var measures = GetMeasures(entity);
        foreach(var measure in measures) {
            var name = measure.Value.Name ?? measure.Key.Name;
            table.Columns.Add(new Column(TypeToColumnType(measure.Key.PropertyType), name));
        }
    }

    private static Dictionary<PropertyInfo, MeasureAttribute> GetMeasures(Type entity)
    {
        var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var measures = properties.Where(e => e.GetCustomAttribute<MeasureAttribute>() != null);
        return measures.ToDictionary(e => e, e => e.GetCustomAttribute<MeasureAttribute>()!);
    }

    private static ColumnType TypeToColumnType(Type entity)
    {
        if(entity.IsAssignableTo(typeof(long))) {
            return ColumnType.Integer;
        }
        else if (entity.IsAssignableTo(typeof(double))) {
            return ColumnType.Float;
        }
        else if (entity.IsAssignableTo(typeof(string))) {
            return ColumnType.Text;
        }
        else if(entity.IsAssignableTo(typeof(Enum))) {
            return ColumnType.Integer; // Foreign Key
        }
        else {
            throw new DryException("Data type is not supported for a Measure", "Internal error mapping to data warehouse 0x0F68CC98");
        }
    }
        
    private void LoadEnumDimension(Type enumType, DimensionAttribute dimensionAttribute)
    {
        var table = new Table(enumType, dimensionAttribute.Name ?? enumType.Name);
        var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

        var keyColumn = $"{table.Title} ID";
        var elements = fields.ToDictionary(k => k, e => e.GetCustomAttribute<DisplayAttribute>());

        table.Columns.Add(new Column(ColumnType.Integer, keyColumn));
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

    public IList<Table> Facts { get; set;} = new List<Table>();

    public IList<Table> Dimensions { get; set; } = new List<Table>();

}

