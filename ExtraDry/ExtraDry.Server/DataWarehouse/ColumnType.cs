using System.Text.Json.Serialization;

namespace ExtraDry.Server.DataWarehouse;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColumnType {
    Integer,
    Float,
    Text,
}
