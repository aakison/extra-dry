using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Core;

/// <summary>
/// Represents an entity that supports a hierarchy of values.
/// </summary>
public interface IHierarchyEntity<T> where T : IHierarchyEntity<T> {

    /// <summary>
    /// The parent entity in the taxonomy, NULL if root node.  This provides the referential 
    /// integrity for the hierarchy that the Lineage doesn't.
    /// </summary>
    T? Parent { get; }

    /// <summary>
    /// A representation of a lineage for managing hierarchical data that has efficient operations
    /// when stored in a database.
    /// </summary>
    HierarchyId Lineage { get; }

}

[Obsolete("Use IHierarchyEntity`T instead")]
public interface ITaxonomyEntity {

}
