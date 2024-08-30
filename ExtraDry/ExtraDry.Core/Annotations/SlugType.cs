namespace ExtraDry.Core;

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
