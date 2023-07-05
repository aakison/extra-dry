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
    /// The item was recycled/soft-deleted.  The entity had a `DeleteRule` attribute which 
    /// requested a recycle be performed by updating a property.  Lambda expressions for 
    /// `remove` and `commit` actions were _not_ executed.
    /// </summary>
    SoftDeleted,

    /// <summary>
    /// The item was expunged/hard-deleted.  Either a hard delete was requested or the entity had 
    /// a `DeleteRule` attribute which explicitly requested that hard delete be used. Lambda 
    /// expressions for `remove` and `commit` actions were executed.
    /// </summary>
    HardDeleted,

}
