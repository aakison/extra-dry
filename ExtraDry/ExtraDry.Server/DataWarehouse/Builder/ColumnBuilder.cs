using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class ColumnBuilder {

    internal ColumnBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo)
    {
        EntityType = entityType;
        PropertyInfo = propertyInfo;
        TableBuilder = tableBuilder;
    }

    protected void SetName(string name)
    {
        ColumnName = name;
    }

    protected void SetType(ColumnType type)
    {
        if(!IsValidColumnType(type)) {
            throw new DryException("Column type is not valid.");
        }
        ColumnType = type;
    }

    internal abstract Column Build();

    protected abstract bool IsValidColumnType(ColumnType type);

    protected Type EntityType { get; set; }

    protected PropertyInfo PropertyInfo { get; set; }

    protected TableBuilder TableBuilder { get; set; }

    public string ColumnName { get; protected set; } = null!;

    public ColumnType ColumnType { get; protected set; }

}
