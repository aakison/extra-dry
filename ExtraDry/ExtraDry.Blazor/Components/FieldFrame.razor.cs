using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A frame for input fields that wraps the input element with a label, description and validation message.
/// </summary>
public partial class FieldFrame : ComponentBase
{

    /// <inheritdoc />
    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <summary>
    /// The description text for the field, typically one or two sentences to prompt first time users.
    /// </summary>
    [Parameter]
    public string Description { get; set; } = "";

    /// <summary>
    /// Indicates if the field should show the description if one is available.  The description can
    /// only be shown if one is provided in <see cref="Description"/>, the label itself is shown 
    /// <see cref="ShowLabel"/> and <see cref="ShowDescription"/> is true.
    /// </summary>>
    [Parameter]
    public bool ShowDescription { get; set; } = false;

    /// <summary>
    /// The label text for the field.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = "";

    /// <summary>
    /// Indicates if the label should be shown.  Without a label, the description cannot be shown.
    /// </summary>
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    /// <summary>
    /// Indicates if the field is valid.  This will change the styles of the field to indicate invalid state to user.
    /// </summary>
    [Parameter]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Indicates if the field is required.  This will add a visual indicator to the label.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// The message to show when the field is invalid.  This is not shown if the field is valid.
    /// </summary>
    [Parameter]
    public string Message { get; set; } = "";

    /// <summary>
    /// Determines the rendering mode for the field frame.  Standard mode stacks the label, input and description vertically.
    /// Inline mode places the label and content side by side (e.g. for checkboxes).
    /// </summary>
    [Parameter]
    public FieldFrameRenderMode RenderMode { get; set; } = FieldFrameRenderMode.Standard;

    /// <summary>
    /// The ID for the target input element.  This element is typically provided as the child content of this component.
    /// </summary>
    [Parameter, EditorRequired]
    public string For { get; set; } = "";

    /// <summary>
    /// The content of the field, typically an input element.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    private string HtmlDescription => Description.Replace("-", "&#8209;"); // non-breaking-hyphen.

    private bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    private bool DisplayDescription { get; set; }

    private bool DisplayLabel => ShowLabel;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "field", SizeClass, ModeCss, /* StateCss, */ /* ValidCss, */ CssClass);

    private string SizeClass => Size.ToString()?.ToLowerInvariant() ?? "";

    private string ModeCss => RenderMode switch {
        FieldFrameRenderMode.Inline => "inline",
        FieldFrameRenderMode.Standard => "standard",
        _ => "unknown-render-mode",
    };

    private void ToggleDescription(MouseEventArgs _)
    {
        if(HasDescription && ShowDescription && ShowLabel) {
            DisplayDescription = !DisplayDescription;
        }
    }

}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldFrameRenderMode
{
    Standard,
    Inline,
}
