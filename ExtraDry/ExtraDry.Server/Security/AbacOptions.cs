using Microsoft.Extensions.Logging;

namespace ExtraDry.Server.Security;

public class AbacOptions : IValidatableObject
{
    public const string SectionName = "Authorization";

    public Dictionary<string, AbacCondition> Conditions { get; init; } = [];

    public List<AbacPolicy> Policies { get; init; } = [];

    public Func<object?, string> AbacTypeResolver { get; set; }
        = (object? target) => target == null ? "null" : target.GetType().Name;

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
