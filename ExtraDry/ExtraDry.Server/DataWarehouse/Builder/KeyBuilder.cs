using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public class KeyBuilder : ColumnBuilder {

    internal KeyBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo) : base(tableBuilder, entityType, propertyInfo) { 
        KeyAttribute = propertyInfo.GetCustomAttribute<KeyAttribute>();
        ColumnName =  $"{tableBuilder.TableName} ID";
        ColumnType = ColumnType.Key;
    }

    public KeyBuilder HasName(string name)
    {
        ColumnName = name;
        // TODO: validate
        return this;
    }

    private KeyAttribute? KeyAttribute { get; set; }

    protected override bool IsValidColumnType(ColumnType type)
    {
        return type == ColumnType.Key;
    }

    internal override Column Build()
    {
        return new Column(ColumnType.Key, ColumnName) {
            Nullable = false,
            PropertyInfo = PropertyInfo,
        };
    }

}
