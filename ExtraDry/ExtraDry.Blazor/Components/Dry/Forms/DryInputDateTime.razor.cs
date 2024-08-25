namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a datetime-local, date, or time input field. Prefer the use of <see
/// cref="DryInput{T}" /> instead of this component as it is more flexible and supports more data
/// types.
/// </summary>
/// <typeparam name="T">
/// The type of the Model that the input renders a property for. Supports DateTime, DateOnly, and
/// TimeOnly.
/// </typeparam>
public partial class DryInputDateTime<T> 
    : DryInputBase<T> 
    where T : class
{

    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

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

    private static List<Type> SupportedPropertyTypes => [typeof(DateTime), typeof(DateOnly), typeof(TimeOnly), typeof(string)];

    private static List<Type> SupportedDisplayTypes => [typeof(DateTime), typeof(DateOnly), typeof(TimeOnly)];

    private string TimeZone { get; set; } = "";

    private string TimeZoneDisplay => TimeZone == "" ? "" : $" ({TimeZone})";

    /// <summary>
    /// Override the base ResolvedTitle to include the time zone display.
    /// </summary>
    private new string ResolvedTitle => $"{base.ResolvedTitle}{TimeZoneDisplay}";

    /// <summary>
    /// The actual input type based on the property type and input type. This is typically the
    /// requested InputType unless the conversion is not possible.
    /// </summary>
    private Type ActualInputType { get; set; } = typeof(DateTime);

    /// <summary>
    /// Type that is emitted with the HTML input element, for browser support of dates and times.
    /// </summary>
    private string HtmlDisplayMode => ActualInputType switch {
        Type t when t == typeof(DateOnly) => "date",
        Type t when t == typeof(TimeOnly) => "time",
        _ => "datetime-local"
    };

    private new string ResolvedAffordance => 
        Affordance == ""
        
        ? Property?.InputFormat?.Affordance
        ?? Property?.InputType switch {
            Type t when t == typeof(DateOnly) => "select-date",
            Type t when t == typeof(DateOnly?) => "select-date",
            Type t when t == typeof(TimeOnly) => "select-time",
            Type t when t == typeof(TimeOnly?) => "select-time",
            _ => "select-datetime"
        }
        : Affordance;

    private string ReadOnlyCss => ReadOnly ? "readonly" : "";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", HtmlDisplayMode, ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

    /// <summary>
    /// Browser enables space to get dialog on date/time but doesn't disable it when the field is 
    /// readonly. This hack disables the space key when the field is readonly.  It also disables 
    /// the up and down arrows which would scroll the page.
    /// </summary>
    private string DisableSpaceWhenReadOnlyHack => ReadOnly ? @"if(event.code == 'Space' || event.code == 'ArrowUp' || event.code == 'ArrowDown') return false;" : "";

    private string ConvertModelPropertyToString()
    {
        var property = Property?.GetValue(Model);
        if(Property == null || property == null) {
            return string.Empty;
        }
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
                Logger.LogWarning("Unable to parse string date {StringDate}, rendering field without date/time.", stringDate);
            }
        }
        else {
            throw new NotImplementedException("Unsupported property type, can't convert to DateTime");
        }

        string? result;
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

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        Value = args.Value?.ToString() ?? "";
        var parseValid = false;
        object? newValue = null;
        if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(DateTime)) {
            if(DateTime.TryParse(Value, out var datetime)) {
                newValue = datetime.ToUniversalTime();
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                var dateTime = dateOnly.ToDateTime(new TimeOnly());
                newValue = dateTime;
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(DateTime) && ActualInputType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                newValue = new DateOnly().ToDateTime(timeOnly);
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(DateTime)) {
            if(DateTime.TryParse(Value, out var datetime)) {
                newValue = datetime.ToUniversalTime().ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                newValue = dateOnly.ToString(DateOnlyFormat, CultureInfo.InvariantCulture);
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(string) && ActualInputType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                newValue = timeOnly.ToString(TimeOnlyFormat, CultureInfo.InvariantCulture);
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(DateOnly)) {
            if(DateOnly.TryParse(Value, out var dateOnly)) {
                newValue = dateOnly;
                parseValid = true;
            }
        }
        else if(Property.PropertyType == typeof(TimeOnly)) {
            if(TimeOnly.TryParse(Value, out var timeOnly)) {
                newValue = timeOnly;
                parseValid = true;
            }
        }


        if(parseValid) {
            Property.SetValue(Model, newValue);
            var validation = ValidateProperty();
            await InvokeOnChangeAsync(newValue);
            await InvokeOnValidationAsync(validation);
        }
        else {
            var validation = new ValidationEventArgs {
                IsValid = false,
                MemberName = Property.Property.Name,
                Message = $"Invalid date/time format",
            };
            await InvokeOnValidationAsync(validation);
        }
    }

    private void SetActualInputType()
    {
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

    private const string DateOnlyFormat = "yyyy-MM-dd";

    private const string TimeOnlyFormat = "HH:mm:ss";

    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
}
