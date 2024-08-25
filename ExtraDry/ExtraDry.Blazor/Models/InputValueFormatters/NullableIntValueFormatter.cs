namespace ExtraDry.Blazor.Models.InputValueFormatters;

public class NullableIntValueFormatter(
    PropertyDescription property)
    : InputValueFormatter(property)
{
    public override string Format(object? value)
    {
        var val = (int?)value;
        if(!val.HasValue) {
            return Property.NullDisplayText;
        }
        var format = Property.Format?.DataFormatString ?? "#,0";
        return val.Value.ToString(format, CultureInfo.CurrentCulture);
    }

    public override bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value) 
            || value.Trim().Equals(Property.NullDisplayText, StringComparison.OrdinalIgnoreCase)) {
            result = null;
            return true;
        }
        value = value.Replace(",", "");
        if(int.TryParse(value, out var intResult)) {
            result = intResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }

}
