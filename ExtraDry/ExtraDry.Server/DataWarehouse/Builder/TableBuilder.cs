using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class TableBuilder {

    public KeyBuilder HasKey()
    {
        return KeyBuilder ?? throw new DryException("Must load a schema before accessing the Key.");
    }

    public string? TableName { get; protected set; }

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

    protected void LoadKeyColumn(PropertyInfo property)
    {
        // TODO: Check against [Key] not an integer
        if(TableEntityType == null) {
            throw new DryException("Must load a schema first");
        }
        KeyBuilder = new KeyBuilder(this, TableEntityType, property);
    }

    protected KeyBuilder? KeyBuilder { get; set; }

    protected Type? TableEntityType { get; set; }

    protected WarehouseModelBuilder? WarehouseBuilder { get; set; }

}
