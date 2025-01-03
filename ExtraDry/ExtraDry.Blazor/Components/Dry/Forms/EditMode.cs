namespace ExtraDry.Blazor;

/// <summary>
/// The abstract edit mode for forms and input controls. The mode is combined with the <see
/// cref="RulesAttribute" /> to determine the behavior of the form and it's elements. For example,
/// a property can be set to block changes on update but not on create, in which case the input
/// element will be read-only when updating, but writable in the form.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EditMode
{
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
