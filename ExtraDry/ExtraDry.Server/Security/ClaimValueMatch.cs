namespace ExtraDry.Server.Security;

/// <summary>
/// The method to use to match a claim value for <see cref="RbacRouteMatchesClaimRequirement"/> policies.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClaimValueMatch
{
    /// <summary>
    /// The value of the route must exactly match the value of the claim.
    /// </summary>
    Exact,

    /// <summary>
    /// The value of the route must match the first part of the claim value, when separated by "@".
    /// </summary>
    FirstPath,

    /// <summary>
    /// The value of the route must match the last part of the claim value, when separated by "@".
    /// </summary>
    LastPath,
}
