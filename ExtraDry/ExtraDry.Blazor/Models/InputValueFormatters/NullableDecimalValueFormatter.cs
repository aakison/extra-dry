namespace ExtraDry.Blazor.Models.InputValueFormatters;

[Obsolete("Use NullableDoubleFormatter instead")]
public class NullableDecimalValueFormatter(
    PropertyDescription property)
    : InputValueFormatter(property)
{
    public override string Format(object? value)
    {
        var val = (decimal?)value;
        if(!val.HasValue) {
            return Property.NullDisplayText;
        }
        var format = Property.Format?.DataFormatString ?? "#,0.00";
        return val.Value.ToString(format, CultureInfo.CurrentCulture);
    }

    public override bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = null;
            return true;
        }
        value = value.Replace(",", "").Replace("$", "");
        if(decimal.TryParse(value, out var decimalResult)) {
            result = decimalResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }
}
