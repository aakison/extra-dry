using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace ExtraDry.Server;

/// <summary>
/// Checkes the <see cref="RbacRouteMatchesClaimRequirement"/> for a tenant route parameter to match a 
/// claim value.
/// </summary>
public class RbacRouteMatchesClaimRequirementHandler(
    IHttpContextAccessor httpContextAccessor) 
    : AuthorizationHandler<RbacRouteMatchesClaimRequirement>
{

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RbacRouteMatchesClaimRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if(httpContext == null) {
            return;
        }
        var user = httpContext.User;
        if(user == null) {
            return;
        }
        var roleOverride = requirement.RoleOverrides.Any(user.IsInRole);
        if(roleOverride) {
            context.Succeed(requirement);
            return;
        }
        var routeValue = httpContext.GetRouteData()?.Values[requirement.RouteParameter]?.ToString();
        if(routeValue == null) {
            return;
        }
        var claims = user.Claims;
        if(claims == null) {
            return;
        }
        Func<Claim, string, bool> valueMatch = requirement.ClaimValueMatch switch {
            ClaimValueMatch.Exact => (Claim c, string e) => c.Value.Equals(e, StringComparison.Ordinal),
            ClaimValueMatch.FirstPath => (Claim c, string e) => c.Value.Equals(e, StringComparison.Ordinal) || c.Value.StartsWith($"{e}@", StringComparison.Ordinal),
            ClaimValueMatch.LastPath => (Claim c, string e) => c.Value.Equals(e, StringComparison.Ordinal) || c.Value.EndsWith($"@{e}", StringComparison.Ordinal),
            _ => (Claim c, string e) => false
        };
        var matchingClaim = claims.FirstOrDefault(e => requirement.ClaimKeys.Contains(e.Type) && valueMatch(e, routeValue));
        if(matchingClaim != null) {
            context.Succeed(requirement);
        }

        await Task.CompletedTask;
    }
}
