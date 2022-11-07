namespace ExtraDry.Core;

/// <summary>
/// Represents a basic query to filter against a list of items.
/// </summary>
public class FilterQuery {

    /// <summary>
    /// The entity specific text filter for the collection.
    /// </summary>
    public string? Filter { get; set; } 

    /// <summary>
    /// If the request would like sorted results, the name of the property to sort by.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Indicates if the results are requested in ascending order by `Sort`.
    /// </summary>
    public bool Ascending { get; set; }

}
