using System;

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A DateTime structure used by the DateTimeFilterBuilder that supports partial date and times
/// required for filtering.
/// </summary>
/// <remarks>
/// The time portion of the builder has not been implemented at this point, but will be added when
/// required.
/// </remarks>
public struct FilterableDateTime
{
    public FilterableDateTime(int year, int? month = null)
    {
        ValidateDateTime(year, month ?? 1, 1);
        Year = year;
        Month = month;
    }

    public FilterableDateTime(int year, int month, int day)
    {
        ValidateDateTime(year, month, day);
        Year = year;
        Month = month;
        Day = day;
    }

    public FilterableDateTime(DateTime dateTime)
    {
        Year = dateTime.Year;
        Month = dateTime.Month;
        Day = dateTime.Day;
    }

    public int Year { get; }
    public int? Month { get; }
    public int? Day { get; }

    public bool IsFullDate => Month.HasValue && Day.HasValue;

    public string Build() => $"{Year}-{Month}-{Day}".TrimEnd('-');

    /// <summary>
    /// Converts the specified string representation of a date and time filter to its 
    /// FilterableDateTime equivalent.
    /// </summary>
    /// <returns>Returns a value that indicates whether the conversion succeeded.</returns>
    public static FilterableDateTime? TryParseFilterableDateTime(string dateTime)
    {
        if(string.IsNullOrEmpty(dateTime)) {
            return null;
        }

        var dateTimeParts = dateTime.Split('-');
        if(dateTimeParts.Length < 1 || dateTimeParts.Length > 3) {
            return null;
        }
        if(dateTimeParts.Any(part => string.IsNullOrEmpty(part) || !Int32.TryParse(part, out _))) {
            return null;
        }
        return dateTimeParts.Length switch {
            1 => new FilterableDateTime(Convert.ToInt32(dateTimeParts[0], CultureInfo.InvariantCulture)),
            2 => new FilterableDateTime(Convert.ToInt32(dateTimeParts[0], CultureInfo.InvariantCulture),
                                        Convert.ToInt32(dateTimeParts[1], CultureInfo.InvariantCulture)),
            3 => new FilterableDateTime(Convert.ToInt32(dateTimeParts[0], CultureInfo.InvariantCulture),
                                        Convert.ToInt32(dateTimeParts[1], CultureInfo.InvariantCulture),
                                        Convert.ToInt32(dateTimeParts[2], CultureInfo.InvariantCulture)),
            _ => null
        };
    }

    private static void ValidateDateTime(int year, int month, int day)
    {
        _ = new DateTime(year, month, day);
    }
}
