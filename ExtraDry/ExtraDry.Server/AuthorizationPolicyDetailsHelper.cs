using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ExtraDry.Server.Security;

namespace ExtraDry.Server;

/// <summary>
/// Helper class to extract authorization policy details from an HTTP context for inclusion in
/// problem details responses.
/// </summary>
internal static class AuthorizationPolicyDetailsHelper
{
    /// <summary>
    /// Gets the authorization policy details from the current HTTP context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A formatted string describing the failed authorization policies, or null if no policy information is available.</returns>
    internal static string? GetAuthorizationPolicyDetails(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var authorizeData = endpoint?.Metadata.GetOrderedMetadata<IAuthorizeData>();

        if(authorizeData == null || authorizeData.Count == 0) {
            return null;
        }

        var policies = authorizeData
            .Where(a => !string.IsNullOrEmpty(a.Policy))
            .Select(a => a.Policy!)
            .Distinct()
            .ToList();

        if(policies.Count == 0) {
            return null;
        }

        if(context.RequestServices.GetService(typeof(AbacOptions)) is not AbacOptions abacOptions) {
            return $"Access denied. Required policy: {string.Join(", ", policies)}";
        }

        var policyDetails = new List<string>();
        foreach(var policyName in policies) {
            var policy = abacOptions.Policies.FirstOrDefault(p => p.Name == policyName);
            if(policy != null) {
                policyDetails.Add($"Access to this resource requires additional permissions.  Please contact your administrator and request one of the following roles: {EnglishJoin(policy.Conditions, "or")}.");
            }
            else {
                policyDetails.Add($"Policy '{policyName}' required but not found.  Please refer issue to Cemora support.");
            }
        }

        return policyDetails.Count > 0
            ? $"Access denied. {string.Join(". ", policyDetails)}"
            : null;
    }

    private static string EnglishJoin(List<string> values, string conjunction)
    {
        if(values.Count == 0) {
            return "";
        }
        else if(values.Count == 1) {
            return values[0];
        }
        else if(values.Count == 2) {
            return $"{values[0]} {conjunction} {values[1]}";
        }
        else {
            var allButLast = values.Take(values.Count - 1);
            var last = values[^1];
            return $"{string.Join(", ", allButLast)}, {conjunction} {last}";
        }
    }
}
