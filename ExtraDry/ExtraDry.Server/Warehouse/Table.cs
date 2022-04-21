using ExtraDry.Core.Warehouse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ExtraDry.Server.Warehouse;

public class Warehouse {

    public void CreateSchema(DbContext context)
    {
        var entityTypes = GetEntities(context);
        var types = new List<Type>();
        foreach(var entity in entityTypes) {
            var factAttribute = entity.GetCustomAttribute<FactAttribute>();
            if(factAttribute != null) {
                var factTable = new Table(factAttribute.Name ?? entity.Name);
                Facts.Add(factTable);
                types.Add(entity);
            }
            var dimensionAttribute = entity.GetCustomAttribute<DimensionAttribute>();
            if(dimensionAttribute != null) {
                var dimensionTable = new Table(dimensionAttribute.Name ?? entity.Name);
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

    private void LoadEnumDimension(Type enumType, DimensionAttribute dimensionAttribute)
    {
        var table = new Table(dimensionAttribute.Name ?? enumType.Name);
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

public class Table {

    public Table(string title)
    {
        Title = title;
    }

    public string Title { get; set; }

    public List<Column> Columns { get; } = new List<Column>();

    public List<Dictionary<string, object>> Data { get; } = new List<Dictionary<string, object>>();

}

public class Column {

    public Column(ColumnType type, string title) {
        ColumnType = type;
        Title = title; 
    }

    public string Title { get; set; }

    public ColumnType ColumnType { get; set; }
    
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColumnType {
    Integer,
    Float,
    Text,
}

