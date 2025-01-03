namespace ExtraDry.Server.EF;

/// <summary>
/// Arguments with information on the entities that have changed.
/// </summary>
/// <remarks>Create args with the added, updated and deleted entities.</remarks>
public class EntitiesChanged(
    IEnumerable<object> added,
    IEnumerable<object> modified,
    IEnumerable<object> deleted,
    AspectDbContext context)
{
    public AspectDbContext Context { get; private set; } = context;

    /// <summary>
    /// The set of entities that are being added in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesAdded { get; private set; } = added;

    /// <summary>
    /// The set of entities that are being modified in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesModified { get; private set; } = modified;

    /// <summary>
    /// The set of entities that are being deleted in this SaveChanges action.
    /// </summary>
    public IEnumerable<object> EntitiesDeleted { get; private set; } = deleted;

    /// <summary>
    /// The timestamp for the change.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
