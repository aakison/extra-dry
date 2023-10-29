namespace ExtraDry.Core;

/// <summary>
/// A filtered collection of hierarcy items sorted breadth-first from the API.
/// </summary>
public class HierarchyCollection<T> : SortedCollection<T>
{
    /// <summary>
    /// The maximum depth of the hierarchy included in the results.  If the collection has a 
    /// filter, the level is not included.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Level { get; set; }

    /// <summary>
    /// The list of additional nodes, if any, that were expanded.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? Expand { get; set; }

    /// <summary>
    /// The list of additional nodes, if any, that were collapsed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? Collapse { get; set; }

    /// <summary>
    /// Create a new <see cref="HierarchyCollection{T}" /> with the items cast to a base class or interface.
    /// </summary>
    public new HierarchyCollection<TCast> Cast<TCast>() => new() {
        Filter = Filter,
        Created = Created,
        Sort = Sort,
        Level = Level,
        Expand = Expand,
        Collapse = Collapse,
        Items = Items.Cast<TCast>().ToList(),
    };

}

