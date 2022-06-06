namespace ExtraDry.Server.DataWarehouse;

[DateDimensionTable]
public class Date {

    [Key]
    public int Id { get; set; }

    [Attribute("Date")]
    public DateTime DateTime { get; set; }

    /// <summary>
    /// The year.
    /// </summary>
    public int Year => DateTime.Year;

    /// <summary>
    /// The month of the year, 1 indexed from January
    /// </summary>
    public int MonthNumber => DateTime.Month;

    /// <summary>
    /// The day of the month, 1 indexed.
    /// </summary>
    public int Day => DateTime.Day;

    /// <summary>
    /// The name of the day of the week.
    /// </summary>
    public DayOfWeek DayOfWeekName => DateTime.DayOfWeek;

    /// <summary>
    /// The number of the day of the week, 1 indexed using Monday as starting day of week.
    /// </summary>
    /// <remarks>Monday is start of week per ISO 8601.</remarks>
    public int DayOfWeekNumber => DateTime.DayOfWeek switch {
        DayOfWeek.Monday => 1,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 3,
        DayOfWeek.Thursday => 4,
        DayOfWeek.Friday => 5,
        DayOfWeek.Saturday => 6,
        DayOfWeek.Sunday => 7,
        _ => throw new NotImplementedException(),
    };

    /// <summary>
    /// The day of the year, 1 indexed from January 1st.
    /// </summary>
    public int DayOfYear => DateTime.DayOfYear;

}
