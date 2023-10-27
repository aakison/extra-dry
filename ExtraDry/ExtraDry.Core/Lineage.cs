namespace ExtraDry.Core;

/// <summary>
/// A database agnostic representation of a lineage for managing hierarchical data in databases.
/// </summary>
/// <remarks>
/// Extra DRY is taking a dependency on HeirarchyId which is a Microsoft SQL Server specific type.
/// Lineage is conceptually a non-SQL Server specific representation of HierarchyId and could be
/// expanded to provide a lot of the functionality of HierarchyId in a database-agnostics fashion.
/// For now, this is being used by <see cref="IHierarchyEntity{T}"/> to provide a common interface 
/// for potential expansion in the future.
/// </remarks>
[JsonConverter(typeof(LineageConverter))]
public class Lineage {

    /// <summary>
    /// Create a lineage with a path of nodes, e.g. /1/3/10/
    /// </summary>
    /// <param name="path">The path of nodes</param>
    public Lineage(string path = "/")
    {
        Path = path;
    }

    /// <summary>
    /// The path that represents the lineage for this entity, e.g. /1/3/10/
    /// </summary>
    public string Path { get; }

}
