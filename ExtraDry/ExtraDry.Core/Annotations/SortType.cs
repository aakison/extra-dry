namespace ExtraDry.Core;

/// <summary>
/// The processing rule to be applied to a property when a <see cref="SortQuery"/> or 
/// <see cref="PageQuery"/> has a <see cref="FilterAttribute"/> provided.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortType
{

    /// <summary>
    /// Property is sortable, the default behavior.  Use to override a property that is not 
    /// sortable by default, e.g. UUID.  Ensure that the property is in the database, if the value
    /// is not persisted than an attempt to sort will result in a runtime exception.
    /// </summary>
    Sortable,

    /// <summary>
    /// Property is not sortable, use to override a property that is sortable by default, 
    /// e.g. string.
    /// </summary>
    NotSortable,

}
