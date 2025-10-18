namespace ExtraDry.Blazor.Models.InputValueFormatters;

[Obsolete("Use DecimalFormatter instead")]
public class DecimalValueFormatter(
    PropertyDescription property)
    : InputValueFormatter(property)
{
    public override string Format(object? value)
    {
        var val = (decimal)value!;
        var format = Property.Format?.DataFormatString ?? "#,0.00";
        return val.ToString(format, CultureInfo.CurrentCulture);
    }

    public override bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = 0.0m;
            return true;
        }
        value = value.Replace(",", "").Replace("$", "");
        if(decimal.TryParse(value, out var decimalResult)) {
            result = decimalResult;
            return true;
        }
        else {
            result = 0.0m;
            return false;
        }
    }
}
