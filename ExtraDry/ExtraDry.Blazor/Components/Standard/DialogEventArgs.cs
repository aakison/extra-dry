namespace ExtraDry.Blazor;

/// <summary>
/// Event args for Ok and Cancel events from a dialog.
/// </summary>
public class DialogEventArgs : EventArgs
{

    /// <summary>
    /// If set in an event handler, cancels the button action from applying.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// If a mouse event started processing, the underlying mouse event that triggered the event.
    /// </summary>
    public MouseEventArgs? MouseEventArgs { get; init; }

}
