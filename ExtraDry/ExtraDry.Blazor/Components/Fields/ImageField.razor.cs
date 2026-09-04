using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents an image field that allows users to select an image file from the local file
/// system, or capture a new photo directly using the device's camera (rear or front facing).
/// The selected filename is stored as the string value. Use <see cref="OnFileSelected"/> to
/// access the file content. A small inline preview of the selected image is also displayed.
/// </summary>
public partial class ImageField : FieldBase<string>
{
    /// <summary>
    /// The accepted file types for the file input, as a comma-separated list of MIME types or
    /// file extensions (e.g. <c>"image/*"</c>). Defaults to all image types.
    /// </summary>
    [Parameter]
    public string Accept { get; set; } = "image/*";

    /// <summary>
    /// Called when an image is selected, captured, or cleared. Provides a <see cref="BrowserBlob"/>
    /// populated from the selected <see cref="IBrowserFile"/>, or null if the selection was
    /// cleared. The <see cref="FieldBase{T}.OnChange"/> callback is also invoked with the filename
    /// as the change value.
    /// </summary>
    [Parameter]
    public EventCallback<BrowserBlob?> OnFileSelected { get; set; }

    /// <summary>
    /// Whether a small preview of the selected image is displayed alongside the field.
    /// </summary>
    [Parameter]
    public bool ShowPreview { get; set; } = true;

    /// <summary>
    /// Whether the affordance to capture a photo using the rear-facing camera is displayed, in
    /// addition to the folder affordance. Defaults to <c>null</c>, which auto-detects whether
    /// the current device is a mobile device (where the browser's <c>capture</c> attribute is
    /// honored to open the native camera app). Set explicitly to override the auto-detection,
    /// e.g. if the heuristic is wrong for a particular device or browser.
    /// </summary>
    [Parameter]
    public bool? ShowCameraAffordance { get; set; }

    /// <summary>
    /// The icon key for the affordance that opens the file/folder picker. This icon, along with
    /// <see cref="CameraIcon"/>, must be registered with the enclosing <c>Theme</c> component.
    /// </summary>
    [Parameter]
    public string FolderIcon { get; set; } = "open-folder";

    /// <summary>
    /// The icon key for the affordance that captures a photo using the rear-facing camera.
    /// </summary>
    [Parameter]
    public string CameraIcon { get; set; } = "open-camera";

    /// <summary>
    /// The maximum file size, in bytes, allowed when reading the selected file to generate the
    /// inline preview. Defaults to 10 MB. Files larger than this will still be selected as the
    /// <see cref="FieldBase{T}.Value"/>, but no preview will be shown.
    /// </summary>
    [Parameter]
    public long MaxPreviewFileSize { get; set; } = 10 * 1024 * 1024;

    [Inject]
    private ILogger<ImageField> Logger { get; set; } = null!;

    [Inject]
    private ExtraDryJavascriptModule Javascript { get; set; } = null!;

    protected override void OnInitialized()
    {
        if(Icon == "") {
            Icon = "picture-placeholder";
        }
        if(Placeholder == "") {
            Placeholder = "choose or capture image...";
        }
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if(firstRender) {
            try {
                isMobileDevice = await Javascript.InvokeAsync<bool>("ImageField_IsMobileDevice");
                StateHasChanged();
            }
            catch(Exception ex) {
                Logger.LogWarning(ex, "Failed to detect if the current device is a mobile device.");
            }
        }
    }

    private bool isMobileDevice;

    /// <summary>
    /// Hides the base single-affordance behavior in favor of the dedicated affordances
    /// (folder, camera, selfie) rendered by this component.
    /// </summary>
    protected new bool DisplayAffordance => ShowAffordance && !ReadOnly;

    private bool DisplayCameraAffordance => ShowCameraAffordance ?? isMobileDevice;

    /// <summary>
    /// The id of the input that best matches the current device, used so that clicking anywhere
    /// on the control (not just a specific affordance button) opens the most appropriate picker:
    /// the rear camera on a mobile device, or the plain file picker on a desktop.
    /// </summary>
    private string PrimaryInputId => DisplayCameraAffordance ? CameraInputId : InputId;

    private string CameraInputId => $"{InputId}-camera";

    private async Task HandleControlClickAsync()
    {
        if(!DisplayAffordance) {
            return;
        }
        await Javascript.InvokeVoidAsync("ImageField_ClickInput", PrimaryInputId);
    }

    private bool DisplayPreview => ShowPreview && !string.IsNullOrEmpty(PreviewSrc);

    private string PreviewSrc { get; set; } = PreviewPlaceholderSrc;

    private const string PreviewPlaceholderSrc = "/_content/ExtraDry.Blazor/img/glyphs/camera-on-slide.svg";

    private async Task HandleFileChangeAsync(InputFileChangeEventArgs e)
    {
        var blob = e.FileCount > 0 ? new BrowserBlob(e.File) : null;
        await LoadPreviewAsync(blob);
        await OnFileSelected.InvokeAsync(blob);
        var args = new ChangeEventArgs { Value = blob?.Title ?? string.Empty };
        await NotifyChange(args);
    }

    /// <summary>
    /// Reads the selected file's content once, both to populate <paramref name="blob"/>'s
    /// <see cref="BrowserBlob.Content"/> (so it is ready for a later upload without needing to
    /// re-read the browser file, which is unreliable once the originating <c>InputFile</c> has
    /// re-rendered) and to render an inline preview.
    /// </summary>
    private async Task LoadPreviewAsync(BrowserBlob? blob)
    {
        if(blob == null) {
            PreviewSrc = PreviewPlaceholderSrc;
            return;
        }
        try {
            await blob.ReadContentAsync(MaxPreviewFileSize);
            var base64 = Convert.ToBase64String(blob.Content ?? []);
            PreviewSrc = $"data:{blob.MimeType};base64,{base64}";
        }
        catch(Exception ex) {
            PreviewSrc = "";
            Logger.LogWarning(ex, "Failed to load preview for image file {FileName}.", blob.Title);
        }
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "image", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue => string.IsNullOrWhiteSpace(Value) ? Placeholder : Value;

    private string PlaceholderCssClass => string.IsNullOrWhiteSpace(Value) ? "placeholder" : "";

    private string DisplayValueCssClasses => DataConverter.JoinNonEmpty(" ", "value", PlaceholderCssClass, ReadOnlyCss);
}
