namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A filter builder used by the PageQueryBuilder that supports date and times.
/// </summary>
/// <remarks>
/// The time portion of the builder has not been implemented at this point, but will be added when
/// required.
/// </remarks>
public class DateTimeFilterBuilder : FilterBuilder
{
    private const string IsoDateFormat = "yyyy-MM-dd";

    public DateTime? Lower { get; set; }

    public bool LowerInclusive { get; set; } = true;

    public DateTime? Upper { get; set; }

    public bool UpperInclusive { get; set; } = true;

    public void SetDates(DateTime? lower, DateTime? upper)
    {
        Lower = lower;
        Upper = upper;
    }

    ///  <inheritdoc cref="FilterBuilder.Build" />
    public override string Build()
        => Lower != null || Upper != null
            ? $"{FilterName}:{(LowerInclusive ? "[" : "(")}{BuildDate(Lower)},{BuildDate(Upper)}{(UpperInclusive ? "]" : ")")}"
            : string.Empty;

    ///  <inheritdoc cref="FilterBuilder.Reset" />
    public override void Reset()
    {
        Lower = null;
        LowerInclusive = true;
        Upper = null;
        UpperInclusive = true;
    }

    /// <summary>
    /// Converts the specified string representation of a date and time filter to its 
    /// DateTimeFilterBuilder equivalent.
    /// </summary>
    /// <returns>Returns a boolean that indicates whether the conversion succeeded.</returns>
    public bool TryParseFilter(string filterString)
    {
        var filterKeyValue = filterString.Split(':');
        if(filterKeyValue.Length != 2) { return false; }

        var filterName = filterKeyValue[0];
        var filterValue = filterKeyValue[1];
        if(string.IsNullOrEmpty(filterName) || string.IsNullOrEmpty(filterValue)) { return false; }

        if(!(filterValue.StartsWith('[') || filterValue.StartsWith('('))
            || !(filterValue.EndsWith(']') || filterValue.EndsWith(')'))
            || !filterValue.Contains(',')) {

            return false;
        }

        var filterDates = filterValue[1..(filterValue.Length - 1)].Split(',');
        if(filterDates.Length < 1 || filterDates.Length > 2 || filterDates.All(fd => string.IsNullOrEmpty(fd))) {
            return false;
        }

        LowerInclusive = filterValue.StartsWith('[');
        if(!string.IsNullOrEmpty(filterDates[0])) {
            if(TryParseDateTime(filterDates[0], out var lower)) {
                Lower = lower;
            }
            else {
                return false;
            }
        }
        UpperInclusive = filterValue.EndsWith(']');
        if(!string.IsNullOrEmpty(filterDates[1])) {
            if(TryParseDateTime(filterDates[1], out var upper)) {
                Upper = upper;
            }
            else {
                return false;
            }
        }

        FilterName = filterName;
        return true;
    }

    private static string BuildDate(DateTime? date) => date.HasValue ? date.Value.ToString(IsoDateFormat, CultureInfo.InvariantCulture) : string.Empty;

    private static bool TryParseDateTime(string dateString, out DateTime? date)
    {
        date = null;
        var dateTimeParts = dateString.Split('-');
        if(dateTimeParts.Length < 1 || dateTimeParts.Length > 3) {
            return false;
        }
        if(dateTimeParts.Any(part => string.IsNullOrEmpty(part) || !Int32.TryParse(part, out _))) {
            return false;
        }
        try {
            date = DateTime.ParseExact(dateString, IsoDateFormat, CultureInfo.InvariantCulture);
            return true;
        }
        catch(FormatException) {
            return false;
        }
    }
}
