namespace ExtraDry.Core;

/// <summary>
/// Validates that a string field is a valid Slug, allowing it to be part of a URI path without translation.
/// This only allows letters, numbers, and the punctuation "-", "_", ".", and "~".
/// </summary>
public class SlugAttribute : RegularExpressionAttribute {

    /// <summary>
    /// Indicates string is a Slug (previously WebId).
    /// </summary>
    public SlugAttribute() : base(SlugRegex)
    {
        ErrorMessage = SlugErrorMessage;
    }

    private const string SlugErrorMessage = @"Slug should be generated and use only letters and hyphens.";

    /// <summary>
    /// The regex for a valid Slug.
    /// </summary>
    /// <remarks>
    /// Limits to 100 characters, but _should_ be smaller, also use with MaxLength the ensure smaller size.
    /// </remarks>
    public const string SlugRegex = @"^[a-zA-Z0-9-_~\.]{0,100}$";

}
