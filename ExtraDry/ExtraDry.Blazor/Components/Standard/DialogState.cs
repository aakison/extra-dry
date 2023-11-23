namespace ExtraDry.Blazor;

/// <summary>
/// The states that cycle through as the dialog is moved through states with Show(), Hide(), and Toggle().
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DialogState
{
    /// <summary>
    /// The dialog has not been shown, or has been hidden and then unloaded.
    /// </summary>
    NotLoaded,

    /// <summary>
    /// The dialog is in the page but the child content has not been loaded or rendered.
    /// </summary>
    Hidden,

    /// <summary>
    /// The mini-dialog has been loaded and has a showing state, typically for animation of dialog.
    /// </summary>
    Showing,

    /// <summary>
    /// The dialog has been loaded and is visible to users.
    /// </summary>
    Visible,

    /// <summary>
    /// The dialog continues to be loaded but has a limited hiding state just before unloading, typically for animation of dialog.
    /// </summary>
    Hiding,

}
