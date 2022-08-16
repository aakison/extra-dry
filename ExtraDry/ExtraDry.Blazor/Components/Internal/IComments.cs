#nullable enable

namespace ExtraDry.Blazor;

/// <summary>
/// An interface not to be used by classes, but provides a target for `inheritdoc` XML comments.
/// Use these comments to ensure consistency across the entire framework.
/// </summary>
internal interface IComments {

    /// <summary>
    /// When an element is not selected, the text to display as a prompt for users.
    /// </summary>
    string Placeholder { get; set; }

}
