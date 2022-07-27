namespace ExtraDry.Core.Models;

/// <summary>
/// A message for queuing transports to indicate an event has occurred on an entity type.
/// </summary>
public class EntityMessage {

    public EntityMessage(string entityName)
    {
        EntityName = entityName;
    }

    /// <summary>
    /// The name of the entity (i.e. entity.GetType().Name).
    /// </summary>
    public string EntityName { get; set; }

}
