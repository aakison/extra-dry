namespace ExtraDry.Server.DataWarehouse;

public class Reference(string table, string column)
{
    public string Table { get; set; } = table;

    public string Column { get; set; } = column;
}
