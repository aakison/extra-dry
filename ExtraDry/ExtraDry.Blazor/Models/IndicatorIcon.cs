namespace ExtraDry.Blazor;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IndicatorIcon
{
    Loading = 1,
    Timeout = 2,
    Error = 4,
    All = Loading | Timeout | Error
}
