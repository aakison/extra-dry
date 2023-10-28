namespace ExtraDry.Core;

/// <summary>
/// Represents a hierarchy query to filter against a hierarchy of items.  Can only be used with 
/// entities that implement <see cref="IHierarchyEntity{T}" />.
/// </summary>
public class HierarchyQuery : FilterQuery {

    /// <summary>
    /// The number of levels of the hierarchy to return.
    /// </summary>
    [Range(0, 16384)]
    public int Level { get; set; }

    /// <summary>
    /// The list of slugs for the specific entities to expand.
    /// </summary>
    public List<string> Expand { get; set; } = new();

    /// <summary>
    /// The list of slugs for the specific entities to collapse.
    /// </summary>
    public List<string> Collapse { get; set; } = new();

}
