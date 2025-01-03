namespace ExtraDry.Core;

/// <summary>
/// Defines the deletion action for an entity. In general, a Recycle rule produces a API that is
/// more consumable for applications that host users. While, if this functionality is not required,
/// an Expunge action creates an API that is simpler to implement. Expunge actions can also be
/// applied to specific entities that might have regulatory requirements for deletion.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeleteAction
{
    /// <summary>
    /// Applies a soft-delete pattern by changing the value of a defined property. Must also
    /// declare the property name and values for both deleted and undeleted states.
    /// </summary>
    Recycle,

    /// <summary>
    /// Applies a hard-delete pattern. If the item cannot be deleted (e.g. for referential
    /// integrity reasons), then an exception will be thrown.
    /// </summary>
    Expunge,

    /// <summary>
    /// Tries to apply a hard-delete pattern. If the item cannot be deleted (e.g. for referential
    /// integrity reasons), then the item will be soft-deleted.
    /// </summary>
    TryExpunge,
}
