namespace ExtraDry.Server.Security;

/// <summary>
/// A 'conjunctive' condition for an Attribute Based Access Control (ABAC) policy. All properties
/// must be met for the condition to be true. To create an 'allow anonymous' condition, leave all
/// properties empty.
/// </summary>
public class AbacCondition
{
    /// <summary>
    /// The set of roles that are required for this condition.
    /// </summary>
    public List<string> Roles { get; init; } = [];

    /// <summary>
    /// The set of claims that are required for this condition.
    /// </summary>
    public Dictionary<string, string> Claims { get; init; } = [];

    /// <summary>
    /// The set of attributes that are required for this condition.
    /// </summary>
    public Dictionary<string, string> Attributes { get; init; } = [];

    /// <summary>
    /// Returns true if the condition allows anonymous access.
    /// </summary>
    public bool AllowAnonymous => Roles.Count == 0 && Claims.Count == 0 && Attributes.Count == 0;

    public override string ToString()
    {
        var roles = string.Join(", ", Roles);
        var claims = string.Join(", ", Claims.Select(e => $"{e.Key} is {e.Value}"));
        var attributes = string.Join(", ", Attributes.Select(e => $"{e.Key} = {e.Value}"));
        return $"ABAC Condition: {roles}; {claims}; {attributes}";
    }
}
