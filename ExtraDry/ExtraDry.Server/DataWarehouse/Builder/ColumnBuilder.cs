using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

public abstract class ColumnBuilder {

    internal ColumnBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo)
    {
        EntityType = entityType;
        PropertyInfo = propertyInfo;
        TableBuilder = tableBuilder;
        ColumnName = DataConverter.CamelCaseToTitleCase(propertyInfo.Name);
    }

    public string ColumnName { get; private set; }

    public ColumnType ColumnType { get; private set; }

    public bool Ignore { get; private set; }

    protected void SetName(string name)
    {
        if(string.IsNullOrWhiteSpace(name)) {
            throw new DryException("Name must not be empty.");
        }
        if(name.Length > 50) {
            // Not a SQL limit, but a UX limit!
            throw new DryException("Name limited to 50 characters.");
        }
        if(name != ColumnName && TableBuilder.HasColumnNamed(name)) {
            throw new DryException($"Names for tables must be unique, {name} is duplicated.");
        }
        ColumnName = name;
    }

    protected void SetType(ColumnType type)
    {
        if(!IsValidColumnType(type)) {
            throw new DryException("Column type is not valid.");
        }
        ColumnType = type;
    }

    protected void SetIgnore(bool ignore)
    {
        Ignore = ignore;
    }

    internal abstract Column Build();

    protected abstract bool IsValidColumnType(ColumnType type);

    protected Type EntityType { get; set; }

    protected PropertyInfo PropertyInfo { get; set; }

    protected TableBuilder TableBuilder { get; set; }

}
