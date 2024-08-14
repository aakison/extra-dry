using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a text input field.  Prefer the use of <see cref="DryInput{T}"/> 
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputNumeric<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
{

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

    /// <summary>
    /// Becuase we are mutating the value that is displayed within the handle change (to strip or calculate values), we need to implement differently
    /// The OnChange functionality will not allow for this (there is a hack to assign the backing field to null, then sleep, then repopulate) so using the binding functionality is the recommended way
    /// https://github.com/dotnet/aspnetcore/issues/17099
    /// </summary>
    private void HandleChange(string newValue)
    {
        if(Property == null || Model == null) {
            return;
        }

        // In the future, if we are to allow basic calculations in numeric fields, this is where that would go.
        // Note that this will run synchronously in the setter of Value and therefore needs to be quick.

        var value = Regex.Replace(newValue, @"[^\d.,]", "");
        Property.SetValue(Model, value);

        var dec = decimal.Parse(value, CultureInfo.CurrentCulture);
        if(Property.InputFormat == typeof(int)) {
            _Value = dec.ToString("#,#", CultureInfo.CurrentCulture);
        }
        else {
            _Value = dec.ToString("#,#.##", CultureInfo.CurrentCulture);
        }
    }

    private async Task CallOnChange() { 
        var task = OnChange?.InvokeAsync();
        if(task != null) {
            await task;
        }
    }

    private string Icon => Property?.Property?.GetCustomAttribute<InputFormatAttribute>()?.Icon ?? string.Empty;
    private string Affordance => Property?.Property?.GetCustomAttribute<InputFormatAttribute>()?.Affordance ?? string.Empty;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string _Value = string.Empty;
    private string Value {
        get {
            return _Value;
        }
        set {
            HandleChange(value);
        }
    }

}
