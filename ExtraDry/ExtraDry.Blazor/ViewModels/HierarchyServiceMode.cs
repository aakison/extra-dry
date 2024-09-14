namespace ExtraDry.Blazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HierarchyServiceMode
{
    FullCollection,
    Filter,
    FilterAndPage,
}
