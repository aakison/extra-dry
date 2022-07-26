namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SoftDeleteRuleAttribute : Attribute {

    // TODO: analysis rule that propertyName should be nameof(xxx)
    // TODO: analysis rule that deleteValue != undeleteValue
    // TODO: analysis rule that nameof(xxx) should point to property on class or base class

    public SoftDeleteRuleAttribute(string propertyName, object deleteValue)
    {
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        CanUndelete = false;
    }

    public SoftDeleteRuleAttribute(string propertyName, object deleteValue, object undeleteValue)
    {
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        UndeleteValue = undeleteValue;
        CanUndelete = true;
    }

    public string PropertyName { get; }

    public object DeleteValue { get; }

    public object? UndeleteValue { get; }

    public bool CanUndelete { get; }
}
