namespace ExtraDry.Core;

/// <summary>
/// Defines the decorated entities behavior when the RuleEngine's Delete, Expunge, or Undelete 
/// methods are used on an instantiated object.  If a action of Expunge is used, the property
/// is not set and the object is removed from the data store.  If no property is specified, the
/// only valid action is Expunge.  If a property is specified, the action can be Expunge or Recycle.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DeleteRuleAttribute : Attribute
{

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction)
    {
        DeleteAction = deleteAction;
        CanUndelete = false;
    }

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction, string propertyName, object? deleteValue)
    {
        DeleteAction = deleteAction;
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        CanUndelete = false;
    }

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction, string propertyName, object? deleteValue, object? undeleteValue)
    {
        DeleteAction = deleteAction;
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        UndeleteValue = undeleteValue;
        CanUndelete = true;
    }

    /// <summary>
    /// The type of delete that is applied to this entity.
    /// </summary>
    public DeleteAction DeleteAction { get; }

    /// <summary>
    /// For a Recycle deletion rule, the name of a property on the entity that is changed to 
    /// indicate a recycle state.
    /// </summary>
    public string PropertyName { get; } = string.Empty;

    /// <summary>
    /// The value of the property (specified by PropertyName) that indicates that the entity is in
    /// a recycled state.
    /// </summary>
    public object? DeleteValue { get; }

    /// <summary>
    /// The value of the property (specified by PropertyName) that is set to return the entity to 
    /// an un-deleted state.
    /// </summary>
    public object? UndeleteValue { get; }

    /// <summary>
    /// The delete rule supports recycling to a soft-delete state and has a valid value 
    /// for undeleting.
    /// </summary>
    public bool CanUndelete { get; }
}
