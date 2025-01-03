namespace ExtraDry.Core;

/// <summary>
/// Resolves an entity on the server from a sample of that entity, typically sent over the network.
/// Service classes will typically implement this so that child objects of a resource can be
/// resolved to the appropriate server-side entity. E.g. When a controller recieves a `Company`
/// with a child `Sector` it will have received a deserialized copy of sector, this sector is not
/// linked back to the data store and the system might need to have the 'right' instance of sector
/// resolved.
/// </summary>
public interface IEntityResolver<T>
{
    /// <summary>
    /// Returns the correct object for the given examplar. This could be an existing one looked up
    /// by a Unique ID, or the examplar could be returned after being attached to the database,
    /// etc.
    /// </summary>
    Task<T?> ResolveAsync(T exemplar);
}
