namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkdownSupportType
{
    None,
    Character,
    Block,
}
