namespace ExtraDry.Blazor;

/// <summary>
/// The semantic context of the command which assists in determining layout.
/// </summary>
/// <remarks>
/// There should only be one `Primary` or `Default` command, and `Danger` should be used sparingly.
/// `Normal` and `Alternate` can be used freely. `Normal` will typically be the main buttons, while
/// `Alternate` will be collapable button-icons.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandContext
{
    /// <summary>
    /// Regular commands that show as normal buttons.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// The primary command, should only be one per ViewModel, e.g. 'Save'.
    /// </summary>
    Primary,

    /// <summary>
    /// The default command which indicates the double-click behavior on tables. Should only be one
    /// per ViewModel.
    /// </summary>
    Default,

    /// <summary>
    /// A command that might have adverse consequences, e.g. 'Delete'
    /// </summary>
    Danger,

    /// <summary>
    /// Alternate commands that are less important and might be button icons or collapsed.
    /// </summary>
    Alternate,
}
