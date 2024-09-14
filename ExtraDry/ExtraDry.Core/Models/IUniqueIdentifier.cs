namespace ExtraDry.Core;

/// <summary>
/// Represents an entity with a unique identifier.  Generally useful for other interfaces for 
/// commonality and not implemented directly.
/// </summary>
public interface IUniqueIdentifier
{
    /// <summary>
    /// A universally unique identifier for this new resource.
    /// </summary>
    /// <example>e8b79f39-3398-4aed-9339-7250166204e5</example>
    public Guid Uuid { get; set; }

}
