namespace ExtraDry.Core;

/// <summary>
/// Represents a basic query to filter against a list of items with sorting enabled.
/// </summary>
public class SortQuery : FilterQuery {

    /// <summary>
    /// If the request would like sorted results, the name of the property to sort by.
    /// Prefix with '+' or '-' to order ascending or descending.
    /// </summary>
    /// <example>+model</example>
    public string? Sort { get; set; }

}
