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
        GithubFlavored = true, // True for markdown fences to <pre><code>...
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

    // Matches an <img> whose src is the original of a blob, e.g. /tenant/components/blobs/{uuid}
    // or /tenant/components/blobs/{uuid}/{filename}.
    private static readonly Regex BlobImagePattern = new(
        @"<img\b[^>]*?\bsrc=""([^""]*/blobs/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})(?:/([^""/?]*))?""[^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex VariantTokenPattern = new(
        @"^(400|800|1200|full|thumb)\.avif$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Converts Markdown text to HTML. When <paramref name="responsiveImages"/> is true, blob images
    /// are wrapped in picture elements; editors should pass false so the HTML round trips cleanly.
    /// </summary>
    public static string ToHtml(string markdown, bool responsiveImages = true)
    {
        if(string.IsNullOrWhiteSpace(markdown)) {
            return string.Empty;
        }
        var html = Markdown.ToHtml(markdown, Pipeline).Trim();
        return responsiveImages ? AddBlobPictures(html) : html;
    }

    /// <summary>
    /// Wraps images that reference a blob (a URI ending in <c>/blobs/{uuid}</c>) in a picture element
    /// offering AVIF variants of the blob. The original blob remains the img fallback.
    /// </summary>
    private static string AddBlobPictures(string html)
    {
        return BlobImagePattern.Replace(html, m => {
            if(VariantTokenPattern.IsMatch(m.Groups[2].Value)) {
                return m.Value;
            }
            var src = m.Groups[1].Value;
            var srcset = $"{src}/400.avif 400w, {src}/800.avif 800w, {src}/1200.avif 1200w";
            return $"<picture><source type=\"image/avif\" srcset=\"{srcset}\">{m.Value}</picture>";
        });
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
