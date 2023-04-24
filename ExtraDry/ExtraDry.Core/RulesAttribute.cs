namespace ExtraDry.Core;

/// <summary>
/// Declare on properties to instruct the Rule Engine how to address the creating and updating of the property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RulesAttribute : Attribute {

    /// <summary>
    /// Create a Rule that allows property to change on both create and update.
    /// </summary>
    public RulesAttribute()
    {

    }

    /// <summary>
    /// Create a Rule that has the same action for both create and update.
    /// </summary>
    public RulesAttribute(RuleAction defaultRule)
    {
        UpdateAction = defaultRule;
        CreateAction = defaultRule;
    }

    /// <summary>
    /// The action that should be take by the Rule Engine during create.
    /// </summary>
    public RuleAction CreateAction { get; set; } = RuleAction.Allow;

    /// <summary>
    /// The action that should be take by the Rule Engine during update.
    /// </summary>
    public RuleAction UpdateAction { get; set; } = RuleAction.Allow;

}
