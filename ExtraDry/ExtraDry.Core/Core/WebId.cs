#nullable enable

using System.Text;

namespace ExtraDry.Core;

/// <summary>
/// Helper class to facilitate the maintenance of "Web" Ids that can be used in a URI path without escaping.
/// </summary>
public static class WebId {

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching WebId.
    /// </summary>
    /// <remarks>
    /// This does not guarantee uniqueness, consider `ToUniqeWebId` instead.
    /// </remarks>
    public static string ToWebId(string name)
    {
        if(string.IsNullOrWhiteSpace(name)) {
            return string.Empty;
        }
        var sb = new StringBuilder(name.Length);
        var okCharacters = @"_"; // "$*+!,'()." are legal, but  ugly, removing them as well.
        name = name.Replace("&", "and", StringComparison.Ordinal);
        var hiddenCharacters = ".'"; // exclude to contract abbreviations and possessives.
        name = RemoveCommonNameSuffixes(name);
        foreach(var c in name) {
            if(char.IsLetterOrDigit(c)) {
                sb.Append(char.ToLowerInvariant(c));
            }
            else if(okCharacters.Contains(c, StringComparison.OrdinalIgnoreCase)) {
                sb.Append(c);
            }
            else if(!hiddenCharacters.Contains(c)) { // prevent possesives from having a floating space.  E.g. "St. Albert's" -> "st-alberts"
                sb.Append('-');
            }
        }
        name = sb.ToString();
        name = name.Replace("--", "-", StringComparison.Ordinal);
        name = name.Replace("--", "-", StringComparison.Ordinal);
        name = name.TrimEnd('-');
        return name;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching WebId, with a maximum length.
    /// </summary>
    /// <remarks>
    /// This does not guarantee uniqueness, consider `ToUniqeWebId` instead.
    /// </remarks>
    public static string ToWebId(string name, int maxLength)
    {
        var webId = ToWebId(name);
        return webId.Length > maxLength ? webId[0..maxLength] : webId;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching WebId.
    /// </summary>
    /// <remarks>
    /// The list of `existing` WebIds is checked and on collision, an alternate web ID is created.
    /// </remarks>
    public static string ToUniqueWebId(string name, int maxLength, IEnumerable<string> existing)
    {
        var stem = ToWebId(name, maxLength - 6);
        var candidate = stem;
        while(existing.Contains(candidate)) {
            candidate = $"{stem}-{RandomWebString(5)}";
        }
        return candidate;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching WebId.
    /// </summary>
    /// <remarks>
    /// The `existsAsync` method is used to check if the WebIds already exists, such as in a database.
    /// </remarks>
    public static async Task<string> ToUniqueWebIdAsync(string name, int maxLength, Func<string, Task<bool>> existsAsync)
    {
        var stem = ToWebId(name, maxLength - 6);
        var candidate = stem;
        while(await existsAsync(candidate)) {
            candidate = $"{stem}-{RandomWebString(5)}";
        }
        return candidate;
    }

    /// <summary>
    /// Generates a string of random characters suitable for embedding in a WebId.
    /// </summary>
    public static string RandomWebString(int count)
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz";
        return RandomCharacters(count, characters);
    }

    private static string RandomCharacters(int count, string characters)
    {
        var sb = new StringBuilder(count);
        for(int i = 0; i < count; ++i) {
            sb.Append(characters[random.Next(0, characters.Length)]);
        }
        return sb.ToString();
    }

    private static readonly Random random = new();

    private static string RemoveCommonNameSuffixes(string name)
    {
        foreach(var ending in removeEndings) {
            name = name.Trim('-', ',', '.', ' ');
            name = TrimEnd(name, ending);
        }
        name = name.Trim('-', ',', '.', ' ');
        return name;
    }

    private static string TrimEnd(string value, string end)
    {
        if(value.EndsWith(end, StringComparison.InvariantCultureIgnoreCase)) {
            return value[..^end.Length].TrimEnd();
        }
        else {
            return value;
        }
    }

    private static readonly string[] removeEndings = new string[] { "The", "Ltd", "Pty", "S.A", "Inc", "Incorporated", "Pty td", "(Inc)" };
}
