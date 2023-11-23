namespace ExtraDry.Blazor.Internal;

/// <summary>
/// The source of the list of items for the ListService to retrieve as lists can be retrieved from
/// a simple list of items or from a hierarchy of items.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListSource
{
    /// <summary>
    /// Retrieve the list of items from a simple list of items.
    /// </summary>
    List,

    /// <summary>
    /// Retrieve the list of items from a hierarchy of items.
    /// </summary>
    Hierarchy,
}
