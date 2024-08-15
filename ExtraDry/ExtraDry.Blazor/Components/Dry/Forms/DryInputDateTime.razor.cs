namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a datetime-local, date, or time input field.  Prefer the use of 
/// <see cref="DryInput{T}"/> instead of this component as it is more flexible and supports more 
/// data types.
/// </summary>
/// <typeparam name="T">
/// The type of the Model that the input renders a property for.  Supports DateTime, DateOnly, and
/// TimeOnly.
/// </typeparam>
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

    private string DisplayMode => Property?.InputType switch {
        Type t when t == typeof(DateOnly) => "date",
        Type t when t == typeof(DateOnly?) => "date",
        Type t when t == typeof(TimeOnly) => "time",
        Type t when t == typeof(TimeOnly?) => "time",
        _ => "datetime-local"
    };

    private string Icon => Property?.InputFormat?.Icon ?? "";

    private string Affordance => Property?.InputFormat?.Affordance
        ?? Property?.InputType switch {
            Type t when t == typeof(DateOnly) => "select-date",
            Type t when t == typeof(DateOnly?) => "select-date",
            Type t when t == typeof(TimeOnly) => "select-time",
            Type t when t == typeof(TimeOnly?) => "select-time",
            _ => "select-datetime"
        };

    private string ReadOnlyCss => ReadOnly ? "readonly" : "";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";
}
