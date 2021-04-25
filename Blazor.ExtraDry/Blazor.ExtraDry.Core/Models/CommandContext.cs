namespace Blazor.ExtraDry {

    /// <summary>
    /// The semantic context of the command which assists in determining layout.
    /// </summary>
    /// <remarks>
    /// There should only be one `Primary` command, and `Danger` should be used sparingly, so `Alternate` is explicitly the default.
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
        /// A command that might have adverse consequences, e.g. 'Delete'
        /// </summary>
        Danger,
    }
}
