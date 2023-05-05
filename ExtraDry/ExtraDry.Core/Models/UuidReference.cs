namespace ExtraDry.Core;

/// <summary>
/// A simple container for holding a Universially Unique Identifier (UUID), such as returned from a Create method.
/// </summary>
[Obsolete("Use ResourceReference instead")]
public class UuidReference {

    /// <summary>
    /// Create with mandatory UUID.
    /// </summary>
    public UuidReference(Guid uuid) => Uuid = uuid;

    /// <summary>
    /// The universally unique identifier.
    /// </summary>
    public Guid Uuid { get; }

}
