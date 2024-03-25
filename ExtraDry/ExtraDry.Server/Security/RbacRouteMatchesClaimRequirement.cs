using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server;

/// <summary>
/// Requirement that the route parameter in a URI matches a claim value associated with the user.
/// </summary>
public class RbacRouteMatchesClaimRequirement : IAuthorizationRequirement
{

    /// <summary>
    /// The name of the route parameter in a URI (in the controller endpoint definition) that is 
    /// used to compare against the claim value.
    /// </summary>
    public string RouteParameter { get; set; } = "";

    /// <summary>
    /// The list of claim keys that are used to compare against the route parameter value.
    /// </summary>
    public string[] ClaimKeys { get; set; } = [];

    /// <summary>
    /// When matching the claim value, which portion of the claim value should be used.
    /// </summary>
    public ClaimValueMatch ClaimValueMatch { get; set; } = ClaimValueMatch.Exact;

    /// <summary>
    /// If a user has a role that is in this list, the requirement is satisfied.
    /// </summary>
    public string[] RoleOverrides { get; set; } = [];

}
