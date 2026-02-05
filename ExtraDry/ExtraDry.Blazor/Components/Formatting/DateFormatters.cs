namespace ExtraDry.Blazor.Components.Formatting;

public class DateTimeFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,2})?";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "yyyy-MM-dd";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        var val = (DateTime)value;
        var formatted = val == DateTime.MinValue ? "" : val.ToString(DataFormat, CultureInfo.InvariantCulture);
        return formatted;
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = DateTime.MinValue;
            return true;
        }
        value = value.Replace(",", "");
        if(DateTime.TryParseExact(value, DataFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateTimeResult)) {
            result = dateTimeResult;
            return true;
        }
        else {
            result = DateTime.MinValue;
            return false;
        }
    }
}

public class NullableDateTimeFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,2})?";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "yyyy-MM-dd";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        var val = (DateTime?)value;
        var formatted = val == DateTime.MinValue ? "" : val?.ToString(DataFormat, CultureInfo.InvariantCulture);
        return formatted ?? "";
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = null;
            return true;
        }
        value = value.Replace(",", "");
        if(DateTime.TryParseExact(value, DataFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateTimeResult)) {
            result = (DateTime?)dateTimeResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }
}


public class DateOnlyFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,2})?";

    public string DataFormat { get; set; } = "yyyy-MM-dd";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        var val = (DateOnly)value;
        var formatted = val == DateOnly.MinValue ? "" : val.ToString(DataFormat, CultureInfo.InvariantCulture);
        return formatted;
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = DateOnly.MinValue;
            return true;
        }
        value = value.Replace(",", "");
        if(DateOnly.TryParseExact(value, DataFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateTimeResult)) {
            result = dateTimeResult;
            return true;
        }
        else {
            result = DateOnly.MinValue;
            return false;
        }
    }
}

public class NullableDateOnlyFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,2})?";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "yyyy-MM-dd";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        var val = (DateOnly?)value;
        var formatted = val == DateOnly.MinValue ? "" : val?.ToString(DataFormat, CultureInfo.InvariantCulture);
        return formatted ?? "";
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = null;
            return true;
        }
        value = value.Replace(",", "");
        if(DateOnly.TryParseExact(value, DataFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateOnlyResult)) {
            result = (DateOnly?)dateOnlyResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }
}

