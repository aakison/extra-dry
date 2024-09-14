namespace ExtraDry.Server.Security;

/// <summary>
/// The ABAC operation that is being checked for authorization purposes.  This is used to determine
/// which configured rule to apply to the authorization check.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AbacOperation
{
    /// <summary>
    /// The operation is to create a new entity.
    /// </summary>
    Create,

    /// <summary>
    /// The operation is to read an entity.
    /// </summary>
    Read,

    /// <summary>
    /// The operation is to update an entity.
    /// </summary>
    Update,

    /// <summary>
    /// The operation is to delete an entity.
    /// </summary>
    Delete,

    /// <summary>
    /// The operation is to list a collection of entities.
    /// </summary>
    List,

    /// <summary>
    /// The operation is to perform aggregate operations on a collection of entities, such as sum, average, etc.
    /// </summary>
    Aggregate,

    /// <summary>
    /// The operation is to execute a command, e.g. an RPC call, related to the entity.
    /// </summary>
    Execute,
}
