namespace ExtraDry.Core;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CrudOperation
{
    /// <summary>
    /// The operation is a HTTP POST to create a new resource.
    /// </summary>
    Create = 1,

    /// <summary>
    /// The operation is a HTTP GET to read an existing resource.
    /// </summary>
    Read = 2,

    /// <summary>
    /// The operation is a HTTP PUT to update an existing resource.
    /// </summary>
    Update = 4,

    /// <summary>
    /// The operation is a HTTP DELETE to delete an existing resource.
    /// </summary>
    Delete = 8,

    /// <summary>
    /// The operation is a HTTP POST to a custom RPC endpoint that does not fit into standard RESTful principles.
    /// </summary>
    Rpc = 16,

    /// <summary>
    /// Represents a combination of permissions for existing resources, including read, update, and delete operations.
    /// </summary>
    Existing = Read | Update | Delete,

    /// <summary>
    /// Specifies the set of operations that modify data, including create, update, and delete actions.
    /// </summary>
    Mutating = Create | Update | Delete,

    /// <summary>
    /// Represents a combination of all available permissions: Create, Read, Update, and Delete.
    /// </summary>
    All = Create | Read | Update | Delete | Rpc,
}

