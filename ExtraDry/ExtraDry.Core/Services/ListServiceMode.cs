namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListServiceMode
{
    FullCollection,

    Filter,

    FilterAndSort,

    FilterSortAndPage,
}
