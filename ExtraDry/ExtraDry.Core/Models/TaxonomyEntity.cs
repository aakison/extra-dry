namespace ExtraDry.Core;

/// <summary>
/// For an entity that exists in a hierarchy, creates extensions for an EF model to model it.
/// This uses a Closure table that tracks all ancestors and descendents between entities.
/// </summary>
public abstract class TaxonomyEntity<T> where T : TaxonomyEntity<T>, ITaxonomyEntity, IResourceIdentifiers {
    /// <summary>
    /// The immediate parent for this entity in the taxonomy.  Use to infer the tree structure 
    /// from multiple entities, and to re-parent an entity during an `Update`.
    /// </summary>
    /// <remarks>
    /// Derived classes should override this and replace with a JsonConverter to a ResourceReference.
    /// </remarks>
    [JsonIgnore]
    public virtual T? Parent { get; set; }

}
