namespace ExtraDry.Core;

/// <summary>
/// Standard payload for list controllers endpoints that return paged results.
/// </summary>
public class PageQuery : SortQuery, IPageQuery
{

    /// <inheritdoc cref="IPageQuery.Skip" />
    public int Skip { get; set; }

    /// <inheritdoc cref="IPageQuery.Take" />
    public int Take { get; set;}

    /// <inheritdoc cref="IPageQuery.Token" />
    public string? Token { get; set; }

    /// <summary>
    /// The default number of items to take if none provided.
    /// </summary>
    public const int DefaultTake = 100;
}
