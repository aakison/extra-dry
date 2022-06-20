namespace ExtraDry.Core;

/// <summary>
/// Standard payload for list controllers endpoints that return paged results, e.g. using `PartialCollection`.
/// </summary>
public class PageQuery : FilterQuery {

    /// <summary>
    /// The number of records to skip before returning results.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// The requested number of records to take.  
    /// Actual result might be less based on available records or endpoint limitations.
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    /// The continuation token from the previous response.
    /// When provided, this will override other options such as `Sort` and `Filter`, but not `Skip` and `Take`.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// The default number of items to take if none provided.
    /// </summary>
    public const int DefaultTake = 100;
}
