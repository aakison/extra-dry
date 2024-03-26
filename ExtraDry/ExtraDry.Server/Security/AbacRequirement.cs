using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server.Security;

public class AbacRequirement(
    AbacOperation operation)
    : IAuthorizationRequirement
{
    public AbacOperation Operation { get; init; } = operation;

    public static AbacRequirement Create { get; } = new(AbacOperation.Create);

    public static AbacRequirement Read { get; } = new(AbacOperation.Read);

    public static AbacRequirement Update { get; } = new(AbacOperation.Update);

    public static AbacRequirement Delete { get; } = new(AbacOperation.Delete);

    public static AbacRequirement List { get; } = new(AbacOperation.List);

    public static AbacRequirement Execute { get; } = new(AbacOperation.Execute);

    public static AbacRequirement Aggregate { get; } = new(AbacOperation.Aggregate);

}
