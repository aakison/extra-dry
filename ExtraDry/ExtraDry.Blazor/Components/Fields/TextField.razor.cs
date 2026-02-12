namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a text field that can be single line or multi-line depending on the MaxLength property.
/// </summary>
public partial class TextField : FieldBase<string>
{

    /// <summary>
    /// The type of markdown support for this field. When set to <see cref="MarkdownSupportType.Character"/>
    /// or <see cref="MarkdownSupportType.Block"/>, the field renders a markdown editor instead of a
    /// standard input or textarea.
    /// </summary>
    [Parameter]
    public MarkdownSupportType MarkdownSupportType { get; set; } = MarkdownSupportType.None;

    /// <summary>
    /// A dictionary of bookmark entries shown in the markdown editor's link dialog. Keys are
    /// display names and values are URLs/paths.
    /// </summary>
    [Parameter]
    public Dictionary<string, string>? LinkBookmarks { get; set; }

    /// <summary>
    /// The maximum length of the text. If greater than 100 then a multi-line text area is used.
    /// This is a hard limit to the text field and is separate from the validation model.  For
    /// user consistency, it is recommended to align this with the string length of the property.
    /// </summary>
    [Parameter]
    public int MaxLength { get; set; } = 100;

    /// <summary>
    /// The size of the field. If set to Auto (the default) then the size is determined based on the /// <see cref="MaxLength"/>.
    /// </summary>
    [Parameter]
    public override PropertySize Size { get; set; } = PropertySize.Auto;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ModeCss, ReadOnlyCss, IsValidCss, CssClass);

    private bool IsMarkdown => MarkdownSupportType != MarkdownSupportType.None;

    private string ModeCss => IsMarkdown ? "markdown" : IsMultiline ? "textarea" : "text";

    private bool IsMultiline => MaxLength > StringLength.Line;

    private PropertySize ResolvedSize {
        get {
            if(Size != PropertySize.Auto) {
                return Size;
            }
            return MaxLength switch {
                <= StringLength.Word => PropertySize.Small,
                <= StringLength.Words => PropertySize.Medium,
                <= StringLength.Line => PropertySize.Large,
                _ => PropertySize.Jumbo,
            };
        }
    }

    private string TextSize => MaxLength switch {
        <= StringLength.Word => nameof(StringLength.Word).ToLowerInvariant(),
        <= StringLength.Words => nameof(StringLength.Words).ToLowerInvariant(),
        <= StringLength.Line => nameof(StringLength.Line).ToLowerInvariant(),
        <= StringLength.Sentence => nameof(StringLength.Sentence).ToLowerInvariant(),
        <= StringLength.Paragraph => nameof(StringLength.Paragraph).ToLowerInvariant(),
        <= StringLength.Page => nameof(StringLength.Page).ToLowerInvariant(),
        _ => nameof(StringLength.Book).ToLowerInvariant()
    };

    private async Task OnMarkdownValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
        var args = new ChangeEventArgs { Value = value };
        await OnChange.InvokeAsync(args);
        await OnInput.InvokeAsync(args);
    }

}
