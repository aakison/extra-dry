namespace ExtraDry.Server.Warehouse;

public class Column {

    public Column(ColumnType type, string title)
    {
        ColumnType = type;
        Title = title;
    }

    public string Title { get; set; }

    public ColumnType ColumnType { get; set; }

}
