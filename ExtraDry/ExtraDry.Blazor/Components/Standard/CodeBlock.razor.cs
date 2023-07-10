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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Id = $"extraDryCodeBlock{++instanceCount}";
    }

    protected override void OnParametersSet()
    {
        OldBody = Body;
        RenderChildContentToBody();
        var lines = Body.Split('\n').ToList();
        if(Normalize) {
            FormatLines(lines);
        }
        Body = string.Join("\n", lines);
        Body = $"<pre><code id=\"{Id}\" data-lang=\"{Lang}\" class=\"{LangClass}\">{Body}</code></pre>";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(OldBody != Body) {
            await Module.InvokeVoidAsync("CodeBlock_AfterRender", Id);
        }
    }

    protected string Id { get; set; } = string.Empty;

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

    private string OldBody { get; set; } = string.Empty;

    private string Body { get; set; } = string.Empty;

    private MarkupString MarkupBody => (MarkupString)Body;

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private static int instanceCount = 0;
}
