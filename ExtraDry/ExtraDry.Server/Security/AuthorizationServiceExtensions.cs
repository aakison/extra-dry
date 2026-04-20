using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ExtraDry.Server.Security;

/// <summary>
/// Extensions for IAuthorizationService to simplify ABAC checks.
/// </summary>
public static class AuthorizationServiceExtensions
{
    /// <summary>
    /// Checks if a user meets a specific ABAC requirement for the specified resource. The user is
    /// automatically determined from the HttpContext.
    /// </summary>
    /// <param name="resource">The resource the policy should be checked with.</param>
    /// <param name="policyName">The policy to check</param>
    /// <returns>Throws an exception if the user is not successfully authorized.</returns>
    public static async Task AssertAuthorizedAsync(this IAuthorizationService auth, object resource, string policyName)
    {
        var result = await auth.AuthorizeAsync(DeferredPrincipal, resource, policyName);
        if(!result.Succeeded) {
            throw new UnauthorizedAccessException(result.Failure.ToString());
        }
    }

    /// <summary>
    /// Checks if a user meets a specific ABAC requirement for the specified resource. The user is
    /// automatically determined from the HttpContext.
    /// </summary>
    /// <param name="resource">The resource the policy should be checked with.</param>
    /// <param name="requirement">
    /// The ABAC requirement to evaluate, e.g. `AbacRequirement.Read`
    /// </param>
    /// <returns>Throws an exception if the user is not successfully authorized.</returns>
    public static async Task AssertAuthorizedAsync(this IAuthorizationService auth, object resource, AbacRequirement requirement)
    {
        var result = await auth.AuthorizeAsync(DeferredPrincipal, resource, requirement);
        if(!result.Succeeded) {
            throw new UnauthorizedAccessException();
        }
    }

    /// <summary>
    /// Checks if a user meets a specific RBAC requirement. The user is automatically determined from the HttpContext.
    /// </summary>
    /// <param name="policyName">The policy to check</param>
    /// <returns>`true` if the user is successfully authorized.</returns>
    public static async Task<bool> IsAuthorizedAsync(this IAuthorizationService auth, string policyName)
    {
        var result = await auth.AuthorizeAsync(DeferredPrincipal, policyName);
        return result.Succeeded;
    }

    /// <summary>
    /// Checks if a user meets a specific ABAC requirement for the specified resource. The user is
    /// automatically determined from the HttpContext.
    /// </summary>
    /// <param name="resource">The resource the policy should be checked with.</param>
    /// <param name="policyName">The policy to check</param>
    /// <returns>`true` if the user is successfully authorized.</returns>
    public static async Task<bool> IsAuthorizedAsync(this IAuthorizationService auth, object resource, string policyName)
    {
        var result = await auth.AuthorizeAsync(DeferredPrincipal, resource, policyName);
        return result.Succeeded;
    }

    /// <summary>
    /// Checks if a user meets a specific ABAC requirement for the specified resource. The user is
    /// automatically determined from the HttpContext.
    /// </summary>
    /// <param name="resource">The resource the policy should be checked with.</param>
    /// <param name="requirement">
    /// The ABAC requirement to evaluate, e.g. `AbacRequirement.Read`
    /// </param>
    /// <returns>`true` if the user is successfully authorized.</returns>
    public static async Task<bool> IsAuthorizedAsync(this IAuthorizationService auth, object resource, AbacRequirement requirement)
    {
        var result = await auth.AuthorizeAsync(DeferredPrincipal, resource, requirement);
        return result.Succeeded;
    }

    /// <summary>
    /// Asserts ABAC authorization using the <see cref="AbacPolicyAttribute"/> declared on the
    /// calling controller action. Uses <see cref="CallerMemberNameAttribute"/> and reflection to
    /// find the attribute, ensuring a single source of truth between OpenAPI documentation and
    /// runtime enforcement. Fails closed if no attribute is found.
    /// </summary>
    /// <param name="auth">The authorization service.</param>
    /// <param name="resource">The resource to check authorization against.</param>
    /// <param name="controller">The controller instance, pass <c>this</c>.</param>
    /// <param name="methodName">Automatically populated by the compiler.</param>
    public static async Task AssertAbacPolicyAsync(this IAuthorizationService auth, object resource, ControllerBase controller, [CallerMemberName] string methodName = "")
    {
        var policyName = AbacPolicyCache.GetOrAdd(
            (controller.GetType(), methodName),
            key => key.ControllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m.Name == key.MethodName)
                .SelectMany(m => m.GetCustomAttributes<AbacPolicyAttribute>())
                .Select(a => a.AbacPolicy)
                .FirstOrDefault()
        )
            ?? throw new InvalidOperationException(
                $"No [AbacPolicy] attribute found on {controller.GetType().Name}.{methodName}. " +
                $"AssertAbacPolicyAsync requires the action to declare an [AbacPolicy].");
        await AssertAuthorizedAsync(auth, resource, policyName);
    }

    private static readonly ConcurrentDictionary<(Type ControllerType, string MethodName), string?> AbacPolicyCache = new();

    /// <summary>
    /// A placeholder principal that is used to defer the actual user principal until the
    /// processing of the handler.
    /// </summary>
    internal static ClaimsPrincipal DeferredPrincipal = new();
}
