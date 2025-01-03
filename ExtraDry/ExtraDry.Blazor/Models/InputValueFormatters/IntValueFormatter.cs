namespace ExtraDry.Blazor.Models.InputValueFormatters;

public class IntValueFormatter(
    PropertyDescription property)
    : InputValueFormatter(property)
{
    public override string Format(object? value)
    {
        var val = (int)value!;
        var format = Property.Format?.DataFormatString ?? "#,#";
        return val.ToString(format, CultureInfo.CurrentCulture);
    }

    public override bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = 0;
            return true;
        }
        value = value.Replace(",", "");
        if(int.TryParse(value, out var intResult)) {
            result = intResult;
            return true;
        }
        else {
            result = 0;
            return false;
        }
    }
}
