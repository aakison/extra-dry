namespace Sample.Components.Api.Security;

/// <summary>
/// The security policies used in the application, aligned to RBAC roles.
/// </summary>
internal static class Policies
{
    /// <summary>
    /// The user role policy name, for users with a tenant.
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// The admin role policy name, for admins across all tenants.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// The agent role policy name, for automated agents.
    /// </summary>
    public const string Agent = "Agent";

    /// <summary>
    /// The admin or agent role policy name.
    /// </summary>
    public const string AdminOrAgent = "AdminOrAgent";
}
