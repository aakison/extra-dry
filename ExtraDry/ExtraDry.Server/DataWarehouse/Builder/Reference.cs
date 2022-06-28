namespace ExtraDry.Server.DataWarehouse;

public class Reference {

    public Reference(string table, string column)
    {
        Table = table;
        Column = column;
    }

    public string Table { get; set; }

    public string Column { get; set; }
}
