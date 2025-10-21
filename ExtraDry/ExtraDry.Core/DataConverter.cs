using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExtraDry.Core;

public partial class DataConverter
{
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
            return $"Yesterday {dateTime:hh:mm tt}";
        }
        else if(delta.TotalDays < 6) {
            return $"{dateTime:ddd hh:mm tt}";
        }
        else {
            return $"{dateTime:MMM dd hh:mm tt}";
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
