namespace ExtraDry.Core;

/// <summary>
/// When a `RuleEngine` Undelete method is called, indicates the result of the action.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UndeleteResult {

    /// <summary>
    /// The item was successfully undeleted.
    /// </summary>
    Undeleted,

    /// <summary>
    /// The item was not able to be undeleted.  This could be because the item was not
    /// in a deleted state or that it did not have an Undelete value.
    /// </summary>
    NotUndeleted,

}
