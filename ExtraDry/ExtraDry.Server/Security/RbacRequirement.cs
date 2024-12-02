using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server.Security;

/// <summary>
/// Requirement for authorization that is backed by ABAC rules, but doesn't assess the resource.
/// Useful for matching the RBAC rule in `[Authorize("Policy")]` attributes to the subset of ABAC
/// rules matched in `IAuthorizationService.IsAuthorized(...)`.
/// </summary>
public class RbacRequirement(
    string abacPolicyName)
    : IAuthorizationRequirement
{

    /// <summary>
    /// The name of the policy that is used to determine the ABAC policy in the 
    /// <see cref="AbacOptions"/>.  Note that names of policies are optional, so must provide a
    /// unique name on the policy to match to.
    /// </summary>
    public string PolicyName { get; init; } = abacPolicyName;

}
