using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server.Security;

/// <summary>
/// An authorization requirement that maps to the ABAC operation.
/// </summary>
public class AbacRequirement(
    AbacOperation operation)
    : IAuthorizationRequirement
{

    /// <summary>
    /// The operation that is requested for this ABAC operation, used to determine the policies 
    /// to apply.
    /// </summary>
    public AbacOperation Operation { get; init; } = operation;

    /// <summary>
    /// The requirement for creating a resource.
    /// </summary>
    public static AbacRequirement Create { get; } = new(AbacOperation.Create);

    /// <summary>
    /// The requirement for reading a resource.
    /// </summary>
    public static AbacRequirement Read { get; } = new(AbacOperation.Read);

    /// <summary>
    /// The requirement for updating a resource.
    /// </summary>
    public static AbacRequirement Update { get; } = new(AbacOperation.Update);

    /// <summary>
    /// The requirement for deleting a resource.
    /// </summary>
    public static AbacRequirement Delete { get; } = new(AbacOperation.Delete);

    /// <summary>
    /// The requirement for listing resources.
    /// </summary>
    public static AbacRequirement List { get; } = new(AbacOperation.List);

    /// <summary>
    /// The requirement for executing an RPC call related to a resource.
    /// </summary>
    public static AbacRequirement Execute { get; } = new(AbacOperation.Execute);

    /// <summary>
    /// The requirement for aggregate operations on a collection of entities.
    /// </summary>
    public static AbacRequirement Aggregate { get; } = new(AbacOperation.Aggregate);

}
