using System.Collections.Generic;

namespace ExtraDry.Server.Warehouse;

public class Table {

    public Table(string title)
    {
        Title = title;
    }

    public string Title { get; set; }

    public List<Column> Columns { get; } = new List<Column>();

    public List<Dictionary<string, object>> Data { get; } = new List<Dictionary<string, object>>();

}
