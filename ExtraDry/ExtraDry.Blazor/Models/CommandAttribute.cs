﻿namespace ExtraDry.Blazor;

/// <summary>
/// Indicates that the specified method on a Decorator, is a valid command.
/// </summary>
/// <remarks>Flag the method as a command with the specified functional command context.</remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandAttribute(
    CommandContext context = CommandContext.Regular)
    : Attribute
{
    /// <summary>
    /// The context of the command, typically `Regular`. On forms, one command should be
    /// `Primary`.
    /// </summary>
    public CommandContext Context { get; set; } = context;

    /// <summary>
    /// The name of the command which is displayed on the button. If not set, the name is inferred
    /// from the method's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The name of an icon to be applied to the command. This will add an Icon to the button with
    /// the key from this property.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// The name of an icon that indicates the visual affordance of the button. This will add an
    /// icon to the end of the button to indicate the action of the button. For example a downward
    /// pointing chevron for a select list, or a calendar page for a date picker.
    /// </summary>
    public string? Affordance { get; set; }

    /// <summary>
    /// An optional category used to create filtered subsets of commands.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// The title of the command button, which is displayed as a tooltip when hovering over the button.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The relative order of this button amongst others. This order is evaluated after groupings
    /// based on `Context` and `Collapse` settings.
    /// </summary>
    /// <remarks>TODO: Implement explicit order on buttons</remarks>
    public int Order { get; set; }

    /// <summary>
    /// Comma-seperated list of roles required for the command to be displayed.
    /// </summary>
    public string? Roles { get; set; }

    /// <summary>
    /// The display CSS class to attach to the button.
    /// </summary>
    public string? CssClass { get; set; }
}
