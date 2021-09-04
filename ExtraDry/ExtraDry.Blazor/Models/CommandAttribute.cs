#nullable enable

using System;

namespace ExtraDry.Blazor {

    /// <summary>
    /// Indicates that the specified method, typically on a ViewModel, is a valid command.
    /// WARNING: May cause problems with Blazor debugging, see: https://github.com/dotnet/aspnetcore/issues/25380
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute {

        /// <summary>
        /// Flag the method as a command with the specified functional command context.
        /// </summary>
        public CommandAttribute(CommandContext context = CommandContext.Alternate)
        {
            Context = context;
        }

        /// <summary>
        /// The context of the command, typically `Alternate`. 
        /// On forms, one command should be `Primary`.
        /// </summary>
        public CommandContext Context { get; set; } 

        /// <summary>
        /// The name of the command which is displayed on the button.
        /// If not set, the name is inferred from the method's name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The name of an icon to be applied to the command.
        /// This is added as class information to an 'i' tag with the command.
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// An optional category used to create filtered subsets of commands.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// The relative order of this button amongst others.
        /// This order is evaluated after groupings based on `Context` and `Collapse` settings.
        /// </summary>
        /// <remarks>
        /// TODO: Implement explicit order on buttons
        /// </remarks>
        public int Order { get; set; }

        /// <summary>
        /// Indicates the desired collapse behavior for the command when displayed with other commands in a bar
        /// and the screen width is limited.
        /// </summary>
        /// <remarks>
        /// TODO: Implement Collapse on Button Bar
        /// </remarks>
        public CommandCollapse Collapse { get; set; }
    }
}
