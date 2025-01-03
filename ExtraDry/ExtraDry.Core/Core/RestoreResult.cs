namespace ExtraDry.Core;

/// <summary>
/// When a `RuleEngine` RestoreAsync method is called, indicates the result of the action.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RestoreResult
{
    /// <summary>
    /// The item was successfully restored.
    /// </summary>
    Restored,

    /// <summary>
    /// The item was not able to be restored. This could be because the item was not in a recycled
    /// state or that it did not have an restore value.
    /// </summary>
    NotRestored,
}
