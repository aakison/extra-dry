using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ExtraDry.Server.Security;

/// <summary>
/// Authorization handler for RBAC requirements that is backed by ABAC rules.
/// </summary>
public class RbacRequirementHandler
    (AbacOptions options,
    IHttpContextAccessor contextAccessor)
    : AuthorizationHandler<RbacRequirement>
{

    /// <inheritdoc cref="RbacRequirementHandler" />
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RbacRequirement requirement)
    {
        var helper = new AbacAuthorizationHelper(options);
        var route = contextAccessor.HttpContext?.Request.RouteValues;
        var user = context.User == AuthorizationServiceExtensions.DeferredPrincipal
            ? contextAccessor.HttpContext?.User
            : context.User;
        var policy = options.Policies.FirstOrDefault(p => p.Name == requirement.PolicyName);
        if(policy == null) {
            return Task.CompletedTask;
        }
        if(helper.IsAuthorized(user, route, Empty, policy)) {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// An empty object to use as the resource for the ABAC check.  This should be null context as 
    /// this is an RBAC check, but pass something through as Helper expects a target object.
    /// </summary>
    private static object Empty { get; } = new();

}
