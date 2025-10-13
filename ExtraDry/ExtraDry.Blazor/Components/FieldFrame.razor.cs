using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ExtraDry.Blazor.Components;

public partial class FieldFrame : ComponentBase
{

    /// <inheritdoc />
    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public string Description { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowDescription { get; set; } = false;

    [Parameter]
    public string Label { get; set; } = "";

    [Parameter]
    public bool ShowLabel { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public bool IsValid { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public string Message { get; set; } = "";

    /// <summary>
    /// The ID for the target input element.
    /// </summary>
    [Parameter, EditorRequired]
    public string For { get; set; } = "";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    private string HtmlDescription => Description.Replace("-", "&#8209;"); // non-breaking-hyphen.

    private bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    private bool DisplayDescription => ShowDescription && HasDescription;

    private bool DisplayLabel => ShowLabel;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "field", SizeClass, /* StateCss, */ /* ValidCss, */ CssClass);

    private string SizeClass => Size.ToString()?.ToLowerInvariant() ?? "";

    private void ToggleDescription(MouseEventArgs _)
    {
        ShowDescription = !ShowDescription;
    }

}
