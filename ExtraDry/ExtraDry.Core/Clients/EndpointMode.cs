namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum EndpointMode
{
    Append,
    Replace,
    Generate,
}

