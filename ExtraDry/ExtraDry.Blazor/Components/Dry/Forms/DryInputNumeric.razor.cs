using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata;

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

    private decimal? MinValue {
        get {
            var range = Property?.Property?.GetCustomAttribute<RangeAttribute>()?.Minimum;
            var decimalRange = Property?.Property?.GetCustomAttribute<DecimalRangeAttribute>()?.Minimum;
            return GetValueFromAttributes(range, decimalRange);
        }
    }

    private decimal? MaxValue {
        get {
            var range = Property?.Property?.GetCustomAttribute<RangeAttribute>()?.Maximum;
            var decimalRange = Property?.Property?.GetCustomAttribute<DecimalRangeAttribute>()?.Maximum;
            return GetValueFromAttributes(range, decimalRange);
        }
    }

    private decimal Step {
        get {
            // A precision would solve this issue, however, there is no precision outside EF attributes, so we assume either 0 or 2.
            var types = new List<Type> { typeof(decimal), typeof(decimal?), typeof(float) };
            if(Property?.PropertyType == null) {
                return 1m;
            }

            if(types.Contains(Property.PropertyType)) {
                return 0.01m;
            }
            return 1m;
        }
    }

    private static decimal? GetValueFromAttributes(object? range, decimal? decimalRange)
    {
        if(range == null && decimalRange == null) {
            return null;
        }
        if(decimalRange != null) {
            return decimalRange.Value;
        }

        try {
            return Convert.ToDecimal(range, CultureInfo.InvariantCulture);
        }
        catch {
            return null;
        }
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

}
