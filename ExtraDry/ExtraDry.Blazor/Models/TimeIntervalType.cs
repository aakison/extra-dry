namespace ExtraDry.Blazor;

/// <summary>
/// A time interval to track the time context when switching between date range views.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TimeIntervalType
{
    /// <summary>
    /// There is no specific time interval, and date based paging is not used.
    /// </summary>
    Static,

    /// <summary>
    /// The range is a multiple of days.
    /// </summary>
    Days,

    /// <summary>
    /// The range is a multiple of months.
    /// </summary>
    Months,

    /// <summary>
    /// The range is a multiple of calendar quarters.
    /// </summary>
    Quarters,

    /// <summary>
    /// The range is a multiple of years.
    /// </summary>
    Years
}
