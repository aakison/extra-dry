namespace ExtraDry.Core;

/// <summary>
/// A page of a filtered collection of hierarchy items items sorted breadth-first from the API with 
/// information on retrieving other pages.
/// </summary>
public class PagedHierarchyCollection<T> : HierarchyCollection<T> {

    /// <inheritdoc cref="PagedCollection{T}.Start" />
    public int Start { get; set; }

    /// <inheritdoc cref="PagedCollection{T}.Total" />
    public int Total { get; set; }

    /// <inheritdoc cref="PagedCollection{T}.IsFullCollection" />
    [JsonIgnore]
    public bool IsFullCollection => Count == Total;

    /// <summary>
    /// Create a new PagedHierarchyCollection with the items cast to a base class or interface.
    /// </summary>
    public new PagedHierarchyCollection<TCast> Cast<TCast>() => new() {
            Filter = Filter,
            Created = Created,
            Items = Items.Cast<TCast>().ToList(),
            Sort = Sort,
            Start = Start,
            Total = Total,
            Level = Level,
            Expand = Expand == null ? null : (string[])Expand.Clone(),
            Collapse = Collapse == null ? null : (string[])Collapse.Clone(),
    };

}
