namespace ExtraDry.Core;

/// <summary>
/// Standard payload for list controllers endpoints that return paged results, e.g. using `PartialCollection`.
/// </summary>
public class PageQuery : SortQuery {

    /// <summary>
    /// The number of records to skip before returning results.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// The requested number of records to take.  
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    /// The continuation token from the previous response.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// The default number of items to take if none provided.
    /// </summary>
    public const int DefaultTake = 100;
}
