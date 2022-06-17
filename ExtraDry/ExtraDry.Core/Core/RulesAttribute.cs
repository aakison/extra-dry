namespace ExtraDry.Core;

/// <summary>
/// Declare on properties to instruct the `RuleEngine` how to address the creating, updating and deleting of the property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RulesAttribute : Attribute {

    public RulesAttribute()
    {

    }

    public RulesAttribute(RuleAction defaultRule/*, object? deleteValue*/)
    {
        UpdateAction = defaultRule;
        CreateAction = defaultRule;
        //DeleteValue = deleteValue;
        //HasDeleteValue = true;
    }

    //public RulesAttribute(RuleAction defaultRule, object? deleteValue, object? undeleteValue)
    //{
    //    UpdateAction = defaultRule;
    //    CreateAction = defaultRule;
    //    DeleteValue = deleteValue;
    //    HasDeleteValue = true;
    //    UndeleteValue = undeleteValue;
    //    HasUndeleteValue = true;
    //}

    public RuleAction CreateAction { get; set; } = RuleAction.Allow;

    public RuleAction UpdateAction { get; set; } = RuleAction.Allow;

    /// <summary>
    /// If set, provides a value that is applied to this property during a deletion.
    /// The `RuleEngine.Delete` method will return `true` if any properties have a DeleteValue.
    /// This should be interpreted as a soft-delete.
    /// </summary>
    public object? DeleteValue { get; set; }

    //public bool HasDeleteValue { get; }

    //public object? UndeleteValue { get; }

    //public bool HasUndeleteValue { get; }
}

// Any DeleteValue implies Soft-Delete enabled.
// When soft-delete enabled:
//   Calls to rules.Delete(entity) will set deleted value.
//   Calls to rules.Create(entity), rules.Update(entity) will block this value from this property.
//   If no value is set rules.Delete does nothing (other values might work or lambda might do something).

// Any UndeleteValue implies Soft-Undelete enabled.
//   Calls to rules.Undelete(entity) will set undeleted value.
//   Calls to rules.Create, rules.Update ignore this value always.
//   USAGE PREVENTS THIS: If no DeleteValue set, exception is thrown on rules.Undelete
//   If value is the same as DeleteValue, exception is thrown on rules.Delete or rules.Undelete
//   If no UndeleteValue, rules.Undelete does nothing (other values might work or lambda might do something).

// If multiple properties have DeleteValues declared...
//   ??? Analysis rule
//   ??? Framework exception
//   ??? Treat as a single value for above rules
//   ??? Treat as a combination of values for the above rules


