using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExtraDry.Server.DataWarehouse;

public class Table {

    public Table(Type type, string name)
    {
        EntityType = type;
        Name = name;
    }

    [JsonIgnore]
    public Type EntityType { get; set; }

    public string Name { get; set; }

    public List<Column> Columns { get; } = new List<Column>();

    public List<Dictionary<string, object>> Data { get; } = new List<Dictionary<string, object>>();

}
