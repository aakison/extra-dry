namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Used with `SelectionSetChangedEventArgs` to indicate the type of the change for the event.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SelectionSetChangedType
{
    /// <summary>
    /// All items have been cleared from the selection. No event fired if the selection set was
    /// already empty.
    /// </summary>
    Cleared,

    /// <summary>
    /// One or more items has been added to the selection set. No events fired if only duplicate
    /// items are added.
    /// </summary>
    Added,

    /// <summary>
    /// One or more items have been removed from the selection set. No event fired when removing
    /// only items that aren't currently in selection.
    /// </summary>
    Removed,

    /// <summary>
    /// The selection set has changed with items both removed and added. Only way to trigger is in
    /// a single-select set is being changed from one selection to another.
    /// </summary>
    Changed,

    /// <summary>
    /// The selection set has changed to indicate all items are selected. Does not occur on
    /// single-select selection sets.
    /// </summary>
    SelectAll,
}
