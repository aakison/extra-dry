﻿using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class TableBuilder
{
    protected TableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity, string? name = null)
    {
        WarehouseBuilder = warehouseBuilder;
        TableEntityType = entity;
        SetName(DataConverter.CamelCaseToTitleCase(name ?? entity.Name));
        var keyProperty = GetKeyProperty();
        KeyBuilder = new KeyBuilder(this, TableEntityType, keyProperty);
    }

    public KeyBuilder HasKey()
    {
        return KeyBuilder;
    }

    public SpokeBuilder Spoke(string name)
    {
        return SpokeBuilders[name];
    }

    public string TableName { get; private set; } = null!; // Constructor sets via method, analyzer misses it...

    public EntitySource? Source { get; internal set; }

    protected PropertyInfo GetKeyProperty()
    {
        var properties = TableEntityType!.GetProperties();
        var keyProperty = properties.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null) ??
            properties.FirstOrDefault(e => string.Equals(e.Name, "Id", StringComparison.OrdinalIgnoreCase)) ??
            properties.FirstOrDefault(e => string.Equals(e.Name, $"{TableEntityType.Name}Id", StringComparison.OrdinalIgnoreCase));
        return keyProperty ?? throw new DryException("Missing primary key", $"On {TableName}, Fact and Dimension tables must have primary keys.");
    }

    internal abstract bool HasColumnNamed(string name);

    protected void SetName(string name)
    {
        if(string.IsNullOrWhiteSpace(name)) {
            throw new DryException("Name must not be empty.");
        }
        if(name.Length > 50) {
            // Not a SQL limit, but a UX limit!
            throw new DryException("Name limited to 50 characters.");
        }
        if(name != TableName && WarehouseBuilder.HasTableNamed(name)) {
            throw new DryException($"Names for tables must be unique, '{name}' is duplicated.");
        }
        TableName = name;
    }

    protected void LoadSpokeBuilders()
    {
        var spokeProperties = GetSpokeProperties();
        foreach(var spoke in spokeProperties) {
            LoadSpoke(spoke);
        }
    }

    protected KeyBuilder KeyBuilder { get; }

    protected Dictionary<string, SpokeBuilder> SpokeBuilders { get; } = [];

    protected Type TableEntityType { get; }

    internal WarehouseModelBuilder WarehouseBuilder { get; }

    private IEnumerable<PropertyInfo> GetSpokeProperties()
    {
        return TableEntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(e => WarehouseBuilder.HasDimension(e.PropertyType))
            .Where(e => e.GetCustomAttribute<SpokeIgnoreAttribute>() == null);
    }

    private void LoadSpoke(PropertyInfo spoke)
    {
        var builder = new SpokeBuilder(this, TableEntityType, spoke);
        SpokeBuilders.Add(spoke.Name, builder);
    }
}
