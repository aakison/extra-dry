using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a text input field.  Prefer the use of <see cref="DryInput{T}"/> 
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputText<T> : ComponentBase, IDryInput<T>, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        Property.SetValue(Model, value);

        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }



    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

}
