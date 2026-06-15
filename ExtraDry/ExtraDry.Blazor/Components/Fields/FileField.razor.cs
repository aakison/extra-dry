using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a file field that allows users to select a file from the local file system.
/// The selected filename is stored as the string value. Use <see cref="OnFileSelected"/> to
/// access the file content.
/// </summary>
public partial class FileField : FieldBase<string>
{
    /// <summary>
    /// The accepted file types for the file input, as a comma-separated list of MIME types or
    /// file extensions (e.g. <c>"image/*"</c>, <c>".pdf,.docx"</c>). Defaults to all files.
    /// </summary>
    [Parameter]
    public string Accept { get; set; } = "*.*";

    /// <summary>
    /// Called when a file is selected. Provides access to the <see cref="InputFileChangeEventArgs"/>
    /// for reading file content. The <see cref="FieldBase{T}.OnChange"/> callback is also invoked
    /// with the filename as the change value.
    /// </summary>
    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnFileSelected { get; set; }

    protected override void OnInitialized()
    {
        if(Icon == "") Icon = "input-file";
        if(Affordance == "") Affordance = "open-folder";
        if(Placeholder == "") Placeholder = "choose file...";
        base.OnInitialized();
    }

    private async Task HandleFileChangeAsync(InputFileChangeEventArgs e)
    {
        await OnFileSelected.InvokeAsync(e);
        var filename = e.FileCount > 0 ? e.File.Name : string.Empty;
        var args = new ChangeEventArgs { Value = filename };
        await NotifyChange(args);
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "file", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue => string.IsNullOrWhiteSpace(Value) ? Placeholder : Value;

    private string PlaceholderCssClass => string.IsNullOrWhiteSpace(Value) ? "placeholder" : "";

    private string DisplayValueCssClasses => DataConverter.JoinNonEmpty(" ", "value", PlaceholderCssClass, ReadOnlyCss);
}
