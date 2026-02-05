using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

public class TimeInterval
{
    private DateTime? endDate;

    private DateTime? startDate;

    public TimeInterval()
    { }

    public TimeInterval(string title, DateTime? start, DateTime? end)
    {
        Title = title;
        Type = TimeIntervalType.Static;
        startDate = start;
        endDate = end;
    }

    public TimeInterval(TimeIntervalType type, string title) : this(type, 0, title)
    {
    }

    public TimeInterval(TimeIntervalType type, int interval, string title)
    {
        Title = title;
        Type = type;
        Interval = interval;
        Initialise();
    }

    private TimeInterval(TimeInterval timeInterval, DateTime? start, DateTime? end)
    {
        Title = timeInterval.Title;
        Type = timeInterval.Type;
        Interval = timeInterval.Interval;
        Description = timeInterval.Description;
        IsClone = true;
        startDate = start;
        endDate = end;
    }

    public TimeIntervalType Type { get; private set; }

    /// <summary>
    /// The range between the lower and upper dates.
    /// </summary>
    public int Interval { get; private set; }

    public string Title { get; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool IsClone { get; }

    public string Summary => string.IsNullOrEmpty(Description) ? Title : Description;

    public void Previous()
    {
        var interval = Interval == 0
            ? Type == TimeIntervalType.Quarters ? -3 : -1
            : Type == TimeIntervalType.Quarters ? Interval * 3 : Interval;
        MoveDates(interval);
    }

    public void Next()
    {
        var interval = Interval == 0
            ? Type == TimeIntervalType.Quarters ? 3 : 1
            : Type == TimeIntervalType.Quarters ? PositiveInterval * 3 : PositiveInterval;
        MoveDates(interval);
    }

    protected virtual DateTime DateTimeNow => DateTime.Now;

    private int PositiveInterval => Interval switch {
        < 0 => -Interval,
        0 => 1,
        _ => Interval,
    };

    private void Initialise()
    {
        switch(Type) {
            case TimeIntervalType.Days:
                SetDays();
                break;

            case TimeIntervalType.Months:
                SetMonths();
                break;

            case TimeIntervalType.Quarters:
                SetQuarter();
                break;

            case TimeIntervalType.Years:
                SetYears();
                break;
        }
        ;
    }

    private void SetDays()
    {
        endDate = DateTimeNow;
        startDate = Interval == 0 ? endDate : endDate.Value.AddDays(Interval);
        // If there is an interval then the range excludes today, i.e. seven days before today.
        endDate = Interval == 0 ? endDate : endDate.Value.AddDays(-1);
        SetDaysDescription();
    }

    [SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "False Positive")]
    private void SetDaysDescription()
    {
        if(!startDate.HasValue || !endDate.HasValue) { return; }

        /*
         *  For one day format as 'MMM d' i.e. Feb 28, unless the date is today then use 'Today'
         *  For a range within the same month format as 'MMM d-d' i.e. 'Feb 14-20'
         *  For a range in different months format as 'MMM d-MMM d' i.e. 'Jan 20-Feb 20'
         */
        if(startDate.Value.Date == endDate.Value.Date && endDate.Value.Date == DateTimeNow.Date) {
            Description = "Today";
        }
        else if(endDate.Value.Day == startDate.Value.Day && endDate.Value.Month == startDate.Value.Month) {
            Description = $"{startDate.Value.ToString("MMM", CultureInfo.InvariantCulture)} {startDate.Value.Day}";
        }
        else if(startDate.Value.Month == endDate.Value.Month) {
            Description = $"{startDate.Value.ToString("MMM", CultureInfo.InvariantCulture)} {startDate.Value.Day}-{endDate.Value.Day}";
        }
        else {
            Description = $"{startDate.Value.ToString("MMM", CultureInfo.InvariantCulture)} {startDate.Value.Day}-{endDate.Value.ToString("MMM", CultureInfo.InvariantCulture)} {endDate.Value.Day}";
        }
    }

    private void SetMonths()
    {
        endDate = DateTimeNow;
        startDate = endDate.Value.AddMonths(Interval);
        startDate = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
        // If there is an interval then the range excludes this month, i.e. three months before
        // this month.
        endDate = Interval == 0 ? endDate : endDate.Value.AddMonths(-1);
        endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, DateTime.DaysInMonth(endDate.Value.Year, endDate.Value.Month));
        SetMonthsDescription();
    }

    [SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "False Positive")]
    private void SetMonthsDescription()
    {
        if(!startDate.HasValue || !endDate.HasValue) { return; }

        /*
         *  For one month format as 'MMM yyyy' i.e. 'Feb 2024'
         *  For a range within the same year format as 'MMM-MMM yyyy' i.e. 'Jan-Mar 2024'
         *  For a range in different months format as 'MMM yyyy-MMM yyyy' i.e. 'Dec 2023-Feb 2024'
         */
        if(endDate.Value.Month == startDate.Value.Month && endDate.Value.Year == startDate.Value.Year) {
            Description = $"{endDate.Value.ToString("MMM yyyy", CultureInfo.InvariantCulture)}";
        }
        else if(startDate.Value.Year == endDate.Value.Year) {
            Description = $"{startDate.Value.ToString("MMM", CultureInfo.InvariantCulture)}-{endDate.Value.ToString("MMM", CultureInfo.InvariantCulture)} {endDate.Value.ToString("yyyy", CultureInfo.InvariantCulture)}";
        }
        else {
            Description = $"{startDate.Value.ToString("MMM yyyy", CultureInfo.InvariantCulture)}-{endDate.Value.ToString("MMM yyyy", CultureInfo.InvariantCulture)}";
        }
    }

    private void SetQuarter()
    {
        var workingDate = DateTimeNow.AddMonths(Interval * 3);
        var quarter = (workingDate.Month + 2) / 3;
        switch(quarter) {
            case 1:
                startDate = new DateTime(workingDate.Year, 1, 1);
                endDate = new DateTime(workingDate.Year, 3, DateTime.DaysInMonth(workingDate.Year, 3));
                break;

            case 2:
                startDate = new DateTime(workingDate.Year, 4, 1);
                endDate = new DateTime(workingDate.Year, 6, DateTime.DaysInMonth(workingDate.Year, 6));
                break;

            case 3:
                startDate = new DateTime(workingDate.Year, 7, 1);
                endDate = new DateTime(workingDate.Year, 9, DateTime.DaysInMonth(workingDate.Year, 9));
                break;

            case 4:
                startDate = new DateTime(workingDate.Year, 10, 1);
                endDate = new DateTime(workingDate.Year, 12, DateTime.DaysInMonth(workingDate.Year, 12));
                break;

            default:
                break;
        }
        SetQuarterDescription();
    }

    private void SetQuarterDescription()
    {
        if(!startDate.HasValue || !endDate.HasValue) { return; }

        var quarter = (endDate.Value.Month + 2) / 3;
        string months = quarter switch {
            1 => "Jan-Mar",
            2 => "Apr-Jun",
            3 => "Jul-Sep",
            4 => "Oct-Dec",
            _ => string.Empty,
        };
        Description = $"{months} {endDate.Value.Year}";
    }

    private void SetYears()
    {
        endDate = DateTimeNow;
        startDate = Interval == 0 ? endDate : endDate.Value.AddYears(Interval);
        startDate = new DateTime(startDate.Value.Year, 1, 1);
        // If there is an interval then the range excludes this year, i.e. three years before this
        // year.
        endDate = Interval == 0 ? endDate : endDate.Value.AddYears(-1);
        endDate = new DateTime(endDate.Value.Year, 12, 31);
        SetYearsDescription();
    }

    private void SetYearsDescription()
    {
        if(!startDate.HasValue || !endDate.HasValue) { return; }

        /*
         *  For one year format as 'yyyy' i.e. '2024'
         *  For a range within the same year format as 'yyyy-yyyy' i.e. '2023-2024'
         */
        Description = endDate.Value.Year == startDate.Value.Year
                    ? $"{endDate.Value.Year.ToString(CultureInfo.InvariantCulture)}"
                    : $"{startDate.Value.Year.ToString(CultureInfo.InvariantCulture)}-{endDate.Value.Year.ToString(CultureInfo.InvariantCulture)}";
    }

    private void MoveDates(int interval)
    {
        switch(Type) {
            case TimeIntervalType.Days:
                startDate = startDate?.AddDays(interval);
                endDate = endDate?.AddDays(interval);
                SetDaysDescription();
                break;

            case TimeIntervalType.Months:
                startDate = startDate?.AddMonths(interval);
                if(endDate.HasValue) {
                    endDate = endDate.Value.AddMonths(interval);
                    endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, DateTime.DaysInMonth(endDate.Value.Year, endDate.Value.Month));
                }
                SetMonthsDescription();
                break;

            case TimeIntervalType.Quarters:
                startDate = startDate?.AddMonths(interval);
                if(endDate.HasValue) {
                    endDate = endDate.Value.AddMonths(interval);
                    endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, DateTime.DaysInMonth(endDate.Value.Year, endDate.Value.Month));
                }
                SetQuarterDescription();
                break;

            case TimeIntervalType.Years:
                startDate = startDate?.AddYears(interval);
                if(endDate.HasValue) {
                    endDate = endDate.Value.AddYears(interval);
                    endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, DateTime.DaysInMonth(endDate.Value.Year, endDate.Value.Month));
                }
                SetYearsDescription();
                break;
        }
        ;
    }

    public TimeInterval Clone() => new(this, startDate, endDate);

    internal void SetFilter(DateTimeFilterBuilder filter)
    {
        switch(Type) {
            case TimeIntervalType.Static:
                filter.SetDates(startDate, endDate);
                break;

            case TimeIntervalType.Days:
            case TimeIntervalType.Months:
            case TimeIntervalType.Quarters:
            case TimeIntervalType.Years:
                if(startDate.HasValue && endDate.HasValue) {
                    filter.SetDates(startDate, endDate);
                }
                break;

            default:
                break;
        }
    }

    internal bool TryParseFilter(DateTimeFilterBuilder filter)
    {
        var parsed = filter switch {
            { Lower: var lower, Upper: var upper } when !lower.HasValue || !upper.HasValue => TryParseAsStatic(filter),
            { Lower: var lower, Upper: var upper } when lower!.Value.Month == 1 && lower!.Value.Day == 1 && upper!.Value.Month == 12 && upper.Value.Day == 31 => TryParseAsYears(filter),
            { Lower: var lower, Upper: var upper } when lower!.Value.Day == 1 && upper!.Value.Day == DateTime.DaysInMonth(upper.Value.Year, upper.Value.Month) => TryParseAsMonths(filter),
            _ => TryParseAsDays(filter),
        };
        return parsed;
    }

    private bool TryParseAsStatic(DateTimeFilterBuilder filter)
    {
        if(!filter.Lower.HasValue && !filter.Upper.HasValue) { return false; }

        Type = TimeIntervalType.Static;
        if(filter.Upper.HasValue) {
            endDate = filter.Upper;
            Description = endDate.Value.Date == DateTimeNow.Date ? $"Before today" : $"Before {endDate.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
            return true;
        }

        if(filter.Lower.HasValue) {
            startDate = filter.Lower;
            Description = startDate.Value.Date == DateTimeNow.Date ? $"After today" : $"After {startDate.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
            return true;
        }
        return false;
    }

    private bool TryParseAsDays(DateTimeFilterBuilder filter)
    {
        if(!(filter.Lower.HasValue) || !(filter.Upper.HasValue)) { return false; }

        Type = TimeIntervalType.Days;
        endDate = filter.Upper;
        startDate = filter.Lower;
        Interval = (startDate.Value.Date == endDate.Value.Date) ? 0 : startDate.Value.Subtract(endDate.Value.AddDays(1)).Days;
        SetDaysDescription();

        return true;
    }

    private bool TryParseAsMonths(DateTimeFilterBuilder filter)
    {
        if(!(filter.Lower.HasValue) || !(filter.Upper.HasValue)) { return false; }

        Type = TimeIntervalType.Months;
        endDate = filter.Upper;
        startDate = filter.Lower;
        Interval = (startDate.Value.Month == endDate.Value.Month) ? 0 : ((startDate.Value.Year - endDate.Value.Year) * 12) + startDate.Value.Month - (endDate.Value.Month + 1);
        SetMonthsDescription();

        return true;
    }

    private bool TryParseAsYears(DateTimeFilterBuilder filter)
    {
        if(!filter.Lower.HasValue || !filter.Upper.HasValue) { return false; }

        Type = TimeIntervalType.Years;
        endDate = filter.Upper;
        startDate = filter.Lower;
        Interval = (startDate.Value.Year == endDate.Value.Year) ? 0 : startDate.Value.Year - (endDate.Value.Year + 1);
        SetYearsDescription();

        return true;
    }
}
