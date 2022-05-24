using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Using a Get property with Set method to provide a fluent builder interface for developers.
/// </remarks>
public abstract class ColumnBuilder {

    internal ColumnBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo)
    {
        EntityType = entityType;
        PropertyInfo = propertyInfo;
        TableBuilder = tableBuilder;
        columnName = DataConverter.CamelCaseToTitleCase(propertyInfo.Name);
    }

    public string ColumnName {
        get => columnName;
    } 
    private string columnName;

    public ColumnType ColumnType { 
        get => columnType; 
    }
    private ColumnType columnType;

    public bool Included {
        get => included;
    } 
    private bool included =true;

    public int? Length {
        get => length;
    }
    private int? length;

    protected void SetLength(int? length)
    {
        if(length != null && length < 0) {
            throw new DryException("Length must be a non-negative integer or null.");
        }
        this.length = length;
    }

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
            throw new DryException($"Names for tables must be unique, '{name}' is duplicated.");
        }
        columnName = name;
    }

    protected void SetType(ColumnType type)
    {
        if(!IsValidColumnType(type)) {
            throw new DryException("Column type is not valid.");
        }
        columnType = type;
    }

    protected void SetIncluded(bool included)
    {
        this.included = included;
    }

    internal abstract Column Build();

    protected abstract bool IsValidColumnType(ColumnType type);

    protected Type EntityType { get; set; }

    internal PropertyInfo PropertyInfo { get; set; }

    protected TableBuilder TableBuilder { get; set; }

}
