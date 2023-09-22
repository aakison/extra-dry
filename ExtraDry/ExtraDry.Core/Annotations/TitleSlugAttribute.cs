namespace ExtraDry.Core;

/// <summary>
/// Validates that a string field is a valid Slug which is being used as a URL friendly unique ID.
/// This only allows lowercase letters, numbers, and hyphens.
/// </summary>
public class TitleSlugAttribute : RegularExpressionAttribute {

    /// <inheritdoc cref="TitleSlugAttribute" />
    public TitleSlugAttribute() : base(TitleSlugRegex)
    {
        ErrorMessage = "The {0} field should use only lowercase letters, numbers and hyphens.";
    }

    private const string TitleSlugRegex = "^[a-z0-9-]+$";
}

