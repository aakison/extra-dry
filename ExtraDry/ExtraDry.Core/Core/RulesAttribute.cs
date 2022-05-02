namespace ExtraDry.Core;

/// <summary>
/// Declare on properties to instruct the `RuleEngine` how to address the creating, updating and deleting of the property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RulesAttribute : Attribute {

    public RulesAttribute()
    {

    }

    public RulesAttribute(RuleAction defaultRule = RuleAction.Allow)
    {
        UpdateAction = defaultRule;
        CreateAction = defaultRule;
    }

    public RuleAction CreateAction { get; set; } = RuleAction.Allow;

    public RuleAction UpdateAction { get; set; } = RuleAction.Allow;

    /// <summary>
    /// If set, provides a value that is applied to this property during a deletion.
    /// The `RuleEngine.Delete` method will return `true` if any properties have a DeleteValue.
    /// This should be interpreted as a soft-delete.
    /// </summary>
    public object? DeleteValue { get; set; }

}
