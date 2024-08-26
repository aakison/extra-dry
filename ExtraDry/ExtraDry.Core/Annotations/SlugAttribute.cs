namespace ExtraDry.Core;

/// <summary>
/// Validates that a string property is a valid Slug for use as a URL friendly ID.
/// </summary>
public class SlugAttribute : RegularExpressionAttribute
{

    /// <inheritdoc cref="SlugAttribute" />
    public SlugAttribute(SlugType type = SlugType.Lowercase) : base(type == SlugType.MixedCase ? MixedCaseSlugRegex : LowercaseSlugRegex)
    {
        if(type == SlugType.MixedCase) {
            ErrorMessage = "The {0} field should use only lowercase and uppercase letters, numbers and hyphens.";
        }
        else {
            ErrorMessage = "The {0} field should use only lowercase letters, numbers and hyphens.";
        }
    }

    private const string LowercaseSlugRegex = "^[a-z0-9-]+$";

    private const string MixedCaseSlugRegex = "^[a-zA-Z0-9-]+$";

}
