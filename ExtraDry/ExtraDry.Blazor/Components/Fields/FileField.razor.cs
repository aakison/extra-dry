using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a file field that allows users to select a file from the local file system.
/// The selected filename is stored as the string value. Use <see cref="OnFileSelected"/> to
/// access the file content.
/// </summary>
public partial class FileField : FieldBase<BrowserBlob>
{
    /// <summary>
    /// The accepted file types for the file input, as a comma-separated list of MIME types or
    /// file extensions (e.g. <c>"image/*"</c>, <c>".pdf,.docx"</c>). Defaults to all files.
    /// </summary>
    [Parameter]
    public string Accept { get; set; } = "*.*";

    /// <summary>
    /// Called when a file is selected, or cleared. Provides a <see cref="BrowserBlob"/> populated
    /// from the selected <see cref="IBrowserFile"/>, or null if the selection was cleared. The
    /// <see cref="FieldBase{T}.OnChange"/> callback is also invoked with the filename as the
    /// change value.
    /// </summary>
    [Parameter]
    public EventCallback<BrowserBlob?> OnFileSelected { get; set; }

    protected override void OnInitialized()
    {
        if(Icon == "") {
            Icon = "input-file";
        }
        if(Affordance == "") {
            Affordance = "open-folder";
        }
        if(Placeholder == "") {
            Placeholder = "choose file...";
        }
        base.OnInitialized();
    }

    private async Task HandleFileChangeAsync(InputFileChangeEventArgs e)
    {
        var blob = e.FileCount > 0 ? new BrowserBlob(e.File) : null;
        await OnFileSelected.InvokeAsync(blob);
        var args = new ChangeEventArgs { Value = blob };
        await NotifyChange(args);
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "file", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue => string.IsNullOrWhiteSpace(Value?.Title) ? Placeholder : Value.Title;

    private string PlaceholderCssClass => string.IsNullOrWhiteSpace(Value?.Title) ? "placeholder" : "";

    private string DisplayValueCssClasses => DataConverter.JoinNonEmpty(" ", "value", PlaceholderCssClass, ReadOnlyCss);
}
