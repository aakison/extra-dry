namespace ExtraDry.Blazor;

public enum EditMode {

    /// <summary>
    /// The form and it's elements should be presented as read-only and not changable.
    /// </summary>
    ReadOnly,

    /// <summary>
    /// The form and it's elements are in a create state allowing editing of more values.
    /// </summary>
    Create,

    /// <summary>
    /// The form and it's elements are in an update state allowing more limited editing of values.
    /// </summary>
    Update,

}
