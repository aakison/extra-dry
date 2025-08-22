using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ExtraDry.Blazor;

/// <summary>
/// Extensions to facilitate working with <see cref="ClaimsPrincipal"/> objects in Blazor.  In Blazor, the claims are 
/// a flat list of claims that are deserialized from the server-side authentication state (not the access token formatted claims).
/// Since this is a flat list, these helpers provide a consistent way to access common claims.
/// </summary>
/// <example>
/// Real world example of the actual claims:
/// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier=7ee95cda-e538-435b-aed9-337d99c208fa; 
/// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name=adrian@cemora.com; 
/// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress=adrian@cemora.com; 
/// AspNet.Identity.SecurityStamp=EB7UZVL4SP5HS7O5YSW2B3MODVSA2OTI; 
/// name=Adrian Akison; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=acme-corp/administrator; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=acme-corp/user-manager; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=acme-corp/code-manager; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=kingscliff-wolves/administrator; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=cemora-admin; 
/// http://schemas.microsoft.com/ws/2008/06/identity/claims/role=acme-corp/webmaster; 
/// amr=pwd
/// </example>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Searches through the common claims for an e-mail address, returning the first one found.
    /// (Prefers fully quaified email claim over just the 'email' claim.)
    /// </summary>
    public static string ClaimEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value
            ?? principal.FindFirst("email")?.Value
            ?? "";
    }

    /// <summary>
    /// The display name of the user. This is not the same as the username, which is the e-mail
    /// address. Don't use ClaimTypes.Name, which ASP.NET identity uses as 'username' which is the
    /// same as e-mail.
    /// </summary>
    public static string ClaimName(this ClaimsPrincipal principal)
    {
        // Don't use ClaimTypes.Name, which ASP.NET identity uses as 'username' which is the same
        // as e-mail.
        return principal.FindFirst("name")?.Value
            ?? "";
    }

    public static bool IsInAnyRole(this ClaimsPrincipal principal, string? roles)
    {
        if(string.IsNullOrWhiteSpace(roles)) {
            // No roles, no restrictions.
            return true;
        }
        return IsInAnyRole(principal, roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public static bool IsInAnyRole(this ClaimsPrincipal principal, params string[] roles)
    {
        if(principal is null) {
            return false;
        }
        if(roles is null || roles.Length == 0) {
            // No roles, no restrictions.
            return true;
        }
        return roles.Any(principal.IsInRole);
    }
}
