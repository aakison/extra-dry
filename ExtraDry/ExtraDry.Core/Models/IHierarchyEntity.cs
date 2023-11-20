using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Core;

/// <summary>
/// Represents an entity that supports a hierarchy of values using the Lineage pattern.
/// This provides most of the desire functionality for a hierarchy, but does not provide
/// the ability to navigate the hierarchy.  Use <see cref="IHierarchyEntity{T}" /> for 
/// full functionality.
/// </summary>
public interface IHierarchyEntity
{
    /// <inheritdoc cref="IResourceIdentifiers.Slug" />
    string Slug { get; }

    /// <summary>
    /// A representation of a lineage for managing hierarchical data that has efficient operations
    /// when stored in a database.
    /// </summary>
    HierarchyId Lineage { get; }
}

/// <summary>
/// Represents an entity that supports a hierarchy of values using both the Lineage pattern and the
/// Parent pattern.  This provides the ability to navigate the hierarchy and is the preferred 
/// mechansim for hierarchy support.
/// </summary>
public interface IHierarchyEntity<T> : IHierarchyEntity where T : IHierarchyEntity<T>
{

    /// <summary>
    /// The parent entity in the taxonomy, NULL if root node.  This provides the referential 
    /// integrity for the hierarchy that the Lineage doesn't.
    /// </summary>
    T? Parent { get; }
}

[Obsolete("Use IHierarchyEntity`T instead")]
public interface ITaxonomyEntity
{

}
