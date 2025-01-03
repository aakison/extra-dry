namespace ExtraDry.Core;

/// <summary>
/// Page query payload for controller endpoints that return paged results.
/// </summary>
public interface IPageQuery
{
    /// <summary>
    /// The number of records to skip before returning results.
    /// </summary>
    int Skip { get; set; }

    /// <summary>
    /// The requested number of records to take.
    /// </summary>
    int Take { get; set; }

    /// <summary>
    /// The continuation token from the previous response.
    /// </summary>
    string? Token { get; set; }
}
