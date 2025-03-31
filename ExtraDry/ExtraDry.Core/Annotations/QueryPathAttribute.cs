namespace ExtraDry.Core;

/// <summary>
/// Validates that a string property is a valid query string for a URL that does not need URL escaping.
/// </summary>
public class QueryPathAttribute : RegularExpressionAttribute
{
    /// <inheritdoc cref="QueryPathAttribute" />
    public QueryPathAttribute(SlugType type = SlugType.Lowercase) : base(type == SlugType.MixedCase ? MixedCaseRegex : LowercaseRegex)
    {
        ErrorMessage = type == SlugType.MixedCase
            ? "The {0} field should start with a slash and use only lowercase and uppercase letters, numbers and limited punctuation ('-', '_', '.', '~')."
            : "The {0} field should start with a slash and use only lowercase letters, numbers and limited punctuation ('-', '_', '.', '~').";
    }

    private const string LowercaseRegex = $"^(/[{Characters}]*)+$";

    private const string MixedCaseRegex = $"^(/[A-Z{Characters}]*)+$";

    private const string Characters = "a-z0-9_.~-";
}
