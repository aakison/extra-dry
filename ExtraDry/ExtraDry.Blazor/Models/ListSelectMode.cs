namespace ExtraDry.Blazor;

/// <summary>
/// For a dry table, determines the action on a row selection.
/// </summary>
public enum ListSelectMode {

    /// <summary>
    /// The list rows are not selectable and there is nothing that can be done with any of them.
    /// </summary>
    None,

    /// <summary>
    /// There is only a single command available and it is used automatically on any row click.
    /// </summary>
    Action,

    /// <summary>
    /// One or more commands exist that work on a single row, only a single row may be selected.
    /// </summary>
    Single,

    /// <summary>
    /// One or more commands exist that support multiple items at once, multiple rows may be selected.
    /// </summary>
    Multiple,

}
