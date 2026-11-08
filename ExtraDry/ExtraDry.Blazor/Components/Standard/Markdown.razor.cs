namespace ExtraDry.Blazor.Components.Standard;

public partial class Markdown : ComponentBase
{
    [Parameter]
    public string Value { get; set; } = "";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public MarkdownSupportType MarkdownSupport { get; set; } = MarkdownSupportType.Block;

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Placeholder { get; set; } = "";

    /// <summary>
    /// A dictionary of bookmark entries shown in the link dialog. Keys are display names and
    /// values are URLs/paths. When provided, these appear as suggestions in the link URL field.
    /// </summary>
    [Parameter]
    public Dictionary<string, string>? LinkBookmarks { get; set; }

    /// <summary>
    /// The nearest enclosing
    /// against the nearest scope, instead of being embedded as base-64 in the markdown, and the
    /// gallery shows the images of all enclosing scopes.
    /// </summary>
    [CascadingParameter(Name = "MarkdownImageScope")]
    public MarkdownImageScope? ImageScope { get; set; }

    [Inject]
    private MarkdownEditorInterop EditorInterop { get; set; } = null!;

    [Inject]
    private IServiceProvider Services { get; set; } = null!;

    private IMarkdownImageService? ImageService => Services.GetService(typeof(IMarkdownImageService)) as IMarkdownImageService;

    private bool CanUploadImages => ImageScope != null && ImageService != null;

    /// <summary>
    /// Called from the editor when an image is pasted or chosen, uploads it as an attachment.
    /// </summary>
    [JSInvokable]
    public async Task<MarkdownImageFile?> UploadImage(string filename, string contentType, byte[] content)
    {
        if(!CanUploadImages) {
            return null;
        }
        return await ImageService!.UploadAsync(ImageScope!.Scope, filename, contentType, content);
    }

    private async Task<List<MarkdownImageFile>?> LoadImagesAsync()
    {
        if(!CanUploadImages) {
            return null;
        }
        return await ImageService!.ListAsync(ImageScope!.Chain);
    }

    private string renderedHtml = "";
    private string editorId = "";
    private DotNetObjectReference<Markdown>? dotNetRef;
    private bool initialized;
    private string? lastPushedMarkdown;

    protected override void OnInitialized()
    {
        editorId = $"markdown-editor-{++instanceCount}";
    }

    protected override void OnParametersSet()
    {
        if(ReadOnly) {
            renderedHtml = MarkdownConverter.ToHtml(Value);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender && !ReadOnly) {
            dotNetRef = DotNetObjectReference.Create(this);
            var options = new MarkdownEditorOptions {
                Mode = MarkdownSupport,
                Placeholder = Placeholder,
                LinkBookmarks = LinkBookmarks,
                ImageFiles = await LoadImagesAsync(),
                EnableImage = CanUploadImages,
                EnableImageUpload = CanUploadImages,
            };
            await EditorInterop.InitializeAsync(editorId, dotNetRef, options);
            initialized = true;
            var html = MarkdownConverter.ToHtml(Value);
            await EditorInterop.SetContentAsync(editorId, html);
            lastPushedMarkdown = Value;
        }
        else if(initialized && !ReadOnly && Value != lastPushedMarkdown) {
            var html = MarkdownConverter.ToHtml(Value);
            await EditorInterop.SetContentAsync(editorId, html);
            lastPushedMarkdown = Value;
        }
    }

    [JSInvokable]
    public async Task OnContentChanged(string html)
    {
        var markdown = MarkdownConverter.ToMarkdown(html);
        if(markdown != Value) {
            Value = markdown;
            lastPushedMarkdown = markdown;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string ModeCss => MarkdownSupport == MarkdownSupportType.Character ? "character" : "block";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, ModeCss, CssClass);

    /// <summary>
    /// Toggles the inline toolbar visibility. When shown, the toolbar
    /// automatically hides when the editor loses focus.
    /// </summary>
    public async Task ToggleToolbar()
    {
        Console.WriteLine("ToggleToolbar called");
        if(initialized && !ReadOnly) {
            await EditorInterop.ToggleToolbarAsync(editorId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if(initialized) {
            try {
                await EditorInterop.DestroyAsync(editorId);
            }
            catch(JSDisconnectedException) {
                // Circuit already disconnected, no cleanup needed
            }
        }
        dotNetRef?.Dispose();
        GC.SuppressFinalize(this);
    }

    private static int instanceCount;
}
