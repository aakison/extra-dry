using System.Text.RegularExpressions;

namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public partial class MarkdownAttribute(MarkdownSupportType supportType) : ValidationAttribute
{
    public MarkdownSupportType SupportType { get; } = supportType;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string text || string.IsNullOrWhiteSpace(text)) {
            return ValidationResult.Success;
        }

        if (SupportType == MarkdownSupportType.CommonMark) {
            return ValidationResult.Success;
        }

        var stripped = StripCodeBlocks(text);

        if (ContainsHtmlTags(stripped)) {
            return new ValidationResult(
                "Markdown content must not contain HTML tags. Use Markdown syntax instead.",
                [validationContext.MemberName!]);
        }

        return ValidationResult.Success;
    }

    private static string StripCodeBlocks(string text)
    {
        // Remove fenced code blocks (``` ... ```)
        var result = FencedCodeBlockRegex().Replace(text, string.Empty);

        // Remove inline code (` ... `)
        result = InlineCodeRegex().Replace(result, string.Empty);

        return result;
    }

    private static bool ContainsHtmlTags(string text)
    {
        if (HtmlCommentRegex().IsMatch(text)) {
            return true;
        }

        if (HtmlTagRegex().IsMatch(text)) {
            return true;
        }

        return false;
    }

    [GeneratedRegex(@"```[\s\S]*?```")]
    private static partial Regex FencedCodeBlockRegex();

    [GeneratedRegex(@"`[^`]+`")]
    private static partial Regex InlineCodeRegex();

    [GeneratedRegex(@"<!--[\s\S]*?-->")]
    private static partial Regex HtmlCommentRegex();

    [GeneratedRegex(@"<\/?[a-zA-Z][a-zA-Z0-9]*(\s[^>]*)?\s*\/?>")]
    private static partial Regex HtmlTagRegex();
}
