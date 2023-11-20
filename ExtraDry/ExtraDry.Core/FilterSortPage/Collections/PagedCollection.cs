namespace ExtraDry.Core;

/// <summary>
/// A page of a sorted and filtered collection of items from the API with information on 
/// retrieving other pages.
/// </summary>
public class PagedCollection<T> : SortedCollection<T>
{

    /// <summary>
    /// The starting index of this partial collection within the full collection.
    /// </summary>
    /// <example>0</example>
    public int Start { get; set; }

    /// <summary>
    /// The total number of items in the full collection of items.
    /// </summary>
    /// <example>1</example>
    public int Total { get; set; }

    /// <summary>
    /// An arbitrary token sent by the server used to managed consistency of results.  As a 
    /// best-practice, always send this token back to the server when fetching additional partial 
    /// results, don't rely on the `Query` and `Start` indexes alone.
    /// </summary>
    /// <example>AAAAZAAAAGQAAAA=</example>
    public string? ContinuationToken { get; set; }

    /// <summary>
    /// Indicates if this partial collection is also the full collection.
    /// Typical when the collection on the server is small.
    /// </summary>
    [JsonIgnore]
    public bool IsFullCollection => Count == Total;

    /// <summary>
    /// Create a new PagedCollection with the items cast to a base class or interface.
    /// </summary>
    public new PagedCollection<TCast> Cast<TCast>() => new() {
        Filter = Filter,
        ContinuationToken = ContinuationToken,
        Created = Created,
        Items = Items.Cast<TCast>().ToList(),
        Sort = Sort,
        Start = Start,
        Total = Total,
    };

}
