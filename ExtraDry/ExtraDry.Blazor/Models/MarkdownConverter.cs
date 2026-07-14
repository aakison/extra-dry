using Markdig;
using System.Text.RegularExpressions;

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
        GithubFlavored = false,
        SmartHrefHandling = true,
    });

    // Matches a <figure> element containing an <img>. SunEditor wraps every inserted image in
    // a <figure> with many custom data-* attributes that ReverseMarkdown cannot handle.
    private static readonly Regex FigureImagePattern = new(
        @"<figure[^>]*>.*?<img\b([^>]*)>.*?</figure>",
        RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Used inside the replacement to extract individual attributes from the <img> tag.
    private static readonly Regex SrcPattern = new(
        @"\bsrc=""([^""]*)""",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex AltPattern = new(
        @"\balt=""([^""]*)""",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
        return Regex.Replace(
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
        html = NormalizeFigureImages(html);
        return HtmlToMarkdownConverter.Convert(html).Trim();
    }

    /// <summary>
    /// Replaces SunEditor's verbose &lt;figure&gt;&lt;img data-*...&gt;&lt;/figure&gt; blocks
    /// with a minimal &lt;img src="..." alt="..."&gt; that ReverseMarkdown can convert cleanly
    /// to standard Markdown image syntax.
    /// </summary>
    private static string NormalizeFigureImages(string html)
    {
        return FigureImagePattern.Replace(html, m => {
            var attrs = m.Groups[1].Value;
            var src = SrcPattern.Match(attrs).Groups[1].Value;
            var alt = AltPattern.Match(attrs).Groups[1].Value;
            return $"<img src=\"{src}\" alt=\"{alt}\">";
        });
    }
}
