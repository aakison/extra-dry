namespace ExtraDry.Core;

/// <summary>
/// Defines the decorated entities behavior when the RuleEngine's Delete, Expunge, or Undelete 
/// methods are used on an instantiated object.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DeleteRuleAttribute : Attribute
{

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction)
    {
        // ctor forces user to explicitly declare action, but this constructor only supports Expunge.
        if(deleteAction != DeleteAction.Expunge) throw new ArgumentException($"To use {deleteAction}, please provide a delete and undelete value", nameof(deleteAction));
        DeleteAction = deleteAction;
        CanUndelete = false;
    }

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction, string propertyName, object? deleteValue)
    {
        if(deleteAction == DeleteAction.Expunge) {
            // Warn that values will be ignored?
        }
        DeleteAction = deleteAction;
        PropertyName = propertyName;
        DeleteValue = deleteValue;
        CanUndelete = false;
    }

    /// <inheritdoc cref="DeleteRuleAttribute" />
    public DeleteRuleAttribute(DeleteAction deleteAction, string propertyName, object? deleteValue, object? undeleteValue)
    {
        if(deleteAction == DeleteAction.Expunge) {
            // Warn that values will be ignored?
        }
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

// TODO: Delete this after dependent projects are updated.
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
[Obsolete("Use DeleteRuleAttribute instead.")]
public class SoftDeleteRuleAttribute : DeleteRuleAttribute
{

    public SoftDeleteRuleAttribute(string propertyName, object? deleteValue)
        : base(DeleteAction.Recycle, propertyName, deleteValue)
    {
    }

    public SoftDeleteRuleAttribute(string propertyName, object? deleteValue, object? undeleteValue)
        : base(DeleteAction.Recycle, propertyName, deleteValue, undeleteValue)
    {
    }
}
