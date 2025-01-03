namespace ExtraDry.Core;

/// <summary>
/// The payload of statistics about entities as returned by ToStatisti
/// </summary>
/// <remarks>
/// Type T is not strictly necessary, but provides consistency with other return types and allows
/// helper methods (such as auto-documentation) to work more easily.
/// </remarks>
public class Statistics<T>
{
    /// <summary>
    /// Create a statistics payload.
    /// </summary>
    public Statistics()
    {
        TypeName = typeof(T).Name;
    }

    /// <summary>
    /// The entity type for which the statistics are compiled.
    /// </summary>
    /// <example>Asset</example>
    [JsonPropertyName("type")]
    public string TypeName { get; set; }

    /// <summary>
    /// The UTC date/time that the statistics were created. The client could use this as part of a
    /// caching strategy, but this is not needed by the server.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// If the statistics cover a subset of all items, this is the query that was used to filter
    /// the full collection.
    /// </summary>
    /// <example>state:active</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Filter { get; set; }

    /// <summary>
    /// The properties that have distinct values along with the count of those values in the
    /// selection.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<DataDistribution>? Distributions { get; set; }
}
