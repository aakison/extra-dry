namespace ExtraDry.Blazor;

/// <summary>
/// If the user clicks outside of the dialog and it loses focus, determines what the dialog should
/// do.
/// Default: SaveAndClose
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MiniDialogAction
{
    /// <summary>
    /// Save change to the dialog content and close the dialog box.
    /// </summary>
    SaveAndClose,

    /// <summary>
    /// Save changes to the dialog content and leave it open.
    /// </summary>
    Save,

    /// <summary>
    /// Don't save changes and close the dialog box.
    /// </summary>
    Cancel,

    /// <summary>
    /// Don't save changes and don't close dialog box, disable any UI associated with the action.
    /// </summary>
    Disabled,
}
