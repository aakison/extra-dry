using System.Text.Json.Serialization;

namespace ExtraDry.Server.Warehouse;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColumnType {
    Integer,
    Float,
    Text,
}
