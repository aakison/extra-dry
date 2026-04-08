namespace ExtraDry.Core;

/// <summary>
/// Validates that a string property contains a valid e-mail address, but passes validation when
/// the value is absent (null or empty string).
/// </summary>
/// <remarks>
/// The BCL <see cref="EmailAddressAttribute"/> predates C# nullable reference types. It correctly
/// returns <see langword="true"/> for <see langword="null"/> but returns <see langword="false"/>
/// for <c>""</c>, creating an asymmetry: empty string is the natural default for non-nullable
/// strings, yet it is rejected by a format validator that has no presence concern. A format
/// validator should only validate when a value is actually present, deferring presence enforcement
/// to <see cref="RequiredAttribute"/>. This attribute corrects that design by treating <c>""</c>
/// the same as <see langword="null"/>.
/// </remarks>
public class OptionalEmailAddressAttribute : ValidationAttribute
{
    /// <inheritdoc cref="OptionalEmailAddressAttribute" />
    public OptionalEmailAddressAttribute()
    {
        ErrorMessage = "Please enter a valid e-mail address";
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) =>
        value is null or "" || _emailAddress.IsValid(value);

    private static readonly EmailAddressAttribute _emailAddress = new();
}
