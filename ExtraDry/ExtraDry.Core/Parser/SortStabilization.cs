namespace ExtraDry.Core;

/// <summary>
/// Defines the behaviour of ExtraDry when providing a sort method to an underlying data store.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortStabilization
{
    /// <summary>
    /// Add the primary key to sort queries to provide a stabalized sort across multiple requests
    /// or database re-indexes. Ideal for relational databases.
    /// </summary>
    PrimaryKey = 0,

    /// <summary>
    /// When retrieving list from a data store this will not use a secondary property to enforce a
    /// stabilized sort. Ideal for CosmosDB or other NoSQL databases, where sorting by 2 columns is
    /// not supported, but the results are already stable.
    /// </summary>
    None = 1,
}
