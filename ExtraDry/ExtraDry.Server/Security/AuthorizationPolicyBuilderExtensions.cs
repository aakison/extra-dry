using ExtraDry.Server.Security;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server;

/// <summary>
/// Extensions for building Authorization Policies.
/// </summary>
public static class AuthorizationPolicyBuilderExtensions
{

    /// <summary>
    /// Build a policy that requires a claim to match a route parameter to user claims.
    /// </summary>
    public static AuthorizationPolicyBuilder RequireRouteMatchesClaim(this AuthorizationPolicyBuilder builder, string routeParameter, string[] claimKeys, ClaimValueMatch match = ClaimValueMatch.Exact, string[]? roleOverrides = null)
    {
        builder.AddRequirements(new RbacRouteMatchesClaimRequirement {
            RouteParameter = routeParameter,
            ClaimKeys = claimKeys,
            ClaimValueMatch = match,
            RoleOverrides = roleOverrides ?? [],
        });
        return builder;
    }

    /// <summary>
    /// Build a policy that matches a configurable policy claim.
    /// </summary>
    public static AuthorizationPolicyBuilder AddRbacRequirement(this AuthorizationPolicyBuilder builder, string abacPolicyName)
    {
        builder.AddRequirements(new RbacRequirement(abacPolicyName));
        return builder;
    }

}
