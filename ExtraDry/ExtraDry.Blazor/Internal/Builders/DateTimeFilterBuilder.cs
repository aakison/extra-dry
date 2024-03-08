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
    public FilterableDateTime? Lower { get; set; }

    public bool LowerInclusive { get; set; } = true;

    public FilterableDateTime? Upper { get; set; }

    public bool UpperInclusive { get; set; } = true;

    public void SetDates(DateTime? lower, DateTime? upper)
    {
        Lower = lower.HasValue ? new FilterableDateTime(lower.Value) : null;
        Upper = upper.HasValue ? new FilterableDateTime(upper.Value) : null;
    }

    internal void SetDates(int lowerYear, int? lowerMonth, int upperYear, int? upperMonth)
    {
        Lower = new FilterableDateTime(lowerYear, lowerMonth);
        Upper = new FilterableDateTime(upperYear, upperMonth);
    }

    ///  <inheritdoc cref="FilterBuilder.Build" />
    public override string Build()
        => Lower != null || Upper != null
            ? $"{FilterName}:{(LowerInclusive ? "[" : "(")}{Lower?.Build()},{Upper?.Build()}{(UpperInclusive ? "]" : ")")}"
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

        FilterName = filterName;
        LowerInclusive = filterValue.StartsWith('[');
        if(!string.IsNullOrEmpty(filterDates[0])) {
            Lower = FilterableDateTime.TryParseFilterableDateTime(filterDates[0]);
        }
        UpperInclusive = filterValue.EndsWith(']');
        if(!string.IsNullOrEmpty(filterDates[1])) {
            Upper = FilterableDateTime.TryParseFilterableDateTime(filterDates[1]);
        }
        return true;
    }

}
