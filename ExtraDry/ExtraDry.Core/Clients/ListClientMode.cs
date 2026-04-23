namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListClientMode
{
    FullCollection,

    /// <summary>
    /// The server returns Filtered Collections, items are in Items property.
    /// </summary>
    Filtered,

    /// <summary>
    /// The server returns Paged Collections, items are in Items property.
    /// </summary>
    Paged,
}
