namespace ExtraDry.Server.Security;

/// <summary>
/// Collection of options for Attribute-Based Access Control (ABAC) rules. These are typically
/// defined in appsettings.json but could be configured and registered from any source.
/// </summary>
public class AbacOptions : IValidatableObject
{
    /// <summary>
    /// When binding from configuration, this is the section name to use.
    /// </summary>
    public const string SectionName = "Authorization";

    /// <summary>
    /// The conditions that can be used by policies, indexed by name. Allows conditions to be
    /// reused across multiple policies, simplifying the configuration.
    /// </summary>
    public Dictionary<string, AbacCondition> Conditions { get; init; } = [];

    /// <summary>
    /// The policies that define the rules for access control. Policies are matched based on type
    /// and operation and all matching policies must be satisfied for access to be granted.
    /// </summary>
    public List<AbacPolicy> Policies { get; init; } = [];

    /// <summary>
    /// The type resolver function that is used to determine the type of an object for ABAC rules.
    /// By default, the type is matched to the object's class name. For cross-cutting concerns,
    /// such as attributes, the type may be taken from a discriminator property. The function
    /// should be modified to return the appropriate type name for the target object.
    /// </summary>
    public Func<object?, string> AbacTypeResolver { get; set; }
        = target => target == null ? "null" : target.GetType().Name;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // TODO: AbacConditions
        // TODO: AbacPolicies
        var referencedConditions = Policies.SelectMany(p => p.Conditions).Distinct() ?? [];
        var actualConditions = Conditions.Select(e => e.Key);
        foreach(var missing in referencedConditions.Except(actualConditions)) {
            yield return new ValidationResult($"Authorization policies referenced condition '{missing}', which was not found in the list of authorization conditions.");
        }
        foreach(var unused in actualConditions.Except(referencedConditions)) {
            yield return new ValidationResult($"Authorization condition '{unused}' was not referenced by any authorization policy.");
        }
    }
}
