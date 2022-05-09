using System.Text.Json.Serialization;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// Type data types for columns in the schema for the data warehouse.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColumnType {

    /// <summary>
    /// Represents the key column for a table, only a single key is supported and it must be an Int.
    /// </summary>
    Key,

    /// <summary>
    /// An integer value column.
    /// </summary>
    Integer,

    /// <summary>
    /// A double value column
    /// </summary>
    Double,

    /// <summary>
    /// A text column, length of text specified in Column's Length property.
    /// </summary>
    Text,
}
