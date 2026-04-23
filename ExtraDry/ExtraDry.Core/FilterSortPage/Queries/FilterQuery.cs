namespace ExtraDry.Core;

/// <summary>
/// Represents a basic query to filter against a list or hierarchy of items.
/// </summary>
public class FilterQuery
{
    /// <summary>
    /// The entity specific text filter for the collection.
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// The string comparison to be used, defaults to `null` as EF database queries don't support
    /// explicit StringComparison.
    /// </summary>
    internal StringComparison? Comparison { get; set; }

    /// <summary>
    /// If the request would like sorted results, the name of the property to sort by. Prefix with
    /// '+' or '-' to order ascending or descending.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Stabalization for the query which adds an additional property as a secondary sort. Some
    /// providers don't support this so the default is `None`.
    /// </summary>
    internal SortStabilization Stabilization { get; set; } = SortStabilization.None;
}
