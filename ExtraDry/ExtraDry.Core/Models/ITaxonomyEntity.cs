namespace ExtraDry.Core;

/// <summary>
/// Represents the addition property required for an entity to support a taxonomy.
/// </summary>
public interface ITaxonomyEntity : INamedSubject {

    /// <summary>
    /// Represents the level of the taxonomy, by convention 0 is the top and each level below that increments by 1.
    /// Implementations can provide a `set` and use this as an integer, or can map to an internal concept.
    /// For example, create a enum for the taxonomy levels and export it with an int case.
    /// More complicated example, create a hierarchy of entities and use the `Type` of the entity to determine Strata.
    /// </summary>
    int Strata { get; }
}
