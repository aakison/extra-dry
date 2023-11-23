namespace ExtraDry.Server.EF;

/// <summary>
/// Arguments with information on the entities that have changed.
/// </summary>
public class EntitiesChangedEventArgs : EventArgs {

    /// <summary>
    /// Create args with the added, updated and deleted entities.
    /// </summary>
    public EntitiesChangedEventArgs(IEnumerable<object> added, IEnumerable<object> modified, IEnumerable<object> deleted)
    {
        EntitiesAdded = added;
        EntitiesModified = modified;
        EntitiesDeleted = deleted;
    }

    /// <summary>
    /// The set of entities that are being added in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesAdded { get; private set; }

    /// <summary>
    /// The set of entities that are being modified in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesModified { get; private set; }

    /// <summary>
    /// The set of entities that are being deleted in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesDeleted { get; private set; }
}
