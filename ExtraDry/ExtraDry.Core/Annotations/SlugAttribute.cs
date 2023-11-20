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
        } else {
            ErrorMessage = "The {0} field should use only lowercase letters, numbers and hyphens.";
        }
    }
             
    private const string LowercaseSlugRegex = "^[a-z0-9-]+$";

    private const string MixedCaseSlugRegex = "^[a-zA-Z0-9-]+$";

}

/// <summary>
/// When validating the slug, the additional constraints to apply.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SlugType
{
    /// <summary>
    /// The slug only consists of lowercase letters, numbers, and hyphens.  This is preferred for
    /// titles of documents and anywhere words are used in the slugs.
    /// </summary>
    Lowercase, 

    /// <summary>
    /// The slug consists of lowercase and uppercase letters, numbers, and hyphens.  This is 
    /// used for codes and other identifiers that are not words.
    /// </summary>
    MixedCase,
}

/// <inheritdoc cref="SlugAttribute" />
[Obsolete("Use SlugAttribute `[Slug(SlugType.Lowercase)]` or [Slug] instead of TitleSlugAttribute")]
public class TitleSlugAttribute : SlugAttribute {

    /// <inheritdoc cref="SlugAttribute" />
    public TitleSlugAttribute() : base(SlugType.Lowercase) { }
}

/// <inheritdoc cref="SlugAttribute" />
[Obsolete("Use SlugAttribute `[Slug(SlugType.MixedCase)]` instead of CodeSlugAttribute")]
public class CodeSlugAttribute : SlugAttribute {

    /// <inheritdoc cref="SlugAttribute" />
    public CodeSlugAttribute() : base(SlugType.MixedCase) { }
}
