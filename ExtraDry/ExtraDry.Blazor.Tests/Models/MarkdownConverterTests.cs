using ExtraDry.Blazor.Models;

namespace ExtraDry.Blazor.Tests.Models;

public class MarkdownConverterTests
{
    // ── Edge Cases ───────────────────────────────────────────────────

    [Fact]
    public void ToHtml_EmptyString_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToHtml(string.Empty);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToMarkdown_EmptyString_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToMarkdown(string.Empty);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHtml_Null_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToHtml(null!);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToMarkdown_Null_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToMarkdown(null!);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHtml_WhitespaceOnly_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToHtml("   \t\n  ");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToMarkdown_WhitespaceOnly_ReturnsEmpty()
    {
        var result = MarkdownConverter.ToMarkdown("   \t\n  ");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHtml_SingleCharacter_ProducesHtml()
    {
        var result = MarkdownConverter.ToHtml("x");
        Assert.Contains("x", result);
    }

    // ── Inline Formatting Round-Trip ─────────────────────────────────

    [Theory]
    [InlineData("**bold**")]
    [InlineData("*italic*")]
    [InlineData("~~strikethrough~~")]
    [InlineData("`inline code`")]
    [InlineData("[link text](https://example.com)")]
    public void RoundTrip_InlineFormatting_PreservesHtmlSemantics(string markdown)
    {
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Headings Round-Trip ──────────────────────────────────────────

    [Theory]
    [InlineData("# Heading 1")]
    [InlineData("## Heading 2")]
    [InlineData("### Heading 3")]
    [InlineData("#### Heading 4")]
    [InlineData("##### Heading 5")]
    [InlineData("###### Heading 6")]
    public void RoundTrip_Headings_PreservesHtmlSemantics(string markdown)
    {
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Paragraphs and Line Breaks ───────────────────────────────────

    [Fact]
    public void RoundTrip_Paragraph_PreservesHtmlSemantics()
    {
        var markdown = "This is a paragraph.";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    [Fact]
    public void RoundTrip_MultipleParagraphs_PreservesHtmlSemantics()
    {
        var markdown = "First paragraph.\n\nSecond paragraph.";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Lists Round-Trip ─────────────────────────────────────────────

    [Fact]
    public void RoundTrip_UnorderedList_PreservesHtmlSemantics()
    {
        var markdown = "- Item 1\n- Item 2\n- Item 3";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    [Fact]
    public void RoundTrip_OrderedList_PreservesHtmlSemantics()
    {
        var markdown = "1. First\n2. Second\n3. Third";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    [Fact]
    public void RoundTrip_NestedList_PreservesHtmlSemantics()
    {
        var markdown = "- Item 1\n    - Nested 1\n    - Nested 2\n- Item 2";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Blockquotes Round-Trip ───────────────────────────────────────

    [Fact]
    public void RoundTrip_Blockquote_PreservesHtmlSemantics()
    {
        var markdown = "> This is a blockquote.";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Fenced Code Blocks Round-Trip ────────────────────────────────

    [Fact]
    public void RoundTrip_FencedCodeBlock_PreservesHtmlSemantics()
    {
        var markdown = "```\nvar x = 1;\n```";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    [Fact]
    public void RoundTrip_FencedCodeBlockWithLanguage_PreservesHtmlSemantics()
    {
        var markdown = "```csharp\nvar x = 1;\n```";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Mixed Content Round-Trip ─────────────────────────────────────

    [Fact]
    public void RoundTrip_ParagraphWithInlineFormatting_PreservesHtmlSemantics()
    {
        var markdown = "This has **bold** and *italic* and `code` in it.";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    [Fact]
    public void RoundTrip_MixedContent_PreservesHtmlSemantics()
    {
        var markdown = "# Title\n\nA paragraph with **bold** text.\n\n- Item 1\n- Item 2\n\n> A quote";
        var html = MarkdownConverter.ToHtml(markdown);
        var roundTripped = MarkdownConverter.ToMarkdown(html);
        var htmlAgain = MarkdownConverter.ToHtml(roundTripped);

        Assert.Equal(html, htmlAgain);
    }

    // ── Direct Conversion Tests ──────────────────────────────────────

    [Fact]
    public void ToHtml_Bold_ProducesStrongTag()
    {
        var result = MarkdownConverter.ToHtml("**bold**");
        Assert.Contains("<strong>bold</strong>", result);
    }

    [Fact]
    public void ToHtml_Italic_ProducesEmTag()
    {
        var result = MarkdownConverter.ToHtml("*italic*");
        Assert.Contains("<em>italic</em>", result);
    }

    [Fact]
    public void ToHtml_InlineCode_ProducesCodeTag()
    {
        var result = MarkdownConverter.ToHtml("`code`");
        Assert.Contains("<code>code</code>", result);
    }

    [Fact]
    public void ToHtml_Link_ProducesAnchorTag()
    {
        var result = MarkdownConverter.ToHtml("[text](https://example.com)");
        Assert.Contains("<a href=\"https://example.com\">text</a>", result);
    }

    [Fact]
    public void ToMarkdown_StrongTag_ProducesBold()
    {
        var result = MarkdownConverter.ToMarkdown("<strong>bold</strong>");
        Assert.Contains("**bold**", result);
    }

    [Fact]
    public void ToMarkdown_EmTag_ProducesItalic()
    {
        var result = MarkdownConverter.ToMarkdown("<em>italic</em>");
        Assert.Contains("*italic*", result);
    }
}
