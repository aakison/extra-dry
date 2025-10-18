namespace ExtraDry.Blazor.Models.InputValueFormatters;

[Obsolete("Use INumberFormatter instead")]
public abstract class InputValueFormatter(
    PropertyDescription property)
{
    public abstract string Format(object? value);

    public abstract bool TryParse(string? value, out object? result);

    protected PropertyDescription Property { get; } = property;
}
