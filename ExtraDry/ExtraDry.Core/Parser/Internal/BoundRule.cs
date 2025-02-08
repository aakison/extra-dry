namespace ExtraDry.Core.Parser.Internal;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BoundRule
{
    None,

    Inclusive,

    Exclusive,
}
