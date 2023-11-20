namespace ExtraDry.Blazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ListServiceMode
{
    FullCollection,
    Filter,
    FilterAndSort,
    FilterSortAndPage,
}
