namespace ExtraDry.Blazor.Models.InputValueFormatters;

[Obsolete("Use IdentityFormatter instead")]
public class IdentityValueFormatter(
    PropertyDescription property)
    : InputValueFormatter(property)
{
    public override string Format(object? value)
    {
        return value?.ToString() ?? "";
    }

    public override bool TryParse(string? value, out object? result)
    {
        result = value;
        return true;
    }
}
