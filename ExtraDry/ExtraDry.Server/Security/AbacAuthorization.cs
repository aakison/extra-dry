using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ExtraDry.Server.Security;

public class AbacAuthorization(
    AbacOptions options,
    IHttpContextAccessor contextAccessor,
    ILogger<AbacAuthorization> logger)
{

    public bool IsAuthorized(object target, AbacOperation operation)
    {
        var user = contextAccessor.HttpContext?.User;
        var route = contextAccessor.HttpContext?.Request.RouteValues;
        return IsAuthorized(user, route, target, operation);
    }

    public void AssertAuthorized(object target, AbacOperation operation)
    {
        if(IsAuthorized(target, operation) == false) {
            throw new UnauthorizedAccessException();
        }
    }

    internal bool IsAuthorized(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacOperation operation)
    {
        var abacPolicies = GetMatchingPolicies(target, operation);
        if(abacPolicies.Count == 0) {
            logger.LogError(@"No ABAC policies found for {Operation} on {Target}", operation, typeof(Target).Name);
            return false;
        }
        if(abacPolicies.All(e => SatisfiesPolicy(user, route, target, e))) {
            return true;
        }
        return false;
    }

    internal List<AbacPolicy> GetMatchingPolicies(object target, AbacOperation operation)
    {
        var type = options.AbacTypeResolver(target);
        var policies = options.Policies.Where(e => 
            (e.Types.Count == 0 || e.Types.Contains(type)) && 
            (e.Operations.Count == 0 || e.Operations.Contains(operation)))
            .ToList();
        return policies;
    }

    internal List<AbacCondition> GetPolicyConditions(AbacPolicy policy)
    {
        var conditions = policy.Conditions.Select(e => options.Conditions[e]).ToList();
        return conditions;
    }

    internal static bool SatisfiesCondition(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacCondition condition)
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

    internal static string Expand(string value, ClaimsPrincipal? user, RouteValueDictionary? route, object target)
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

    internal bool SatisfiesPolicy(ClaimsPrincipal? user, RouteValueDictionary? route, object target, AbacPolicy policy)
    {
        var conditions = GetPolicyConditions(policy);
        if(conditions.Any(e => SatisfiesCondition(user, route, target, e))) {
            return true;
        }
        return false;
    }

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
