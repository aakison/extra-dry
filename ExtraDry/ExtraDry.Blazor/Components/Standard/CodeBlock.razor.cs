using ExtraDry.Highlight;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

/// <summary>
/// Provides pretty formatting of code for displaying blocks of codes.
/// </summary>
public partial class CodeBlock : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// An optional indicator of the language that is being rendered in the code block.
    /// </summary>
    [Parameter]
    public string Lang { get; set; } = string.Empty;

    /// <summary>
    /// The code that is inserted into the code block.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// When set, aligns the code to the left of the block allowing code to be indented
    /// in .razor file without indent showing on rendered page.
    /// </summary>
    [Parameter]
    public bool Normalize { get; set; } = true;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    protected override void OnParametersSet()
    {
        RenderChildContentToBody();
        var lines = Body.Split('\n').ToList();
        if(Normalize) {
            FormatLines(lines);
        }
        Body = string.Join("\n", lines);
        Body = Body.Replace("&lt;", "<");
        Body = Body.Replace("&gt;", ">");
        Body = Body.Replace("&amp;", "&");
        // TODO: Change to CSS for highlighting, but need to rationalize the classes first...
        var highlightedCode = highlighter.Highlight(Lang, Body);
        Body = $"<pre><code data-lang=\"{Lang}\" class=\"{LangClass}\">{highlightedCode}</code></pre>";
    }

    /// <summary>
    /// The code pseudo-syntax highlighter.  Large startup time so use a singleton.
    /// Not really used anywhere else so not forcing CodeBlock consumers to register in DI.
    /// </summary>
    private static readonly Highlighter highlighter = new(new HtmlEngine { UseCss = false });

    protected string LangClass => $"language-{Lang}";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "code-block", CssClass);

    private static void FormatLines(List<string> lines)
    {
        while(lines.Any() && string.IsNullOrWhiteSpace(lines.First())) {
            lines.RemoveAt(0);
        }
        while(lines.Any() && string.IsNullOrWhiteSpace(lines.Last())) {
            lines.RemoveAt(lines.Count - 1);
        }
        if(lines.Count == 0) {
            return;
        }
        if(lines.Count == 1) {
            lines[0] = lines[0].Trim();
            return;
        }
        var indentLines = lines.Skip(1).Where(e => !string.IsNullOrWhiteSpace(e));
        var globalIndent = indentLines.Min(LeadingSpaces);
        for(int i = 0; i < lines.Count; ++i) {
            var line = lines[i];
            if(line.Length > globalIndent && char.IsWhiteSpace(line[0])) {
                lines[i] = line[globalIndent..];
            }
        }

        static int LeadingSpaces(string s) => s.TakeWhile(char.IsWhiteSpace).Count();
    }

    /// <summary>
    /// Allows more than just static text inside the code block.
    /// </summary>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "Alternative is to write JavaScript post-processing step.")]
    private void RenderChildContentToBody()
    {
        if(ChildContent == null) {
            return;
        }
        var builder = new RenderTreeBuilder();
        ChildContent.Invoke(builder);
        var frames = builder.GetFrames();
        Body = string.Join("", frames.Array
            .Where(e => e.FrameType == RenderTreeFrameType.Text || e.FrameType == RenderTreeFrameType.Markup)
            .Select(e => e.TextContent));
    }

    private string Body { get; set; } = string.Empty;

    private MarkupString MarkupBody => (MarkupString)Body;

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

}
