using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtraDry.Server;

/// <summary>
/// Provides extension methods for mapping Extra Dry endpoints into the application's routing
/// pipeline.
/// </summary>
public static partial class ExtraDryEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps an anonymous endpoint at <c>/bundles/atlas.svg</c> that serves a generated SVG sprite
    /// atlas. The <paramref name="iconProvider"/> resolves icons from DI so they don't need to be
    /// available at configuration time. Only icons with <see cref="SvgRenderType.Atlas"/> are
    /// included. Each qualifying SVG file is transformed into a named <c>&lt;symbol&gt;</c>
    /// element following the same rules as the Blazor <c>Theme</c> component. The atlas is
    /// generated once at startup.
    /// </summary>
    public static IEndpointRouteBuilder MapSvgAtlas(this IEndpointRouteBuilder endpoints, IEnumerable<IconInfo> icons, string? debugOutputPath = null)
    {
        var services = endpoints.ServiceProvider;
        var env = services.GetRequiredService<IWebHostEnvironment>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("SvgAtlas");

        var documentIcons = icons
            .Where(i => i.SvgRenderType == SvgRenderType.Atlas
                && i.ImagePath != null
                && i.ImagePath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            .ToList();
        var fallbackIcons = IconInfo.FallbackIcons.Values.Where(e => !documentIcons.Any(i => i.Key == e.Key));
        documentIcons = documentIcons.Concat(fallbackIcons).ToList();

        var symbols = new StringBuilder();
        var allDefs = new StringBuilder();

        foreach(var icon in documentIcons) {
            var fileInfo = env.WebRootFileProvider.GetFileInfo(icon.ImagePath!);
            if(!fileInfo.Exists) {
                logger.LogError("Unable to load IconInfo with key '{Key}': file '{ImagePath}' not found.", icon.Key, icon.ImagePath);
                continue;
            }
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            // Strip anything before the <svg> tag (XML declarations, comments, etc.)
            var svgStart = content.IndexOf("<svg", StringComparison.OrdinalIgnoreCase);
            if(svgStart < 0) {
                logger.LogError("Unable to load IconInfo with key '{Key}': file '{ImagePath}' does not contain an <svg> tag.", icon.Key, icon.ImagePath);
                continue;
            }
            content = content[svgStart..];

            // Same transformation rules as Theme.razor.cs
            var svgTag = SvgTagRegex().Match(content).Value;
            var viewBox = ViewBoxRegex().Match(svgTag).Value;
            var svgBody = SvgTagRegex().Replace(content, "").Replace("</svg>", "");

            // Strip editor metadata, comments, namespaced elements, and namespaced attributes.
            // Order matters: metadata and comments first (they nest namespaced content), then
            // remaining namespaced elements/closing tags, then namespaced attributes.
            svgBody = MetadataElementRegex().Replace(svgBody, "");
            svgBody = CommentRegex().Replace(svgBody, "");
            svgBody = NamespacedElementRegex().Replace(svgBody, "");
            svgBody = NamespacedAttributeRegex().Replace(svgBody, "");
            svgBody = EmptyDefsRegex().Replace(svgBody, "");
            svgBody = BlankLinesRegex().Replace(svgBody, "\n");

            var symbol = $@"<symbol id=""{icon.Key}"" {viewBox}>{svgBody}</symbol>";
            var defs = DefsRegex().Match(svgBody).Value;
            if(!string.IsNullOrWhiteSpace(defs)) {
                symbol = symbol.Replace(defs, "");
                allDefs.AppendLine(defs);
            }
            symbols.AppendLine(symbol);
            logger.LogInformation("Added icon '{Key}' to SVG atlas from file '{ImagePath}'.", icon.Key, icon.ImagePath);
        }

        var atlas = $@"<svg xmlns=""http://www.w3.org/2000/svg"">{allDefs}{symbols}</svg>";

        endpoints.MapGet("/bundles/atlas.svg", () => Results.Content(atlas, "image/svg+xml"))
            .AllowAnonymous()
            .ExcludeFromDescription();

        return endpoints;
    }

    [GeneratedRegex(@"<svg[^>]*>")]
    private static partial Regex SvgTagRegex();

    [GeneratedRegex(@"viewBox=""[\d\s.]*""")]
    private static partial Regex ViewBoxRegex();

    [GeneratedRegex(@"<defs.*</defs>", RegexOptions.Singleline)]
    private static partial Regex DefsRegex();

    [GeneratedRegex(@"<metadata\b[^>]*>.*?</metadata\s*>", RegexOptions.Singleline)]
    private static partial Regex MetadataElementRegex();

    [GeneratedRegex(@"<!--.*?-->", RegexOptions.Singleline)]
    private static partial Regex CommentRegex();

    [GeneratedRegex(@"<\w+:\w+\b[^>]*/>|<\w+:\w+\b[^>]*>.*?</\w+:\w+\s*>|</\w+:\w+\s*>", RegexOptions.Singleline)]
    private static partial Regex NamespacedElementRegex();

    [GeneratedRegex(@"\s+[\w-]+:[\w-]+=""[^""]*""")]
    private static partial Regex NamespacedAttributeRegex();

    [GeneratedRegex(@"<defs\b[^>]*/>", RegexOptions.Singleline)]
    private static partial Regex EmptyDefsRegex();

    [GeneratedRegex(@"\n\s*\n")]
    private static partial Regex BlankLinesRegex();
}
