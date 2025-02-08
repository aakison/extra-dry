namespace ExtraDry.Core;

/// <summary>
/// Represents a basic query to filter against a list of items with results sorted.
/// </summary>
public class SortQuery : FilterQuery
{
    /// <summary>
    /// If the request would like sorted results, the name of the property to sort by. Prefix with
    /// '+' or '-' to order ascending or descending.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Stabalization for the query which adds an additional property as a secondary sort. Some
    /// providers don't support this so the default is `None`.
    /// </summary>
    public SortStabilization Stabilization { get; set; } = SortStabilization.None;
}
