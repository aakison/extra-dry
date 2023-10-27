namespace ExtraDry.Core;

/// <summary>
/// Page query payload for controller endpoints that return paged results.
/// </summary>
public interface IPageQuery {

    /// <summary>
    /// The number of records to skip before returning results.
    /// </summary>
    /// <example>100</example>
    int Skip { get; set; }

    /// <summary>
    /// The requested number of records to take.  
    /// </summary>
    /// <example>50</example>
    int Take { get; set; }

    /// <summary>
    /// The continuation token from the previous response.
    /// </summary>
    /// <example>AAAUAAAACgAAAA==</example>
    string? Token { get; set; }

}
