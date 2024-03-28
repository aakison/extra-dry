using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ExtraDry.Server.Security;

/// <summary>
/// Internal class that has all the actual logic for combining a set of AbacOptions, a user, 
/// a route, and a target object; and then determining if the user has access to a resource.
/// Broken into it's own class to allow for easier testing.
/// </summary>
internal class AbacAuthorizationHelper(AbacOptions options) { 

    /// <summary>
    /// Looks up the policies that match the target object and operation, returning true if 
    /// all policies succeed.
    /// </summary>
    internal bool IsAuthorized(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacOperation operation)
    {
        var abacPolicies = GetMatchingPolicies(target, operation);
        if(abacPolicies.Count == 0) {
            //logger.LogError(@"No ABAC policies found for {Operation} on {Target}", operation, typeof(Target).Name);
            return false;
        }
        if(abacPolicies.All(e => IsAuthorized(user, route, target, e))) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Applies the indicated policy to the target object and operation, returning true if 
    /// it succeeds.
    /// </summary>
    internal bool IsAuthorized(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacPolicy policy)
    {
        var conditions = GetPolicyConditions(policy);
        if(conditions.Any(e => SatisfiesCondition(user, route, target, e))) {
            return true;
        }
        return false;
    }

    private List<AbacPolicy> GetMatchingPolicies(object target, AbacOperation operation)
    {
        var type = options.AbacTypeResolver(target);
        var policies = options.Policies.Where(e => 
            (e.Types.Count == 0 || e.Types.Contains(type)) && 
            (e.Operations.Count == 0 || e.Operations.Contains(operation)))
            .ToList();
        return policies;
    }

    private List<AbacCondition> GetPolicyConditions(AbacPolicy policy)
    {
        var conditions = policy.Conditions.Select(e => options.Conditions[e]).ToList();
        return conditions;
    }

    private static bool SatisfiesCondition(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacCondition condition)
    {
        if(condition.AllowAnonymous) {
            return true;
        }
        if(user == null) {
            return false;
        }
        if(condition.Roles.All(user.IsInRole) && 
            condition.Claims.All(e => user.HasClaim(e.Key, Expand(e.Value, user, route, target))) &&
            condition.Attributes.All(e => HasAttribute(target, e.Key, Expand(e.Value, user, route, target)))) {
            return true;
        }
        return false;
    }

    private static string Expand(string value, ClaimsPrincipal? user, RouteValueDictionary? route, object target)
    {
        var expanded = value;
        if(expanded.Contains("{user.") && user != null) {
            foreach(var claim in user.Claims) {
                expanded = expanded.Replace($"{{user.{claim.Type}}}", claim.Value);
            }
            expanded = expanded.Replace(@"{user.sub}", user.Claims.FirstOrDefault(e => e.Type == SoapSubClaimType)?.Value ?? "");
            expanded = expanded.Replace(@"{user.unique_name}", user.Claims.FirstOrDefault(e => e.Type == SoapUniqueNameClaimType)?.Value ?? "");
        }
        if(expanded.Contains("{route.") && route != null) {
            foreach(var key in route.Keys) {
                expanded = expanded.Replace($"{{route.{key}}}", route[key]?.ToString() ?? "");
            }
        }
        if(expanded.Contains("{target.")) {
            foreach(var property in target.GetType().GetProperties()) {
                expanded = expanded.Replace($"{{target.{property.Name}}}", property.GetValue(target)?.ToString() ?? "");
            }
        }
        if(expanded.Contains("{attribute.") && target is IAttributed attributed) {
            foreach(var attribute in attributed.Attributes) {
                expanded = expanded.Replace($"{{attribute.{attribute.Key}}}", attribute.Value);
            }
        }
        return expanded;
    }

    private const string SoapSubClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    private const string SoapUniqueNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    [SuppressMessage("Performance", "CA1854:Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method", Justification = "False positive.")]
    private static bool HasAttribute(object target, string key, string value)
    {
        if(target is IAttributed attributed) {
            if(attributed.Attributes.ContainsKey(key) && attributed.Attributes[key] == value) {
                return true;
            }
        }
        var property = target.GetType().GetProperty(key);
        if(property == null) {
            return false;
        }
        var propertyValue = property.GetValue(target);
        return propertyValue?.ToString() == value;
    }

}
