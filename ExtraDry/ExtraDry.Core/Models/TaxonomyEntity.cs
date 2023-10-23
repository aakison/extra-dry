namespace ExtraDry.Core;

/// <summary>
/// For an entity that exists in a hierarchy, creates extensions for an EF model to model it.
/// This uses a Closure table that tracks all ancestors and descendents between entities.
/// </summary>
public abstract class TaxonomyEntity<T> where T : TaxonomyEntity<T>, ITaxonomyEntity, IResourceIdentifiers {

    /// <summary>
    /// Useful for creating a new `T` which is the direct child of a parent `T`.
    /// If parent is null, then the entity is set at the root of the taxonomy.
    /// </summary>
    public void SetParent(T? parent)
    {
        if(parent != null) {
            if(parent.Strata >= ((ITaxonomyEntity)this).Strata) {
                throw new InvalidOperationException("Parent must be at a higher level than current entity.");
            }
        }
        Parent = parent;
    }

    /// <summary>
    /// The immediate parent for this entity in the taxonomy.  Use to infer the tree structure 
    /// from multiple entities, and to re-parent an entity during an `Update`.
    /// </summary>
    /// <remarks>
    /// Derived classes should override this and replace with a JsonConverter to a ResourceReference.
    /// </remarks>
    [JsonIgnore]
    public virtual T? Parent { get; set; }

    // TODO - SG: Remove?

    ///// <summary>
    ///// The set of all ancestors for this entity.
    ///// </summary>
    ///// <remarks>
    ///// Derived classes should use their data provider of choice to populate this list.
    ///// </remarks>
    //[JsonIgnore]
    //[NotMapped]
    //public virtual List<T> Ancestors { get; set; }

    ///// <summary>
    ///// The set of all descendants for this entity.
    ///// </summary>
    ///// <remarks>
    ///// Derived classes should use their data provider of choice to populate this list.
    ///// </remarks>
    //[JsonIgnore]
    //[NotMapped]
    //public virtual List<T> Descendants { get; set; }

}
