namespace ExtraDry.Core;

/// <summary>
/// Declaration for the Rule Engine to address the creating and updating of the properties.
/// When used on a class, provides a default for all properties of that class.  To remove
/// a aggregated object from the OpenApi specification, the declaration must be on the class.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
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
