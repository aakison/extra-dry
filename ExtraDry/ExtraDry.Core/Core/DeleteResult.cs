namespace ExtraDry.Core;

/// <summary>
/// When a `RuleEngine` Undelete method is called, indicates the result of the action.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeleteResult {

    /// <summary>
    /// The item was not deleted, this is an error condition and is not expected.
    /// </summary>
    NotDeleted,

    /// <summary>
    /// The item was soft-deleted.  The entity had a `SoftDeleteRule` attribute.
    /// Lambda expressions for `remove` and `commit` actions were _not_ executed.
    /// </summary>
    SoftDeleted,

    /// <summary>
    /// The item was hard-deleted.  
    /// Either a hard delete was requested or the entity had no `SoftDeleteRule` attribute.
    /// Lambda expressions for `remove` and `commit` actions were executed.
    /// </summary>
    HardDeleted,

}
