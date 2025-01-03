using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// Type data types for columns in the schema for the data warehouse.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Integer is a math concept first.")]
public enum ColumnType
{
    /// <summary>
    /// Represents the key column for a table, only a single key is supported and it must be an
    /// Int.
    /// </summary>
    Key,

    /// <summary>
    /// An integer value column.
    /// </summary>
    Integer,

    /// <summary>
    /// A real value column.
    /// </summary>
    Real,

    /// <summary>
    /// A decimal/money field that has a fixed precision.
    /// </summary>
    Decimal,

    /// <summary>
    /// A text column, length of text specified in Column's Length property.
    /// </summary>
    Text,

    /// <summary>
    /// A date column, without time. Typically only occurs in the Date dimension.
    /// </summary>
    Date,

    /// <summary>
    /// A time column, without date. Typically only occurs in the Time dimension.
    /// </summary>
    Time,
}
