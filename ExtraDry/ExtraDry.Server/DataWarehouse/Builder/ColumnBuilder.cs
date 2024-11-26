using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Using a Get property with Set method to provide a fluent builder interface for developers.
/// </remarks>
public abstract class ColumnBuilder
{

    internal ColumnBuilder(TableBuilder tableBuilder, Type entityType, PropertyInfo propertyInfo)
    {
        EntityType = entityType;
        PropertyInfo = propertyInfo;
        TableBuilder = tableBuilder;
        ColumnName = DataConverter.CamelCaseToTitleCase(propertyInfo.Name);
        Converter = e => e;
    }

    public string ColumnName { get; private set; }

    public ColumnType ColumnType { get; private set; }

    public bool Included { get; private set; } = true;

    public int? Length { get; private set; }

    public (int precision, int scale) Precision => precision;
    private (int precision, int scale) precision = (18, 2);

    public object Default { get; private set; } = new();

    protected void SetLength(int? length)
    {
        if(length is not null and < 0) {
            throw new DryException($"Column '{ColumnName}' length must be a non-negative integer or null.");
        }
        Length = length;
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
            throw new DryException($"Names for columns must be unique, '{name}' is duplicated on '{TableBuilder.TableName}'.");
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

    protected void SetPrecision(int precision, int scale)
    {
        if(precision <= 0) {
            throw new DryException("Decimal precision must have a positive integer length.");
        }
        if(scale < 0) {
            throw new DryException("Decimal scale must have non-negative length.");
        }
        if(precision > 38) {
            throw new DryException("Decimal precision is limited to 38 bytes by .NET, even though SQL Server can handle more.");
        }
        if(scale > precision) {
            throw new DryException("Scale must be less than or equal to the precision.");
        }
        this.precision = (precision, scale);
    }

    protected void SetIncluded(bool included)
    {
        Included = included;
    }

    protected void SetConverter(Func<object, object> converter)
    {
        Converter = converter;
    }

    protected void SetDefault(object @default)
    {
        Default = @default;
    }

    protected Func<object, object> Converter { get; private set; }

    internal abstract Column Build();

    protected abstract bool IsValidColumnType(ColumnType type);

    protected Type EntityType { get; set; }

    internal PropertyInfo PropertyInfo { get; set; }

    protected TableBuilder TableBuilder { get; set; }


}
