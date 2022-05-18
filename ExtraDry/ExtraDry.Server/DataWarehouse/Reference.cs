namespace ExtraDry.Server.DataWarehouse;

public class Reference {

    public Reference(Table table, Column column)
    {
        Table = table;
        Column = column;
    }

    [JsonIgnore]
    public Table Table { get; set; }

    [JsonIgnore]
    public Column Column { get; set; }

    public string TableName => Table.Name;

    public string ColumnName => Column.Name;
}
