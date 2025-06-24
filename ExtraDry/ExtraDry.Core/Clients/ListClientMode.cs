namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListClientMode
{
    FullCollection,

    Filter,

    FilterAndSort,

    FilterSortAndPage,
}
