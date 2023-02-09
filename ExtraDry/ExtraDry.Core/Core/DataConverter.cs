using System.Reflection;
using System.Text.RegularExpressions;

namespace ExtraDry.Core;

public class DataConverter {

    /// <summary>
    /// Given a date, formats it for display using a relative time.
    /// For example, 5 minutes ago, or Yesterday.
    /// </summary>
    public static string DateToRelativeTime(DateTime dateTime)
    {
        var utc = dateTime.ToUniversalTime();
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
    /// A function which returns the current date and time.  
    /// Defaults to UTC which should match date storage format in databases.
    /// </summary>
    public static Func<DateTime> CurrentDateTime { get; set; } = () => DateTime.UtcNow;

    /// <summary>
    /// Given a camelCase (or PascalCase) string, inserts spaces between words, retaining acronyms.
    /// E.g. "TwoWords" becomes "Two Words", "VGAGraphics" becomes "VGA Graphics".
    /// </summary>
    public static string CamelCaseToTitleCase(string value)
    {
        var acronyms = new Regex(@"(\w)([A-Z][a-z])");
        value = acronyms.Replace(value, "$1 $2");

        var words = new Regex(@"([a-z])([A-Z])");
        value = words.Replace(value, "$1 $2");

        return value;
    }

    /// <summary>
    /// Given a camelCase (or PascalCase) string, converts it to kebab-case.
    /// </summary>
    public static string CamelCaseToKebabCase(string value) => Slug.ToSlug(CamelCaseToTitleCase(value));

    /// <summary>
    /// Gets the DataAnnotation DisplayName attribute for a given enum (for displaying enums values nicely to users)
    /// </summary>
    public static string DisplayEnum(Enum value)
    {
        if(value == null) {
            throw new ArgumentNullException(nameof(value));
        }
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

}
