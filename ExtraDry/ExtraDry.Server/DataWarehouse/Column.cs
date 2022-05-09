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

    public int Length { get; set; } = int.MaxValue;

    public bool IsNullable { get; set; }

}
