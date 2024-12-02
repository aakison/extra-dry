using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ExtraDry.Server.Security;

/// <summary>
/// Authorization handler for RBAC requirements that is backed by ABAC rules.
/// </summary>
public class AbacRequirementHandler
    (AbacOptions options,
    IHttpContextAccessor contextAccessor)
    : AuthorizationHandler<AbacRequirement>
{

    /// <inheritdoc />
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacRequirement requirement)
    {
        var helper = new AbacAuthorizationHelper(options);
        var route = contextAccessor.HttpContext?.Request.RouteValues;
        if(context.Resource != null) {
            var user = context.User == AuthorizationServiceExtensions.DeferredPrincipal
                ? contextAccessor.HttpContext?.User
                : context.User;
            var success = helper.IsAuthorized(user, route, context.Resource, requirement.Operation);
            if(success) {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
