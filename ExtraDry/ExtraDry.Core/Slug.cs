﻿using System.Text;

namespace ExtraDry.Core;

/// <summary>
/// Helper class to facilitate the maintenance of Slugs that can be used in a URI path without
/// escaping.
/// </summary>
public static class Slug
{
    #region Slug Methods

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching Slug.
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueSlug` instead.</remarks>
    public static string ToSlug(string name, bool lowercase = true)
    {
        return ToSlugInternal(name, lowercase);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching Slug, with a maximum
    /// length.
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueSlug` instead.</remarks>
    public static string ToSlug(string name, int maxLength, bool lowercase = true)
    {
        var slug = ToSlug(name, lowercase);
        return slug.Length > maxLength ? slug.Substring(0, maxLength) : slug;
    }

    /// <summary>
    /// Given a UUID, create a matching Slug that is slightly more URI friendly.
    /// </summary>
    /// <remarks>
    /// The UUID is converted to a string and each character is replaced with a more URI friendly
    /// character. The alpha characters are also remapped to retain sorting order. This was
    /// introduced with the introduction of V7 UUIDs which are also sortable based on time created.
    /// </remarks>
    public static string ToSlug(Guid uuid)
    {
        return new string(uuid.ToString().Select(SlugChar).ToArray());

        static char SlugChar(char c) => char.IsDigit(c) ? "acemnopqrs"[c - '0'] : char.IsLetter(c) ? "uvwxyz"[c - 'a'] : c;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching Slug.
    /// </summary>
    /// <remarks>
    /// The list of `existing` Slugs is checked and on collision, an alternate Slug is created.
    /// </remarks>
    public static string ToUniqueSlug(string name, int maxLength, IEnumerable<string> existing)
    {
        var stem = ToSlug(name, maxLength - 6);
        return ToUnique(existing, stem);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching Slug.
    /// </summary>
    /// <remarks>
    /// The `existsAsync` method is used to check if the Slugs already exists, such as in a
    /// database.
    /// </remarks>
    public static async Task<string> ToUniqueSlugAsync(string name, int maxLength, Func<string, Task<bool>> existsAsync)
    {
        var stem = ToSlug(name, maxLength - 6);
        return await ToUniqueAsync(existsAsync, stem);
    }

    #endregion Slug Methods

    #region TitleSlug Methods

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching TitleSlug. A TitleSlug
    /// contains only lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueTitleSlug` instead.</remarks>
    public static string ToTitleSlug(string name)
    {
        return ToSlugInternal(name, true, string.Empty);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching TitleSlug, with a maximum
    /// length. A TitleSlug contains only lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueTitleSlug` instead.</remarks>
    public static string ToTitleSlug(string name, int maxLength)
    {
        var slug = ToSlugInternal(name, true, string.Empty);
        return slug.Length > maxLength ? slug.Substring(0, maxLength) : slug;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching TitleSlug. A TitleSlug
    /// contains only lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>
    /// The list of `existing` Slugs is checked and on collision, an alternate TitleSlug is
    /// created.
    /// </remarks>
    public static string ToUniqueTitleSlug(string name, int maxLength, IEnumerable<string> existing)
    {
        var stem = ToTitleSlug(name, maxLength - 6);
        return ToUnique(existing, stem);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching TitleSlug. A TitleSlug
    /// contains only lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>
    /// The `existsAsync` method is used to check if the TitleSlugs already exists, such as in a
    /// database.
    /// </remarks>
    public static async Task<string> ToUniqueTitleSlugAsync(string name, int maxLength, Func<string, Task<bool>> existsAsync)
    {
        var stem = ToTitleSlug(name, maxLength - 6);
        return await ToUniqueAsync(existsAsync, stem);
    }

    #endregion TitleSlug Methods

    #region CodeSlug Methods

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching CodeSlug. A CodeSlug
    /// contains upper or lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueCodeSlug` instead.</remarks>
    public static string ToCodeSlug(string name)
    {
        return ToSlugInternal(name, false, string.Empty);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching CodeSlug, with a maximum
    /// length. A CodeSlug contains only lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>This does not guarantee uniqueness, consider `ToUniqueCodeSlug` instead.</remarks>
    public static string ToCodeSlug(string name, int maxLength)
    {
        var slug = ToSlugInternal(name, false, string.Empty);
        return slug.Length > maxLength ? slug.Substring(0, maxLength) : slug;
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching CodeSlug. A CodeSlug
    /// contains upper or lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>
    /// The list of `existing` Slugs is checked and on collision, an alternate CodeSlug is created.
    /// </remarks>
    public static string ToUniqueCodeSlug(string name, int maxLength, IEnumerable<string> existing)
    {
        var stem = ToCodeSlug(name, maxLength - 6);
        return ToUnique(existing, stem);
    }

    /// <summary>
    /// Given a name, with punctuation and mixed case, create a matching CodeSlug. A CodeSlug
    /// contains upper or lowercase letters, digits and hyphens
    /// </summary>
    /// <remarks>
    /// The `existsAsync` method is used to check if the CodeSlugs already exists, such as in a
    /// database.
    /// </remarks>
    public static async Task<string> ToUniqueCodeSlugAsync(string name, int maxLength, Func<string, Task<bool>> existsAsync)
    {
        var stem = ToCodeSlug(name, maxLength - 6);
        return await ToUniqueAsync(existsAsync, stem);
    }

    #endregion CodeSlug Methods

    /// <summary>
    /// Generates a string of random characters suitable for embedding in a Slug.
    /// </summary>
    public static string RandomWebString(int count)
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz";
        return RandomCharacters(count, characters);
    }

    /// <summary>
    /// Used to determine if a character can be used in a slug.
    /// </summary>
    /// <remarks>
    /// Used instead of char.IsLetterOrDigit becuase that doesn't discriminate on charater set,
    /// which can allow invalid characters for urls
    /// </remarks>
    private static bool IsAsciiLetterOrDigit(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
    }

    private static string ToSlugInternal(string name, bool lowercase = true, string okCharacters = @"_")
    {
        if(string.IsNullOrWhiteSpace(name)) {
            return string.Empty;
        }
        var sb = new StringBuilder(name.Length);
        name = name.Replace("&", "and");
        var hiddenCharacters = ".'"; // exclude to contract abbreviations and possessives.
        name = RemoveCommonNameSuffixes(name);
        foreach(var c in name) {
            if(IsAsciiLetterOrDigit(c)) {
                sb.Append(lowercase ? char.ToLowerInvariant(c) : c);
            }
            else if(okCharacters.Contains(c)) {
                sb.Append(c);
            }
            else if(!hiddenCharacters.Contains(c)) { // prevent possesives from having a floating space.  E.g. "St. Albert's" -> "st-alberts"
                sb.Append('-');
            }
        }
        name = sb.ToString();
        name = name.Replace("--", "-");
        name = name.Replace("--", "-");
        name = name.TrimEnd('-');
        return name;
    }

    private static string ToUnique(IEnumerable<string> existing, string stem)
    {
        // If the name has any non-hyphen characters, we'll use that as the stem. Else we'll
        // generate a generic slug
        var hasValidStemCondition = stem.Any(IsAsciiLetterOrDigit);
        var candidate = hasValidStemCondition ? stem : GenerateGenericSlug();

        while(existing.Contains(candidate)) {
            candidate = hasValidStemCondition ? $"{stem}-{RandomWebString(5)}" : GenerateGenericSlug();
        }
        return candidate;
    }

    /// <summary>
    /// Returns a generic slug in the format aaaaa-aaaaaaa, for use when the provided stem is not
    /// valid.
    /// </summary>
    private static string GenerateGenericSlug()
    {
        return $"{RandomWebString(5)}-{RandomWebString(7)}";
    }

    private static async Task<string> ToUniqueAsync(Func<string, Task<bool>> existsAsync, string stem)
    {
        // If the name has any non-hyphen characters, we'll use that as the stem. Else we'll
        // generate a generic slug
        var hasValidStemCondition = stem.Any(IsAsciiLetterOrDigit);
        var candidate = hasValidStemCondition ? stem : GenerateGenericSlug();

        while(await existsAsync(candidate)) {
            candidate = hasValidStemCondition ? $"{stem}-{RandomWebString(5)}" : GenerateGenericSlug();
        }
        return candidate;
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
            return value.Substring(0, value.Length - end.Length).TrimEnd();
        }
        else {
            return value;
        }
    }

    private static readonly string[] removeEndings = ["The", "Ltd", "Pty", "S.A", "Inc", "Incorporated", "Pty td", "(Inc)"];
}
