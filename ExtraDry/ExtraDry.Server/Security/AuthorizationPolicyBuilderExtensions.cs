using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server;

/// <summary>
/// Extensions for building Authorization Policies.
/// </summary>
public static class AuthorizationPolicyBuilderExtensions
{

    /// <summary>
    /// Build a <see cref="AuthorizationPolicyBuilder"/> that requires a claim to match a route parameter.
    /// </summary>
    public static AuthorizationPolicyBuilder RequireRouteMatchesClaim(this AuthorizationPolicyBuilder builder, string routeParameter, string[] claimKeys, ClaimValueMatch match = ClaimValueMatch.Exact, string[]? roleOverrides = null)
    {
        builder.AddRequirements(new RouteMatchesClaimRequirement {
            RouteParameter = routeParameter,
            ClaimKeys = claimKeys,
            ClaimValueMatch = match,
            RoleOverrides = roleOverrides ?? [],
        });
        return builder;
    }

}
