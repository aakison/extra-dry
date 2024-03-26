namespace ExtraDry.Server.Security;

/// <summary>
/// Represents an ABAC policy that can be used to authorize access to resources.  All properties
/// are 'disjunctive' meaning that any one of the properties may be met for the policy to be 
/// satisfied.  The policy will apply to ANY of the types, for ANY of the operations, and if ANY
/// of the conditions are met, then the policy succeeds.
/// </summary>
public class AbacPolicy
{
    /// <summary>
    /// An optional name for the policy.  If provided, can be used to automatically register the
    /// policy with the dotnet authorization system.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The set of types that this policy applies to when assessing ABAC authorization of 
    /// resources.  If empty, the policy applies to all types.
    /// </summary>
    public List<string> Types { get; set; } = [];

    public List<AbacOperation> Operations { get; set; } = [];

    public List<string> Conditions { get; set; } = [];

}
