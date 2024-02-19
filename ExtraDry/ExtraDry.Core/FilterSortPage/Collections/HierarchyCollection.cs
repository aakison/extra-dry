namespace ExtraDry.Core;

/// <summary>
/// A filtered collection of hierarcy items sorted depth-first from the API.
/// </summary>
public class HierarchyCollection<T> : SortedCollection<T>
{
    /// <summary>
    /// The depth of the hierarchy included in the results as defined in the request.  If the 
    /// collection has a filter, the level is not included.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Level { get; set; }

    /// <summary>
    /// The maximum level of the hierarchy for the collection, ignoring any Level filter that may 
    /// be applied.  
    /// </summary>
    public int MaxLevels { get; set; }

    /// <summary>
    /// The minimum level of the hierarchy for the collection, ignoring any Level filter that may 
    /// be applied.  
    /// </summary>
    public int MinLevels { get; set; }

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
    /// The list of nodes in the collection that have children and could be expanded.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? Expandable { get; set; }

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

