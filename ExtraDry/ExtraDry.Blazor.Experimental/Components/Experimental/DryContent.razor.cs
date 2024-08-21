namespace ExtraDry.Blazor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", 
    Justification = "Decide fate of component")]
public partial class DryContent : ComponentBase {

    [Parameter]
    public ContentLayout? Content { get; set; }

    [Parameter]
    public string? ContentName { get; set; }

    [Inject]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "DRY1503:JavaScript runtime injection should be replaced with module aware ExtraDryJavacriptModule.", Justification = "Entire component needs to be refactored or temporarily retired.")]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;
    private static IServiceProvider StaticServiceProvider { get; set; } = null!;

    [Inject]
    private ILogger<DryContent> Logger { get; set; } = null!;

    private static ILogger<DryContent> StaticLogger { get; set; } = null!;

    protected override void OnInitialized()
    {
        StaticServiceProvider = ServiceProvider;
        StaticLogger = Logger;
    }

    [Command(Category = "Section", Name = "Add New")]
    public void AddSection()
    {
        if(Content == null) {
            return;
        }
        Content.Sections.Add(new ContentSection { 
            Containers = { 
                new ContentContainer { 
                    Html = "<div>New Section</div>",
                    Padding = ContentPadding.Single,
                }
            } 
        });
        StateHasChanged();
    }

    [Command(Category = "Section", Name = "Remove Current")]
    public void RemoveSection()
    {
        if(Content == null) {
            return;
        }
        throw new NotImplementedException();
    }

    [Command(Category = "Selection", Icon = "bold", Collapse = CommandCollapse.Always)]
    public async Task ToggleBold()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleBold");
    }

    [Command(Category = "Selection")]
    public async Task ToggleItalic()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleItalic");
    }

    [Command(Category = "Selection", Name = "H1")]
    public async Task ToggleHeader1()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 1);
    }

    [Command(Category = "Selection", Name = "H2")]
    public async Task ToggleHeader2()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 2);
    }

    [Command(Name = "H3")]
    public async Task ToggleHeader3()
    {
        await JSRuntime!.InvokeVoidAsync("roosterToggleHeader", 3);
    }

    [Command(Name = "H4")]
    public async Task ToggleHeader4()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 4);
    }

    [Command(Name = "H5")]
    public async Task ToggleHeader5()
    {
        await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 5);
    }

    [Command(Name = "H6")]
    public async Task ToggleHeader6()
    {
        await JSRuntime!.InvokeVoidAsync("roosterToggleHeader", 6);
    }

    [Command(Name = "HR")]
    public async Task HorizontalRule()
    {
        await JSRuntime.InvokeVoidAsync("roosterHorizontalRule");
    }

    [Command(Icon = "Eraser", Collapse = CommandCollapse.Always)]
    public async Task ClearFormat()
    {
        await JSRuntime.InvokeVoidAsync("roosterClearFormat");
    }

    [Command]
    public async Task AddHyperlink()
    {
        await JSRuntime.InvokeVoidAsync("roosterInsertHyperlink", HyperlinkClass, HyperlinkHref, HyperlinkTitle);
    }

    public ContentSection? CurrentSection { get; set; }

    public ContentContainer? CurrentContainer { get; set; }

    [Control(ControlType.RadioButtons)]
    [Display(Name = "Theme")]
    public ContentTheme CurrentSectionTheme {
        get => CurrentSection?.Theme ?? ContentTheme.Light;
        set {
            if(CurrentSection != null) {
                CurrentSection.Theme = value;
                StateHasChanged();
            }
        }
    }

    [Control(ControlType.RadioButtons, Icon = "_content/ExtraDry.Blazor/img/layout-{0}.png")]
    [Display(Name = "Layout")]
    public SectionLayout CurrentSectionLayout {
        get => CurrentSection?.Layout ?? SectionLayout.Single;
        set {
            if(CurrentSection != null) {
                CurrentSection.Layout = value;
                StateHasChanged();
            }
        }
    }

    [Control(ControlType.RadioButtons, CaptionTemplate = "", Icon = "_content/ExtraDry.Blazor/img/alignment-{0}.png")]
    [Display(Name = "Alignment")]
    public ContentAlignment CurrentContainerAlignment {
        get => CurrentContainer?.Alignment ?? ContentAlignment.TopLeft;
        set {
            if(CurrentContainer != null) {
                CurrentContainer.Alignment = value;
                StateHasChanged();
            }
        }
    }

    [Control(ControlType.RadioButtons, Icon = "_content/ExtraDry.Blazor/img/padding-{0}.png")]
    [Display(Name = "Padding")]
    public ContentPadding CurrentContainerPadding {
        get => CurrentContainer?.Padding ?? ContentPadding.None;
        set {
            if(CurrentContainer != null) {
                CurrentContainer.Padding = value;
                StateHasChanged();
            }
        }
    }

    [JSInvokable("UploadImage")]
    public static async Task UploadImage(string imageDataUrl)
    {
        if(!imageDataUrl.StartsWith("data:", StringComparison.Ordinal)) {
            throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This URL must begin with 'data:' scheme.", "Unable to upload image. 0x0F4B39DA");
        }
        var semicolon = imageDataUrl.IndexOf(';');
        if(semicolon > 64 || semicolon < 7) {
            throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This must include the mime type between the first ':' and the first ';'", "Unable to upload image. 0x0F8A8B8C");
        }
        var base64Delimiter = imageDataUrl.IndexOf("base64,", StringComparison.Ordinal);
        if(base64Delimiter < 0) {
            throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This must include the content of the image properly base64 encoded.", "Unable to upload image. 0x0F3CEE65");
        }
        //var mimeType = imageDataUrl[6..semicolon]; Save for later, should add to blob information.
        //var base64 = imageDataUrl[(base64Delimiter+7)..];
        //var bytes = Convert.FromBase64String(base64);
        //if(StaticServiceProvider.GetService(typeof(IBlobService)) is not IBlobService blobService) {
        //    StaticLogger.LogWarning("No IBlobService was registered with the service locator, the pasted image will encoded inside the content of the page.  This becomes problematic for large or multiple images and images should be stored in blob storage.  Create an implementation of IBlobService and register with the IServiceCollection.");
        //    return new BlobInfo() {
        //        //UniqueId = Guid.Empty,
        //        Url = imageDataUrl,
        //    };
        //}
        //var blob = await blobService.CreateAsync(bytes);
        //return blob;
        await Task.Delay(1);
        throw new NotImplementedException("Change implementation to use BlobService.");
    }

    public string HyperlinkClass { get; set; } = string.Empty;

    public string HyperlinkTitle { get; set; } = string.Empty;

    public string HyperlinkHref { get; set; } = string.Empty;

    private void EditorFocus(ContentSection section, ContentContainer container)
    {
        CurrentSection = section;
        CurrentContainer = container;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(Content == null) {
            return;
        }
        foreach(var section in Content.Sections) {
            foreach(var container in section.DisplayContainers) {
                if(!roosterIsCanonical.Contains(container.Id)) {
                    await JSRuntime.InvokeVoidAsync("startEditing", container.Id);
                    await JSRuntime.InvokeVoidAsync("roosterSetContent", container.Id, container.Html);
                    roosterIsCanonical.Add(container.Id);
                }
            }
        }
    }
    private readonly List<Guid> roosterIsCanonical = [];

    private async Task EditorFocusOut(ContentContainer container)
    {
        var value = await JSRuntime.InvokeAsync<string>("roosterGetContent", container.Id);
        container.Html = value;
    }

}
