using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Models;

/// <summary>
/// The arguments that are sent to a <see cref="CommandInfo"/> specified command.  The plurality 
/// of the arguments determines the contexts in which the command is available.  E.g. a command 
/// that takes a single model as a parameter cannot be used when multiple models are selected.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Not a reference to a built-in type.")]
public enum CommandArguments {

    /// <summary>
    /// This command takes no arguments, typical of global command like 'Add Entity'
    /// </summary>
    None,

    /// <summary>
    /// This command takes a single argument and can only work on one at a time.  Typical of UI 
    /// changing commands like 'Open Entity'.
    /// </summary>
    Single,

    /// <summary>
    /// This command takes multiple arguments and will execute across all of them in bulk.  Use 
    /// whenever possible to enable bulk actions on lists, such as 'Close Entities'.
    /// </summary>
    Multiple,

}
