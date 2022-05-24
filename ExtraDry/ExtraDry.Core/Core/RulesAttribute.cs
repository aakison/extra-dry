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
    /// <para>
    /// Consumers must use the <see cref="GetDeleteValue"/> method to retrieve the value, as this property getter will throw
    /// an exception if the value has not been set.
    /// </para>
    /// </summary>
    /// <exception cref="DryException">
    /// If the getter of this property is invoked.
    /// </exception>
    public object? DeleteValue {
        get {
            throw new DryException("Please use HasDeleteValue/GetDeleteValue methods to access this value.");
        }
        set {
            deleteValue = value;
            hasDeleteValue = true;
        }
    }

    /// <summary>
    /// Gets the value of <see cref="DeleteValue"/> if it has been set, or <c>null</c>.
    /// </summary>
    public object? GetDeleteValue()
    {
        return deleteValue;
    }

    /// <summary>
    /// Returns true if the DeleteValue has been set, False otherwise.
    /// </summary>
    /// <returns></returns>
    public bool HasDeleteValue()
    {
        return hasDeleteValue;
    }

    private object? deleteValue;
    private bool hasDeleteValue = false;
}
