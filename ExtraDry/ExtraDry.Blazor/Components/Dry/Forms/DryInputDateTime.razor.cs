using Newtonsoft.Json.Linq;

namespace ExtraDry.Blazor.Forms;

public partial class DryInputDateTime<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
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

    /// <inheritdoc/>
    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public Action<bool, string>? SetValidation { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }

        Value = DisplayMode switch {
            "date" => StringValue("yyyy-MM-dd"),
            "time" => StringValue("HH:mm"),
            _ => StringValue("yyyy-MM-ddTHH:mm")
        };
    }

    private string StringValue(string format)
    {
        var prop = Property?.GetValue(Model);
        if(Property == null || prop == null) {
            return string.Empty;
        }
        if(Property.PropertyType == typeof(DateTime)) {
            return ((DateTime)prop).ToLocalTime().ToString(format, CultureInfo.InvariantCulture);
        }
        else if(Property.PropertyType == typeof(DateOnly)) {
            return ((DateOnly)prop).ToString(format, CultureInfo.InvariantCulture);
        }
        else if(Property.PropertyType == typeof(TimeOnly)) {
            return ((TimeOnly)prop).ToString(format, CultureInfo.InvariantCulture);
        }
        return prop?.ToString() ?? string.Empty;
    }



    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        var valid = false;
        if(Property.PropertyType == typeof(DateTime)) {
            if(DateTime.TryParse(value?.ToString(), out var datetime)) {
                Property.SetValue(Model, datetime.ToUniversalTime());
                valid = true;
            } 
        }
        else if(Property.PropertyType == typeof(DateOnly)) {
            if(DateOnly.TryParse(value?.ToString(), out var dateOnly)) {
                Property.SetValue(Model, dateOnly);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(value?.ToString(), out var timeOnly)) {
                Property.SetValue(Model, timeOnly);
                valid = true;
            }
        }
        if(SetValidation != null) {
            SetValidation(valid, valid ? string.Empty : $"Value is not a valid {Property.PropertyType.Name}");
        }

        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    private string DisplayMode {
        get {
            if(Property?.InputFormat == typeof(DateOnly)) {
                return "date";
            }
            else if(Property?.InputFormat == typeof(TimeOnly)) {
                return "time";
            }
            return "datetime-local";
        }
    }


    private string Icon => Property?.Property?.GetCustomAttribute<InputFormatAttribute>()?.Icon ?? string.Empty;
    private string Affordance => Property?.Property?.GetCustomAttribute<InputFormatAttribute>()?.Affordance ?? string.Empty;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = string.Empty;
}
