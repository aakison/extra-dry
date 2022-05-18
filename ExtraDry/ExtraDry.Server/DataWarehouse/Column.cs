using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// A column for a table in the schema for the data warehouse.
/// </summary>
public class Column {

    /// <summary>
    /// Creates a new column of the indicate type and with the specified title.
    /// </summary>
    public Column(ColumnType type, string name)
    {
        ColumnType = type;
        Name = name;
    }

    public string Name { get; set; }

    public ColumnType ColumnType { get; set; }

    /// <summary>
    /// If the column is Text field, indicates the length of the text field.
    /// 0 implies maximum allowable length.
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

}

