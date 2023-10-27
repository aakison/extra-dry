namespace ExtraDry.Core;

/// <summary>
/// Represents a generic payload for returning lists of items from an API.
/// </summary>
public class FilteredCollection<T> {

    /// <summary>
    /// The UTC date/time that the partial results were created.
    /// The client could use this as part of a caching strategy, but this is not needed by the server.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// If the full collection is a subset of all items, this is the query that was used to filter the full collection.
    /// </summary>
    /// <example>term property:value</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Filter { get; set; }

    /// <summary>
    /// If the collection is sorted, this is the name of the Property the sort is performed on.
    /// </summary>
    /// <example>property</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Sort { get; set; }

    /// <summary>
    /// The total number of items in the result set.
    /// </summary>
    /// <example>1</example>
    public int Count => Items.Count;

    /// <summary>
    /// The actual collection of items.
    /// </summary>
    public IList<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Create a new FilteredCollection with the items cast to a base class or interface.
    /// </summary>
    public FilteredCollection<TCast> Cast<TCast>() => new() {
        Filter = Filter,
        Created = Created,
        Sort = Sort,
        Items = Items.Cast<TCast>().ToList(),
    };

}

