namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SoftDeleteRuleAttribute : Attribute {

    public SoftDeleteRuleAttribute(string propertyName, object? deleteValue)
    {
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        CanUndelete = false;
    }

    public SoftDeleteRuleAttribute(string propertyName, object? deleteValue, object? undeleteValue)
    {
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        UndeleteValue = undeleteValue;
        CanUndelete = true;
    }

    public string PropertyName { get; }

    public object? DeleteValue { get; }

    public object? UndeleteValue { get; }

    public bool CanUndelete { get; }
}
