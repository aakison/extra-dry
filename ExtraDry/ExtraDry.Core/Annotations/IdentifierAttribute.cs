namespace ExtraDry.Core;

/// <summary>
/// An identifier can contain unicode letters, underscores and digits, but may not start with a
/// digit.
/// </summary>
public class IdentifierAttribute : RegularExpressionAttribute
{
    /// <inheritdoc cref="IdentifierAttribute" />
    public IdentifierAttribute() : base(ValidIdentifierRegex)
    {
        ErrorMessage = "The {0} field can only contain unicode letters, _ and digits (0-9) but may not start with a digit.";
    }

    private const string ValidIdentifierRegex = @"^[_a-zA-Z]\w+$";
}
