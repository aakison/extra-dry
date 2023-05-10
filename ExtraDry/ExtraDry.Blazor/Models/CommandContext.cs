namespace ExtraDry.Blazor;

/// <summary>
/// The semantic context of the command which assists in determining layout.
/// </summary>
/// <remarks>
/// There should only be one `Primary` or `Default` command, and `Danger` should be used 
/// sparingly, so `Alternate` is explicitly the default.
/// </remarks>
public enum CommandContext {

    /// <summary>
    /// Alternate commands that are not destructive, e.g. 'Cancel'.
    /// </summary>
    Alternate = 0,

    /// <summary>
    /// The primary command, should only be one per ViewModel, e.g. 'Save'.
    /// </summary>
    Primary,

    /// <summary>
    /// The default command which indicates the double-click behavior on tables.  Should only be 
    /// one per ViewModel.  
    /// </summary>
    Default,

    /// <summary>
    /// A command that might have adverse consequences, e.g. 'Delete'
    /// </summary>
    Danger,
}
