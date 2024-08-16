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

    /// <summary>
    /// Event that is raised when the input is validated using internal rules.  Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        SetActualInputType();
        Value = HtmlDisplayMode switch {
            "date" => ConvertModelPropertyToString(),
            "time" => ConvertModelPropertyToString(),
            _ => ConvertModelPropertyToString()
        };
    }

    [Inject]
    private ILogger<DryInputDateTime<T>> Logger { get; set; } = null!;

    private static List<Type> SupportedPropertyTypes => [typeof(DateTime), typeof(DateOnly), typeof(TimeOnly), typeof(string)];

    private static List<Type> SupportedDisplayTypes => [typeof(DateTime), typeof(DateOnly), typeof(TimeOnly)];

    private string ConvertModelPropertyToString()
    {
        var property = Property?.GetValue(Model);
        if(Property == null || property == null) {
            return string.Empty;
        }
        var str = (Property.PropertyType, ActualInputType) switch {
            (Type pt, _) when pt == typeof(DateTime) 
                => ((DateTime)property).ToLocalTime().ToString(DateTimeFormat, CultureInfo.InvariantCulture),
            (Type pt, _) when pt == typeof(DateOnly) 
                => ((DateOnly)property).ToString(DateOnlyFormat, CultureInfo.InvariantCulture),
            (Type pt, _) when pt == typeof(TimeOnly) 
                => ((TimeOnly)property).ToString(TimeOnlyFormat, CultureInfo.InvariantCulture),
            _ => property?.ToString() ?? string.Empty
        };
        DateTime? tempDateTime = null;
        if(property is DateTime dateTime) {
            tempDateTime = ActualInputType == typeof(DateTime) ? dateTime.ToLocalTime() : dateTime;
        }
        else if(property is DateOnly dateOnly) {
            tempDateTime = dateOnly.ToDateTime(new TimeOnly());
        }
        else if(property is TimeOnly timeOnly) {
            tempDateTime = new DateOnly().ToDateTime(timeOnly);
        }
        else if(property is string stringDate) {
            if(DateTime.TryParse(stringDate, out var dt)) {
                tempDateTime = dt;
            }
            else { 
                Logger.LogError("Unable to parse string date {StringDate}, rendering field without date/time.", stringDate);
            }
        }
        else {
            throw new NotImplementedException("Unsupported property type, can't convert to DateTime");
        }

        var result = "1970-01-01";
        if(ActualInputType == typeof(DateTime)) {
            result = tempDateTime?.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }
        else if(ActualInputType == typeof(DateOnly)) {
            result = tempDateTime?.ToString(DateOnlyFormat, CultureInfo.InvariantCulture);
        }
        else if(ActualInputType == typeof(TimeOnly)) {
            result = tempDateTime?.ToString(TimeOnlyFormat, CultureInfo.InvariantCulture);
        }
        else {
            throw new NotImplementedException("Unsupported Actual InputType, can't convert to string representation.");
        }

        if(ActualInputType == typeof(DateTime)) {
            if(tempDateTime == null) {
                TimeZone = "";
            }
            else if(tempDateTime.Value.Kind == DateTimeKind.Utc) {
                TimeZone = "UTC";
            }
            else {
                var tz = TimeZoneInfo.Local;
                TimeZone = tz.IsDaylightSavingTime(tempDateTime.Value) ? tz.DaylightName : tz.StandardName;
            }
        }

        return result ?? "";
    }

    private const string DateOnlyFormat = "yyyy-MM-dd";

    private const string TimeOnlyFormat = "HH:mm:ss";

    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

    private string TimeZone { get; set; } = "";

    private string TimeZoneDisplay => TimeZone == "" ? "" : $" ({TimeZone})";

    private string InputTitle => $"{Property?.FieldCaption} {TimeZoneDisplay}";


    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        Value = args.Value?.ToString() ?? "";
        var valid = false;
        if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(DateTime)) {
            if(DateTime.TryParse(Value, out var datetime)) {
                Property.SetValue(Model, datetime.ToUniversalTime());
                valid = true;
            } 
        }
        else if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                var dateTime = dateOnly.ToDateTime(new TimeOnly());
                Property.SetValue(Model, dateTime);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                var dateTime = new DateOnly().ToDateTime(timeOnly);
                Property.SetValue(Model, dateTime);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(DateTime)) {
            if(DateTime.TryParse(Value, out var datetime)) {
                Property.SetValue(Model, datetime.ToUniversalTime().ToString(DateTimeFormat, CultureInfo.InvariantCulture));
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                var dateTime = dateOnly.ToString(DateOnlyFormat, CultureInfo.InvariantCulture);
                Property.SetValue(Model, dateTime);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                var dateTime = timeOnly.ToString(TimeOnlyFormat, CultureInfo.InvariantCulture);
                Property.SetValue(Model, dateTime);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                Property.SetValue(Model, dateOnly);
                valid = true;
            }
        }
        else if(Property.PropertyType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                Property.SetValue(Model, timeOnly);
                valid = true;
            }
        }

        await OnValidation.InvokeAsync(new ValidationEventArgs { 
            IsValid = valid, 
            MemberName = Property.PropertyType.Name,
            Message = valid ? string.Empty : $"Value is not a valid {Property.PropertyType.Name}",
        });

        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    /// <summary>
    /// The actual input type based on the property type and input type.  This is typically the 
    /// requested InputType unless the conversion is not possible.  
    /// </summary>
    private Type ActualInputType { get; set; } = typeof(DateTime);

    private void SetActualInputType() { 
        if(Property == null) {
            throw new InvalidOperationException("Do not call this method until after OnParametersSet");
        }
        ActualInputType = Property.InputType; // default unless a problem...
        if(!SupportedDisplayTypes.Contains(Property.InputType)) {
            Logger.LogWarning("Invalid input type override, DryInputDateTime does not support {InputType}", Property.InputType.Name);
            ActualInputType = Property.PropertyType; // override to PropertyType
        }
        if(Property.PropertyType == typeof(DateOnly) && Property.InputType == typeof(DateTime)
            || Property.PropertyType == typeof(TimeOnly) && Property.InputType == typeof(DateTime)
            || Property.PropertyType == typeof(TimeOnly) && Property.InputType == typeof(DateOnly)
            || Property.PropertyType == typeof(DateOnly) && Property.InputType == typeof(TimeOnly)
            ) {
            Logger.LogWarning("Invalid cast operation requested, DryInputDateTime cannot save a {InputType} as a {PropertyType}", Property.InputType.Name, Property.PropertyType.Name);
            ActualInputType = Property.PropertyType; // override to PropertyType
        }
        if(!SupportedPropertyTypes.Contains(Property.PropertyType)) {
            Logger.LogWarning("Invalid property type, DryInputDateTime does not support {PropertyType}", Property.PropertyType.Name);
            ActualInputType = typeof(DateTime); // override to DateTime to display field
        }
    }

    /// <summary>
    /// Type that is emitted with the HTML input element, for browser support of dates and times.
    /// </summary>
    private string HtmlDisplayMode => ActualInputType switch {
        Type t when t == typeof(DateOnly) => "date",
        Type t when t == typeof(TimeOnly) => "time",
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

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", HtmlDisplayMode, ReadOnlyCss);

    private string Value { get; set; } = "";
}
