using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class TableBuilder {

    protected TableBuilder(WarehouseModelBuilder warehouseBuilder, Type entity)
    {
        WarehouseBuilder = warehouseBuilder;
        TableEntityType = entity;
        TableName = DataConverter.CamelCaseToTitleCase(entity.Name);
        var keyProperty = GetKeyProperty();
        KeyBuilder = new KeyBuilder(this, TableEntityType, keyProperty);
    }

    public KeyBuilder HasKey()
    {
        return KeyBuilder;
    }

    public string TableName { get; private set; }

    protected PropertyInfo GetKeyProperty()
    {
        var properties = TableEntityType!.GetProperties();
        var keyProperty = properties.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null) ??
            properties.FirstOrDefault(e => string.Compare(e.Name, "Id", StringComparison.OrdinalIgnoreCase) == 0) ??
            properties.FirstOrDefault(e => string.Compare(e.Name, $"{TableEntityType.Name}Id", StringComparison.OrdinalIgnoreCase) == 0);
        if(keyProperty == null) {
            throw new DryException("Fact and Dimension tables must have primary keys.");
        }
        return keyProperty;
    }

    internal abstract bool HasColumnNamed(string name);

    protected void SetName(string name)
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
    }

    protected KeyBuilder KeyBuilder { get; }

    protected Type TableEntityType { get; }

    protected WarehouseModelBuilder WarehouseBuilder { get; }

}
