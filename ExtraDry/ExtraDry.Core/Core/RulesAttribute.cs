namespace ExtraDry.Core;

/// <summary>
/// Declare on properties to instruct the `RuleEngine` how to address the creating, updating and deleting of the property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RulesAttribute : Attribute {

    public RulesAttribute()
    {

    }

    public RulesAttribute(RuleAction defaultRule)
    {
        UpdateAction = defaultRule;
        CreateAction = defaultRule;
    }

    public RuleAction CreateAction { get; set; } = RuleAction.Allow;

    public RuleAction UpdateAction { get; set; } = RuleAction.Allow;

}
