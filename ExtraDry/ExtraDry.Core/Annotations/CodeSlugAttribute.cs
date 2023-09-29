namespace ExtraDry.Core;

/// <summary>
/// Validates that a string field is a valid Slug which is being used as a URL friendly unique ID.
/// This only allows lowercase and uppercase letters, numbers, and hyphens.
/// </summary>
public class CodeSlugAttribute : RegularExpressionAttribute {

    /// <inheritdoc cref="TitleSlugAttribute" />
    public CodeSlugAttribute() : base(CodeSlugRegex)
    {
        ErrorMessage = "The {0} field should use only lowercase and uppercase letters, numbers and hyphens.";
    }

    private const string CodeSlugRegex = "^[a-zA-Z0-9-]+$";
}

