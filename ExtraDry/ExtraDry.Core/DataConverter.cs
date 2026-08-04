using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtraDry.Core;

public partial class DataConverter
{
    public static string DateToDisplayDate(DateTime dateTime)
    {
        // Assume Unspecified is UTC, which is how we store dates in databases.
        var utc = dateTime.Kind switch {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
        var localTime = utc.ToLocalTime();
        return localTime.ToString(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Given a date, formats it for display using a relative time. For example, 5 minutes ago, or
    /// Yesterday.
    /// </summary>
    public static string DateToRelativeTime(DateTime dateTime)
    {
        // Assume Unspecified is UTC, which is how we store dates in databases.
        var utc = dateTime.Kind switch {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
        var current = CurrentDateTime().ToUniversalTime();
        var delta = current - utc;
        var localTime = utc.ToLocalTime();
        var today = current.Date == utc.Date;
        var yesterday = current.Date == utc.Date.AddDays(1);
        if(delta.TotalSeconds < 30) {
            return "Just now";
        }
        else if(delta.TotalMinutes < 2) {
            return "A minute ago";
        }
        else if(delta.TotalMinutes < 60) {
            var minutes = (int)delta.TotalMinutes;
            return $"{minutes} minutes ago";
        }
        else if(delta.TotalHours < 2) {
            return "An hour ago";
        }
        else if(delta.TotalHours < 24 && today) {
            var hours = (int)delta.TotalHours;
            return $"{hours} hours ago";
        }
        else if(yesterday) {
            return $"Yesterday {localTime:hh:mm tt}";
        }
        else if(delta.TotalDays < 6) {
            return $"{localTime:ddd hh:mm tt}";
        }
        else {
            return $"{localTime:MMM dd hh:mm tt}";
        }
    }


    /// <summary>
    /// Given a date, formats it for display using a relative day. For example, Today, Yesterday, or
    /// a specific date.
    /// </summary>
    public static string DateToRelativeDay(DateTime dateTime)
    {
        // Assume Unspecified is UTC, which is how we store dates in databases.
        var utc = dateTime.Kind switch {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
        var current = CurrentDateTime().ToUniversalTime();
        var localTime = utc.ToLocalTime();
        var today = current.Date == utc.Date;
        var yesterday = current.Date == utc.Date.AddDays(1);
        if(today) {
            return "Today";
        }
        else if(yesterday) {
            return $"Yesterday";
        }
        else {
            return $"{localTime:d MMM yyyy}";
        }
    }

    /// <summary>
    /// A function which returns the current date and time. Defaults to UTC which should match date
    /// storage format in databases.
    /// </summary>
    public static Func<DateTime> CurrentDateTime { get; set; } = () => DateTime.UtcNow;

    /// <summary>
    /// Given a camelCase (or PascalCase) string, inserts spaces between words, retaining acronyms.
    /// E.g. "TwoWords" becomes "Two Words", "VGAGraphics" becomes "VGA Graphics".
    /// </summary>
    public static string CamelCaseToTitleCase(string value)
    {
        value = AcronymsInString().Replace(value, "$1 $2");
        value = WordsInString().Replace(value, "$1 $2");
        value = SmallWordsInString().Replace(value, match => match.Value.ToLowerInvariant());
        return value;
    }

    /// <summary>
    /// Given a camelCase (or PascalCase) string, converts it to kebab-case.
    /// </summary>
    public static string CamelCaseToKebabCase(string value) => Slug.ToSlug(CamelCaseToTitleCase(value));

    /// <summary>
    /// Converts a string to ASCII by transliterating Latin characters with diacritics to their
    /// base ASCII equivalents, mapping Unicode whitespace variants to a plain space, and dropping
    /// any remaining non-ASCII characters. Intended for use where the target encoding is
    /// restricted to ISO-8859-1, such as HTTP header values.
    /// E.g. "Screenshot\u202Fpm" becomes "Screenshot pm", "ā" becomes "a".
    /// </summary>
    public static string ToAscii(string value)
    {
        var sb = new StringBuilder(value.Length);
        foreach(var c in value) {
            if(c < 128) {
                sb.Append(c);
            }
            else if(char.IsWhiteSpace(c)) {
                sb.Append(' ');
            }
            else if(LatinToAscii.TryGetValue(c, out var ascii)) {
                sb.Append(ascii);
            }
            // non-mappable non-ASCII characters are dropped
        }
        return sb.ToString();
    }

    /// <summary>
    /// Transliteration table for Latin-1 Supplement and Latin Extended-A to ASCII equivalents.
    /// Covers Western and Central European characters with diacritics.
    /// </summary>
    private static readonly Dictionary<char, char> LatinToAscii = new() {
        // Latin-1 Supplement (U+00C0–U+00FF)
        { 'À', 'A' }, { 'Á', 'A' }, { 'Â', 'A' }, { 'Ã', 'A' }, { 'Ä', 'A' }, { 'Å', 'A' }, { 'Æ', 'A' },
        { 'à', 'a' }, { 'á', 'a' }, { 'â', 'a' }, { 'ã', 'a' }, { 'ä', 'a' }, { 'å', 'a' }, { 'æ', 'a' },
        { 'Ç', 'C' }, { 'ç', 'c' },
        { 'È', 'E' }, { 'É', 'E' }, { 'Ê', 'E' }, { 'Ë', 'E' },
        { 'è', 'e' }, { 'é', 'e' }, { 'ê', 'e' }, { 'ë', 'e' },
        { 'Ì', 'I' }, { 'Í', 'I' }, { 'Î', 'I' }, { 'Ï', 'I' },
        { 'ì', 'i' }, { 'í', 'i' }, { 'î', 'i' }, { 'ï', 'i' },
        { 'Ð', 'D' }, { 'ð', 'd' },
        { 'Ñ', 'N' }, { 'ñ', 'n' },
        { 'Ò', 'O' }, { 'Ó', 'O' }, { 'Ô', 'O' }, { 'Õ', 'O' }, { 'Ö', 'O' }, { 'Ø', 'O' },
        { 'ò', 'o' }, { 'ó', 'o' }, { 'ô', 'o' }, { 'õ', 'o' }, { 'ö', 'o' }, { 'ø', 'o' },
        { 'Ù', 'U' }, { 'Ú', 'U' }, { 'Û', 'U' }, { 'Ü', 'U' },
        { 'ù', 'u' }, { 'ú', 'u' }, { 'û', 'u' }, { 'ü', 'u' },
        { 'Ý', 'Y' }, { 'Ÿ', 'Y' },
        { 'ý', 'y' }, { 'ÿ', 'y' },
        { 'Þ', 'T' }, { 'þ', 't' },
        { 'ß', 's' },
        // Latin Extended-A (U+0100–U+017F)
        { 'Ā', 'A' }, { 'ā', 'a' }, { 'Ă', 'A' }, { 'ă', 'a' }, { 'Ą', 'A' }, { 'ą', 'a' },
        { 'Ć', 'C' }, { 'ć', 'c' }, { 'Ĉ', 'C' }, { 'ĉ', 'c' }, { 'Ċ', 'C' }, { 'ċ', 'c' }, { 'Č', 'C' }, { 'č', 'c' },
        { 'Ď', 'D' }, { 'ď', 'd' }, { 'Đ', 'D' }, { 'đ', 'd' },
        { 'Ē', 'E' }, { 'ē', 'e' }, { 'Ĕ', 'E' }, { 'ĕ', 'e' }, { 'Ė', 'E' }, { 'ė', 'e' }, { 'Ę', 'E' }, { 'ę', 'e' }, { 'Ě', 'E' }, { 'ě', 'e' },
        { 'Ĝ', 'G' }, { 'ĝ', 'g' }, { 'Ğ', 'G' }, { 'ğ', 'g' }, { 'Ġ', 'G' }, { 'ġ', 'g' }, { 'Ģ', 'G' }, { 'ģ', 'g' },
        { 'Ĥ', 'H' }, { 'ĥ', 'h' }, { 'Ħ', 'H' }, { 'ħ', 'h' },
        { 'Ĩ', 'I' }, { 'ĩ', 'i' }, { 'Ī', 'I' }, { 'ī', 'i' }, { 'Ĭ', 'I' }, { 'ĭ', 'i' }, { 'Į', 'I' }, { 'į', 'i' }, { 'İ', 'I' }, { 'ı', 'i' },
        { 'Ĵ', 'J' }, { 'ĵ', 'j' },
        { 'Ķ', 'K' }, { 'ķ', 'k' }, { 'ĸ', 'k' },
        { 'Ĺ', 'L' }, { 'ĺ', 'l' }, { 'Ļ', 'L' }, { 'ļ', 'l' }, { 'Ľ', 'L' }, { 'ľ', 'l' }, { 'Ŀ', 'L' }, { 'ŀ', 'l' }, { 'Ł', 'L' }, { 'ł', 'l' },
        { 'Ń', 'N' }, { 'ń', 'n' }, { 'Ņ', 'N' }, { 'ņ', 'n' }, { 'Ň', 'N' }, { 'ň', 'n' }, { 'ŉ', 'n' }, { 'Ŋ', 'N' }, { 'ŋ', 'n' },
        { 'Ō', 'O' }, { 'ō', 'o' }, { 'Ŏ', 'O' }, { 'ŏ', 'o' }, { 'Ő', 'O' }, { 'ő', 'o' }, { 'Œ', 'O' }, { 'œ', 'o' },
        { 'Ŕ', 'R' }, { 'ŕ', 'r' }, { 'Ŗ', 'R' }, { 'ŗ', 'r' }, { 'Ř', 'R' }, { 'ř', 'r' },
        { 'Ś', 'S' }, { 'ś', 's' }, { 'Ŝ', 'S' }, { 'ŝ', 's' }, { 'Ş', 'S' }, { 'ş', 's' }, { 'Š', 'S' }, { 'š', 's' },
        { 'Ţ', 'T' }, { 'ţ', 't' }, { 'Ť', 'T' }, { 'ť', 't' }, { 'Ŧ', 'T' }, { 'ŧ', 't' },
        { 'Ũ', 'U' }, { 'ũ', 'u' }, { 'Ū', 'U' }, { 'ū', 'u' }, { 'Ŭ', 'U' }, { 'ŭ', 'u' }, { 'Ů', 'U' }, { 'ů', 'u' }, { 'Ű', 'U' }, { 'ű', 'u' }, { 'Ų', 'U' }, { 'ų', 'u' },
        { 'Ŵ', 'W' }, { 'ŵ', 'w' },
        { 'Ŷ', 'Y' }, { 'ŷ', 'y' },
        { 'Ź', 'Z' }, { 'ź', 'z' }, { 'Ż', 'Z' }, { 'ż', 'z' }, { 'Ž', 'Z' }, { 'ž', 'z' },
    };

    /// <summary>
    /// Given a kebab-case string, converts it to title case. E.g. "two-words" becomes "Two Words".
    /// </summary>
    public static string KebabCaseToTitleCase(string value)
    {
        // remove dashes and convert to title case
        value = value.Replace("-", " ");
        value = FirstLetters().Replace(value, match => match.Value.ToUpperInvariant());
        value = SmallWordsInString().Replace(value, match => match.Value.ToLowerInvariant());
        return CamelCaseToTitleCase(value);
    }

    /// <summary>
    /// Gets the DataAnnotation DisplayName attribute for a given enum (for displaying enums values
    /// nicely to users)
    /// </summary>
    public static string DisplayEnum(Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var enumType = value.GetType();
        var enumValue = Enum.GetName(enumType, value);
        if(enumValue == null) {
            // can't find member any more, e.g. it was removed from enum but in value still around.
            return value.ToString();
        }
        var member = enumType.GetMember(enumValue)[0];

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.Name ?? member.Name;
    }

    /// <summary>
    /// Gets the DataAnnotation DisplayName attribute for a given enum (for displaying enums values
    /// nicely to users)
    /// </summary>
    public static string DisplayShortEnum(Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var enumType = value.GetType();
        var enumValue = Enum.GetName(enumType, value);
        if(enumValue == null) {
            // can't find member any more, e.g. it was removed from enum but in value still around.
            return value.ToString();
        }
        var member = enumType.GetMember(enumValue)[0];

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.GetShortName() ?? displayAttribute?.GetName() ?? member.Name;
    }

    public static IList<TEnum> EnumValues<TEnum>()
    {
        var type = typeof(TEnum);
        if(!type.IsEnum) {
            throw new ArgumentException("Generic Type must be an enum");
        }
        var values = new List<TEnum>();
        var enumValues = type.GetEnumValues();
        foreach(var value in enumValues) {
            if(value != null) {
                var memberInfo = type.GetMember(value.ToString()!).First();
                var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
                if(displayAttribute?.GetAutoGenerateField() ?? true) {
                    values.Add((TEnum)value);
                }
            }
        }
        return values;
    }

    public static IList<Enum> EnumValues(Type type)
    {
        if(!type.IsEnum) {
            throw new ArgumentException("Generic Type must be an enum");
        }
        var values = new List<Enum>();
        var enumValues = type.GetEnumValues();
        foreach(var value in enumValues) {
            if(value != null) {
                var memberInfo = type.GetMember(value.ToString()!).First();
                var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
                if(displayAttribute?.GetAutoGenerateField() ?? true) {
                    values.Add((Enum)value);
                }
            }
        }
        return values;
    }

    /// <summary>
    /// Works like the normal string.join, except any args that are null or only whitespace are
    /// ignored. Convenient for use when joining lists of things that might have some optional or
    /// missing items, e.g. CSS classes.
    /// </summary>
    public static string JoinNonEmpty(string separator, params string?[] args)
    {
        return string.Join(separator, args.Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => e!.Trim()));
    }

    [GeneratedRegex(@"(\w)([A-Z][a-z])")]
    private static partial Regex AcronymsInString();

    [GeneratedRegex(@"([a-z])([A-Z])")]
    private static partial Regex WordsInString();

    // lookbehind to avoid capitalizing small words at beginning of sentence
    [GeneratedRegex(@"(?<!^)\b(A|An|And|As|At|But|By|En|For|If|In|Of|On|Or|The|To|V[.]?|Via|Vs[.]?)\b")]
    private static partial Regex SmallWordsInString();

    [GeneratedRegex(@"^\w|\s\w")]
    private static partial Regex FirstLetters();

}
