using Microsoft.AspNetCore.Authorization;
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
    /// A placeholder principal that is used to defer the actual user principal until the
    /// processing of the handler.
    /// </summary>
    internal static ClaimsPrincipal DeferredPrincipal = new();
}
