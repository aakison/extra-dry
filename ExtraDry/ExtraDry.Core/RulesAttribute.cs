namespace ExtraDry.Core;

/// <summary>
/// Declaration for the Rule Engine to address the creating and updating of the properties. When
/// used on a class, provides a default for all properties of that class. To remove a aggregated
/// object from the OpenApi specification, the declaration must be on the class.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
public sealed class RulesAttribute : Attribute
{
    /// <summary>
    /// Create a Rule that allows property to change on both create and update.
    /// </summary>
    public RulesAttribute()
    {
    }

    public RulesAttribute(FieldAccess fieldAccess)
    {
        switch(fieldAccess) {
            case FieldAccess.ReadOnly:
                CreateAction = RuleAction.Ignore;
                UpdateAction = RuleAction.Block;
                break;
            case FieldAccess.ReadWrite:
                CreateAction = RuleAction.Allow;
                UpdateAction = RuleAction.Allow;
                break;
            case FieldAccess.WriteOnCreate:
                CreateAction = RuleAction.Allow;
                UpdateAction = RuleAction.Block;
                break;
            case FieldAccess.Computed:
                CreateAction = RuleAction.Ignore;
                UpdateAction = RuleAction.Ignore;
                break;
        }
    }

    /// <summary>
    /// The action that should be take by the Rule Engine during create.
    /// </summary>
    public RuleAction CreateAction { get; set; } = RuleAction.Ignore; // aka ReadOnly

    /// <summary>
    /// The action that should be take by the Rule Engine during update.
    /// </summary>
    public RuleAction UpdateAction { get; set; } = RuleAction.Block; // aka ReadOnly
}
