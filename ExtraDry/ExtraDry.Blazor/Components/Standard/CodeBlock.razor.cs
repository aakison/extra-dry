#nullable enable

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

public partial class CodeBlock : ComponentBase {

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Lang { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Normalize { get; set; } = true;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Id = $"extraDryCodeBlock{++instanceCount}";
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine($"Parameters set on {Id}");
        if(Normalize) {
            OldBody = Body;
            RenderChildContentToBody();
            var lines = Body.Split('\n').ToList();
            FormatLines(lines);
            Body = string.Join("\n", lines);
            Body = $"<pre><code>{Body}</code></pre>";
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(OldBody != Body) {
            await Module.InvokeVoidAsync("CodeBlock_AfterRender", Id);
        }
    }

    protected string Id { get; set; } = string.Empty;

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private void FormatLines(List<string> lines)
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
        var globalIndent = indentLines.Min(e => LeadingSpaces(e));
        for(int i = 0; i < lines.Count; ++i) {
            var line = lines[i];
            if(line.Length > globalIndent && char.IsWhiteSpace(line[0])) {
                lines[i] = line[globalIndent..];
            }
        }

        static int LeadingSpaces(string s) => s.TakeWhile(e => char.IsWhiteSpace(e)).Count();
    }

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
        //foreach(var frame in frames.Array) {
        //    Console.WriteLine(frame.FrameType);
        //    if(frame.FrameType == RenderTreeFrameType.Text || frame.FrameType == RenderTreeFrameType.Markup) {
        //        Console.WriteLine(frame.TextContent);
        //    }
        //}
    }

    private string OldBody { get; set; } = string.Empty;

    private string Body { get; set; } = string.Empty;

    private MarkupString MarkupBody => (MarkupString)Body;

    private static int instanceCount = 0;

}
