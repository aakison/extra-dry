namespace ExtraDry.Blazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandCollapse
{
    Never,

    Always,

    IconThenEllipses,

    StraightToEllipses,
}
