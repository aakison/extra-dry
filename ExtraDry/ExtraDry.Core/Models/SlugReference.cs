using System.Text.RegularExpressions;

namespace ExtraDry.Core;

/// <summary>
/// A simple container for holding a Slug (previously WebId) that is safe for use URI paths, such as returned from a Create method.
/// </summary>
public class SlugReference {

    /// <summary>
    /// Create with mandatory UUID.
    /// </summary>
    public SlugReference(string slug)
    {
        if(!validSlug.IsMatch(slug)) {
            throw new ArgumentException("When creating a Slug, only characters allowed in a URL path are allowed.");
        }
        Slug = slug;
    }

    /// <summary>
    /// The universally unique identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Slug { get; }

    // Only allow safe characters, following RFC3986
    private static readonly Regex validSlug = new(SlugAttribute.SlugRegex, RegexOptions.Compiled);

}
