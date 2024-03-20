using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ExtraDry.Server;

/// <summary>
/// Checkes the <see cref="RouteMatchesClaimRequirement"/> for a tenant route parameter to match a 
/// claim value.
/// </summary>
public class RouteMatchesClaimRequirementHandler(
    IHttpContextAccessor httpContextAccessor) 
    : AuthorizationHandler<RouteMatchesClaimRequirement>
{

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RouteMatchesClaimRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if(httpContext == null) {
            return;
        }
        var routeValue = httpContext.GetRouteData()?.Values[requirement.RouteParameter]?.ToString();
        if(routeValue == null) {
            return;
        }

        var matchingClaim = httpContext?.User?.Claims.FirstOrDefault(e => requirement.ClaimKeys.Contains(e.Type) && routeValue == e.Value);

        if(matchingClaim != null) {
            context.Succeed(requirement);
        }

        await Task.CompletedTask;
    }
}
