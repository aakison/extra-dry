using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a single select dropdown list.  Prefer the use of <see cref="DryInput{T}"/> 
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputSingleSelect<T> 
    : ComponentBase, IDryInput<T>, IExtraDryComponent 
    where T : class
{

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public T Model { get; set; } = null!;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public PropertyDescription Property { get; set; } = null!;

    /// <summary>
    /// Set of values to select from, any object can be used and the ToString values are displayed.
    /// </summary>
    [Parameter, EditorRequired]
    public List<object>? Values { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected override void OnParametersSet()
    {
        if(Model != null) {
            SelectedValue = Property?.GetValue(Model);
        }
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private async Task SelectOption(ChangeEventArgs args)
    {
        if(Values == null || !int.TryParse(args.Value as string, out var index)) {
            return; // selected blank line
        }
        SelectedValue = Values[index];
        if(Model != null) {
            Property?.SetValue(Model, SelectedValue);
            await InvokeOnChange(args);
        }
    }

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        await OnChange.InvokeAsync(args);
    }

    private bool ReadOnly => EditMode == EditMode.ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private object? SelectedValue { get; set; }

}
