using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// A column for a table in the schema for the data warehouse.
/// </summary>
/// <remarks>Creates a new column of the indicate type and with the specified title.</remarks>
public class Column(
    ColumnType type,
    string name,
    Func<object, object>? converter = null)
{
    public string Name { get; set; } = name;

    public ColumnType ColumnType { get; set; } = type;

    /// <summary>
    /// If the column is Text field, indicates the length of the text field. 0 implies maximum
    /// allowable length.
    /// </summary>
    public int? Length { get; set; }

    public bool Nullable { get; set; }

    /// <summary>
    /// The reflected PropertyInfo, if the column is a property of a Fact or Dimension class.
    /// </summary>
    [JsonIgnore]
    public PropertyInfo? PropertyInfo { get; set; }

    /// <summary>
    /// If the column is a foreign key to a dimension, the information on referenced table/column.
    /// </summary>
    public Reference? Reference { get; set; }

    /// <summary>
    /// When using a decimal, the precision that is stored. Default aligned with Entity Framework
    /// default.
    /// </summary>
    public string Precision { get; set; } = "18,2";

    /// <summary>
    /// The default value for the column if no data is present. Data warehouses don't work well
    /// with null values, so replace with some content.
    /// </summary>
    public object Default { get; set; } = new();

    /// <summary>
    /// Functor that converts data from source database to destination data warehouse.
    /// </summary>
    [JsonIgnore]
    public Func<object, object> Converter { get; set; } = converter ?? (e => e);
}
