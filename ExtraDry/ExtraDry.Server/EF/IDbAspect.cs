namespace ExtraDry.Server.EF;

/// <summary>
/// A database aspect that can be used to intercept and modify entities as they are being saved.
/// </summary>
public interface IDbAspect
{
    /// <summary>
    /// Callback when entities are changing, allowing property updates and new entities to be
    /// populated.
    /// </summary>
    void EntitiesChanging(EntitiesChanged args);

    /// <summary>
    /// Callback when entities have changed, allowing property updates, but no new entities.
    /// </summary>
    void EntitiesChanged(EntitiesChanged args);
}
