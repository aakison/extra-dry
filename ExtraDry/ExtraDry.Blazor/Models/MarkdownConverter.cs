using Markdig;

namespace ExtraDry.Blazor.Models;

/// <summary>
/// Provides bidirectional conversion between Markdown and HTML.
/// This service is stateless and can be used as a singleton.
/// </summary>
public class MarkdownConverter
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    private static readonly ReverseMarkdown.Converter HtmlToMarkdownConverter = new(new ReverseMarkdown.Config {
        GithubFlavored = true,
        SmartHrefHandling = true,
    });

    /// <summary>
    /// Converts Markdown text to HTML.
    /// </summary>
    public static string ToHtml(string markdown)
    {
        if(string.IsNullOrWhiteSpace(markdown)) {
            return string.Empty;
        }
        return Markdown.ToHtml(markdown, Pipeline).Trim();
    }

    /// <summary>
    /// Converts markdown text to ready to render HTML markup. Optionally sanitizes links to
    /// prevent navigation when clicked, which is useful for live preview scenarios.
    /// </summary>
    public static MarkupString ToMarkup(string markdown, bool sanitizeLinks = false)
    {
        var html = ToHtml(markdown);
        if(sanitizeLinks) {
            html = SanitizeLinks(html);
        }
        return new MarkupString(html);
    }

    public static string SanitizeLinks(string html)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            html,
            @"href=""([^""]*)""",
            @"href=""$1"" onclick=""event.preventDefault();""");
    }

    /// <summary>
    /// Converts HTML text to Markdown.
    /// </summary>
    public static string ToMarkdown(string html)
    {
        if(string.IsNullOrWhiteSpace(html)) {
            return string.Empty;
        }
        return HtmlToMarkdownConverter.Convert(html).Trim();
    }
}
