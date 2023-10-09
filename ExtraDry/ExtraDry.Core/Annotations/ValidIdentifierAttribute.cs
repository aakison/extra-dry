namespace ExtraDry.Core;

/// <summary>
/// A valid identifier can contain unicode letters, $, _ and digits (0-9) but may not start with a digit.
/// </summary>
public class ValidIdentifierAttribute : RegularExpressionAttribute {

    /// <inheritdoc cref="TitleSlugAttribute" />
    public ValidIdentifierAttribute() : base(ValidIdentifierRegex)
    {
        ErrorMessage = "The {0} field can only contain unicode letters, $, _ and digits (0-9) but may not start with a digit.";
    }

    private const string ValidIdentifierRegex = "^[_\\$a-zA-Z]\\w+$";
}

