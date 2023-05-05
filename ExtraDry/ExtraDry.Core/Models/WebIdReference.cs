using System.Text.RegularExpressions;

namespace ExtraDry.Core;

/// <summary>
/// A simple container for holding a Web Identifier (WebID) that is safe for use URI paths, such as returned from a Create method.
/// </summary>
[Obsolete("Use ResourceReference instead")]
public class WebIdReference {

    /// <summary>
    /// Create with mandatory UUID.
    /// </summary>
    public WebIdReference(string webId)
    {
        if(!validWebId.IsMatch(webId)) {
            throw new ArgumentException("When creating a WebId, only characters allowed in a URL path are allowed.");
        }
        WebId = webId;
    }

    /// <summary>
    /// The universally unique identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string WebId { get; }

    // Only allow safe characters, following RFC3986
    private static readonly Regex validWebId = new(WebIdAttribute.WebIdRegex, RegexOptions.Compiled);

}
