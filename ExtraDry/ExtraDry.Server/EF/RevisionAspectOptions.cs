namespace ExtraDry.Server.EF;

/// <summary>
/// Options for configuring the <see cref="RevisionAspect" />. Use with the <see
/// cref="ServiceCollectionExtensions.AddRevisionAspect(IServiceCollection,
/// Action{RevisionAspectOptions})" /> extension method during startup.
/// </summary>
public class RevisionAspectOptions : AuditAspectOptions
{
    /// <summary>
    /// The list of roles that indicate that a 'user' is a system or an agent and should not be
    /// included in the revision. Defaults to 'system' and 'agent'.
    /// </summary>
    public List<string> ExcludedRoles { get; } = ["system", "agent"];
}
