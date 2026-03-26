namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkdownSupportType
{
    /// <summary>
    /// Markdown is not supported. Any markdown syntax will be treated as plain text and rendered verbatim.
    /// </summary>
    None,

    /// <summary>
    /// Character markdown is supported. Only inline markdown syntax (e.g. emphasis, links) will be processed and rendered.
    /// </summary>
    Character,

    /// <summary>
    /// All markdown is supported. Both inline and block-level markdown syntax (e.g. paragraphs, lists, headings) will be processed and rendered.
    /// </summary>
    Block,


    CommonMark,
}
