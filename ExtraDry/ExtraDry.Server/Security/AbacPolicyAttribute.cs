using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Server.Security;

/// <summary>
/// Marks an endpoint as requiring authentication, with ABAC authorization enforced in the
/// controller. The policy name is carried as metadata for OpenAPI documentation and is
/// automatically resolved by <see cref="AuthorizationServiceExtensions.AssertAbacPolicyAsync"/>
/// via <see cref="CallerMemberNameAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class AbacPolicyAttribute(string policyName) : Attribute, IAuthorizeData
{
    /// <summary>The ABAC policy name, used for documentation and runtime resolution.</summary>
    public string AbacPolicy { get; } = policyName;

    /// <inheritdoc />
    public string? Policy { get; set; }

    /// <inheritdoc />
    public string? Roles { get; set; }

    /// <inheritdoc />
    public string? AuthenticationSchemes { get; set; }
}
