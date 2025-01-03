using System.Globalization;

namespace ExtraDry.Blazor.Tests.Models;

public class TimeIntervalTests
{
    private readonly DateTimeFilterBuilder filter;

    public TimeIntervalTests()
    {
        filter = new DateTimeFilterBuilder { FilterName = "Created" }; ;
    }

    [Fact]
    public void CreateObject()
    {
        var timeInterval = new TimeInterval(TimeIntervalType.Static, "Test");

        Assert.NotNull(timeInterval);
        Assert.Equal("Test", timeInterval.Title);
        Assert.Equal(string.Empty, timeInterval.Description);
        Assert.Equal("Test", timeInterval.Summary);
        Assert.False(timeInterval.IsClone);
    }

    [Theory]
    [InlineData("", "2024-02-29", "Created:[,2024-02-29]")]
    [InlineData("2024-02-29", "", "Created:[2024-02-29,]")]
    [InlineData("", "", "")]
    public void StaticInterval(string start, string end, string expectedFilter)
    {
        var timeInterval = new TimeInterval("Static",
                                            string.IsNullOrEmpty(start) ? null : DateTime.Parse(start, CultureInfo.InvariantCulture),
                                            string.IsNullOrEmpty(end) ? null : DateTime.Parse(end, CultureInfo.InvariantCulture));

        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal("Static", timeInterval.Title);
        Assert.Equal(string.Empty, timeInterval.Description);
        Assert.Equal("Static", timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData("Created:[,2024-02-29]", "Before 29 Feb 2024")]
    [InlineData("Created:[2024-02-29,]", "After 29 Feb 2024")]
    public void StaticIntervalParseFilter(string filterString, string expectedSummary)
    {
        var timeInterval = new TestableTimeInterval();
        filter.TryParseFilter(filterString);

        var result = timeInterval.TryParseFilter(filter);

        Assert.True(result);
        Assert.Equal(string.Empty, timeInterval.Title);
        Assert.Equal(TimeIntervalType.Static, timeInterval.Type);
        Assert.Equal(expectedSummary, timeInterval.Summary);
        filter.Reset();
        timeInterval.SetFilter(filter);
        Assert.Equal(filterString, filter.Build());
    }

    [Theory]
    [InlineData(0, "Today")]
    [InlineData(-1, "Feb 27")]
    [InlineData(-7, "Feb 21-27")]
    [InlineData(-30, "Jan 29-Feb 27")]
    public void DayIntervalCreate(int day, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Days, day, day.ToString(CultureInfo.InvariantCulture));

        Assert.NotNull(timeInterval);
        Assert.Equal(day.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, 0, "Today", "Created:[2024-02-28,2024-02-28]")]
    [InlineData(0, 1, "Feb 27", "Created:[2024-02-27,2024-02-27]")]
    [InlineData(0, 2, "Feb 26", "Created:[2024-02-26,2024-02-26]")]
    [InlineData(0, 3, "Feb 25", "Created:[2024-02-25,2024-02-25]")]
    [InlineData(-1, 1, "Feb 26", "Created:[2024-02-26,2024-02-26]")]
    [InlineData(-1, 2, "Feb 25", "Created:[2024-02-25,2024-02-25]")]
    [InlineData(-1, 3, "Feb 24", "Created:[2024-02-24,2024-02-24]")]
    [InlineData(-2, 0, "Feb 26-27", "Created:[2024-02-26,2024-02-27]")]
    [InlineData(-7, 1, "Feb 14-20", "Created:[2024-02-14,2024-02-20]")]
    [InlineData(-7, 2, "Feb 7-13", "Created:[2024-02-07,2024-02-13]")]
    [InlineData(-7, 3, "Jan 31-Feb 6", "Created:[2024-01-31,2024-02-06]")]
    [InlineData(-30, 1, "Dec 30-Jan 28", "Created:[2023-12-30,2024-01-28]")]
    [InlineData(-30, 2, "Nov 30-Dec 29", "Created:[2023-11-30,2023-12-29]")]
    [InlineData(-30, 3, "Oct 31-Nov 29", "Created:[2023-10-31,2023-11-29]")]
    public void DayIntervalPrevious(int day, int offset, string expectedDescription, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Days, day, day.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Previous();
        }
        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal(day.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, 3, "Mar 2", "Created:[2024-03-02,2024-03-02]")]
    [InlineData(0, 2, "Mar 1", "Created:[2024-03-01,2024-03-01]")]
    [InlineData(0, 1, "Feb 29", "Created:[2024-02-29,2024-02-29]")]
    [InlineData(0, 0, "Today", "Created:[2024-02-28,2024-02-28]")]
    [InlineData(-1, 3, "Mar 1", "Created:[2024-03-01,2024-03-01]")]
    [InlineData(-1, 2, "Feb 29", "Created:[2024-02-29,2024-02-29]")]
    [InlineData(-1, 1, "Today", "Created:[2024-02-28,2024-02-28]")]
    [InlineData(-2, 0, "Feb 26-27", "Created:[2024-02-26,2024-02-27]")]
    [InlineData(-7, 3, "Mar 13-19", "Created:[2024-03-13,2024-03-19]")]
    [InlineData(-7, 2, "Mar 6-12", "Created:[2024-03-06,2024-03-12]")]
    [InlineData(-7, 1, "Feb 28-Mar 5", "Created:[2024-02-28,2024-03-05]")]
    [InlineData(-30, 3, "Apr 28-May 27", "Created:[2024-04-28,2024-05-27]")]
    [InlineData(-30, 2, "Mar 29-Apr 27", "Created:[2024-03-29,2024-04-27]")]
    [InlineData(-30, 1, "Feb 28-Mar 28", "Created:[2024-02-28,2024-03-28]")]
    public void DayIntervalNext(int day, int offset, string expectedDescription, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Days, day, day.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Next();
        }
        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal(day.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, 0, "Created:[2024-02-28,2024-02-28]")]
    [InlineData(0, 1, "Created:[2024-02-29,2024-02-29]")]
    [InlineData(0, -1, "Created:[2024-02-27,2024-02-27]")]
    [InlineData(-1, 0, "Created:[2024-02-27,2024-02-27]")]
    [InlineData(-1, 1, "Created:[2024-02-28,2024-02-28]")]
    [InlineData(-1, -1, "Created:[2024-02-26,2024-02-26]")]
    [InlineData(-2, 0, "Created:[2024-02-26,2024-02-27]")]
    [InlineData(-7, 0, "Created:[2024-02-21,2024-02-27]")]
    [InlineData(-7, 1, "Created:[2024-02-28,2024-03-05]")]
    [InlineData(-7, -1, "Created:[2024-02-14,2024-02-20]")]
    [InlineData(-30, 0, "Created:[2024-01-29,2024-02-27]")]
    [InlineData(-30, 1, "Created:[2024-02-28,2024-03-28]")]
    [InlineData(-30, -1, "Created:[2023-12-30,2024-01-28]")]
    public void DayIntervalSetFilter(int day, int offset, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Days, day, day.ToString(CultureInfo.InvariantCulture));
        if(offset < 0) {
            timeInterval.Previous();
        }
        else if(offset > 0) {
            timeInterval.Next();
        }

        timeInterval.SetFilter(filter);

        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData("Created:[2024-02-28,2024-02-28]", 0, "Today")]
    [InlineData("Created:[2024-02-29,2024-02-29]", 0, "Feb 29")]
    [InlineData("Created:[2024-02-27,2024-02-27]", 0, "Feb 27")]
    [InlineData("Created:[2024-02-26,2024-02-26]", 0, "Feb 26")]
    [InlineData("Created:[2024-02-26,2024-02-27]", -2, "Feb 26-27")]
    [InlineData("Created:[2024-02-21,2024-02-27]", -7, "Feb 21-27")]
    [InlineData("Created:[2024-02-28,2024-03-05]", -7, "Feb 28-Mar 5")]
    [InlineData("Created:[2024-02-14,2024-02-20]", -7, "Feb 14-20")]
    [InlineData("Created:[2024-01-29,2024-02-27]", -30, "Jan 29-Feb 27")]
    [InlineData("Created:[2024-02-28,2024-03-28]", -30, "Feb 28-Mar 28")]
    [InlineData("Created:[2023-12-30,2024-01-28]", -30, "Dec 30-Jan 28")]
    public void DayIntervalParseFilter(string filterString, int expectedInterval, string expectedSummary)
    {
        var timeInterval = new TestableTimeInterval();
        filter.TryParseFilter(filterString);

        var result = timeInterval.TryParseFilter(filter);

        Assert.True(result);
        Assert.Equal(string.Empty, timeInterval.Title);
        Assert.Equal(TimeIntervalType.Days, timeInterval.Type);
        Assert.Equal(expectedInterval, timeInterval.Interval);
        Assert.Equal(expectedSummary, timeInterval.Summary);
        filter.Reset();
        timeInterval.SetFilter(filter);
        Assert.Equal(filterString, filter.Build());
    }

    [Theory]
    [InlineData(0, "Feb 2024")]
    [InlineData(-1, "Jan 2024")]
    [InlineData(-3, "Nov 2023-Jan 2024")]
    [InlineData(-6, "Aug 2023-Jan 2024")]
    [InlineData(-12, "Feb 2023-Jan 2024")]
    public void MonthIntervalCreate(int month, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Months, month, month.ToString(CultureInfo.InvariantCulture));

        Assert.NotNull(timeInterval);
        Assert.Equal(month.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, 1, "Jan 2024", "Created:[2024-01-01,2024-01-31]")]
    [InlineData(0, 2, "Dec 2023", "Created:[2023-12-01,2023-12-31]")]
    [InlineData(0, 3, "Nov 2023", "Created:[2023-11-01,2023-11-30]")]
    [InlineData(-1, 1, "Dec 2023", "Created:[2023-12-01,2023-12-31]")]
    [InlineData(-1, 2, "Nov 2023", "Created:[2023-11-01,2023-11-30]")]
    [InlineData(-1, 3, "Oct 2023", "Created:[2023-10-01,2023-10-31]")]
    [InlineData(-3, 1, "Aug-Oct 2023", "Created:[2023-08-01,2023-10-31]")]
    [InlineData(-3, 2, "May-Jul 2023", "Created:[2023-05-01,2023-07-31]")]
    [InlineData(-3, 3, "Feb-Apr 2023", "Created:[2023-02-01,2023-04-30]")]
    [InlineData(-6, 1, "Feb-Jul 2023", "Created:[2023-02-01,2023-07-31]")]
    [InlineData(-6, 2, "Aug 2022-Jan 2023", "Created:[2022-08-01,2023-01-31]")]
    [InlineData(-6, 3, "Feb-Jul 2022", "Created:[2022-02-01,2022-07-31]")]
    [InlineData(-12, 1, "Feb 2022-Jan 2023", "Created:[2022-02-01,2023-01-31]")]
    [InlineData(-12, 2, "Feb 2021-Jan 2022", "Created:[2021-02-01,2022-01-31]")]
    [InlineData(-12, 3, "Feb 2020-Jan 2021", "Created:[2020-02-01,2021-01-31]")]
    public void MonthIntervalPrevious(int month, int offset, string expectedDescription, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Months, month, month.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Previous();
        }
        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal(month.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, 3, "May 2024")]
    [InlineData(0, 2, "Apr 2024")]
    [InlineData(0, 1, "Mar 2024")]
    [InlineData(-1, 3, "Apr 2024")]
    [InlineData(-1, 2, "Mar 2024")]
    [InlineData(-1, 1, "Feb 2024")]
    [InlineData(-2, 0, "Dec 2023-Jan 2024")]
    [InlineData(-3, 3, "Aug-Oct 2024")]
    [InlineData(-3, 2, "May-Jul 2024")]
    [InlineData(-3, 1, "Feb-Apr 2024")]
    [InlineData(-6, 3, "Feb-Jul 2025")]
    [InlineData(-6, 2, "Aug 2024-Jan 2025")]
    [InlineData(-6, 1, "Feb-Jul 2024")]
    [InlineData(-12, 3, "Feb 2026-Jan 2027")]
    [InlineData(-12, 2, "Feb 2025-Jan 2026")]
    [InlineData(-12, 1, "Feb 2024-Jan 2025")]
    public void MonthIntervalNext(int month, int offset, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Months, month, month.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Next();
        }

        Assert.NotNull(timeInterval);
        Assert.Equal(month.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, 0, "Created:[2024-02-01,2024-02-29]")]
    [InlineData(-1, 0, "Created:[2024-01-01,2024-01-31]")]
    [InlineData(-2, 0, "Created:[2023-12-01,2024-01-31]")]
    [InlineData(-3, 0, "Created:[2023-11-01,2024-01-31]")]
    [InlineData(-3, -1, "Created:[2023-08-01,2023-10-31]")]
    [InlineData(-6, 0, "Created:[2023-08-01,2024-01-31]")]
    [InlineData(-12, 0, "Created:[2023-02-01,2024-01-31]")]
    public void MonthIntervalSetFilter(int month, int offset, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Months, month, month.ToString(CultureInfo.InvariantCulture));
        if(offset < 0) {
            timeInterval.Previous();
        }
        else if(offset > 0) {
            timeInterval.Next();
        }

        timeInterval.SetFilter(filter);

        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData("Created:[2024-02-01,2024-02-29]", 0, "Feb 2024")]
    [InlineData("Created:[2024-01-01,2024-01-31]", 0, "Jan 2024")]
    [InlineData("Created:[2023-12-01,2024-01-31]", -2, "Dec 2023-Jan 2024")]
    [InlineData("Created:[2023-11-01,2024-01-31]", -3, "Nov 2023-Jan 2024")]
    [InlineData("Created:[2023-08-01,2023-10-31]", -3, "Aug-Oct 2023")]
    [InlineData("Created:[2023-08-01,2024-01-31]", -6, "Aug 2023-Jan 2024")]
    [InlineData("Created:[2023-02-01,2024-01-31]", -12, "Feb 2023-Jan 2024")]
    public void MonthIntervalParseFilter(string filterString, int expectedInterval, string expectedSummery)
    {
        var timeInterval = new TestableTimeInterval();
        filter.TryParseFilter(filterString);

        var result = timeInterval.TryParseFilter(filter);

        Assert.True(result);
        Assert.Equal(string.Empty, timeInterval.Title);
        Assert.Equal(TimeIntervalType.Months, timeInterval.Type);
        Assert.Equal(expectedInterval, timeInterval.Interval);
        Assert.Equal(expectedSummery, timeInterval.Summary);
        filter.Reset();
        timeInterval.SetFilter(filter);
        Assert.Equal(filterString, filter.Build());
    }

    [Theory]
    [InlineData(0, "Jan-Mar 2024")]
    [InlineData(-1, "Oct-Dec 2023")]
    [InlineData(-2, "Jul-Sep 2023")]
    [InlineData(-3, "Apr-Jun 2023")]
    [InlineData(-4, "Jan-Mar 2023")]
    public void QuarterIntervalCreate(int quarter, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Quarters, quarter, quarter.ToString(CultureInfo.InvariantCulture));

        Assert.NotNull(timeInterval);
        Assert.Equal(quarter.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, 1, "Oct-Dec 2023", "Created:[2023-10-01,2023-12-31]")]
    [InlineData(0, 2, "Jul-Sep 2023", "Created:[2023-07-01,2023-09-30]")]
    [InlineData(0, 3, "Apr-Jun 2023", "Created:[2023-04-01,2023-06-30]")]
    [InlineData(0, 4, "Jan-Mar 2023", "Created:[2023-01-01,2023-03-31]")]
    public void QuarterIntervalPrevious(int quarter, int offset, string expectedDescription, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Quarters, 0, "0");

        for(int i = 0; i < offset; i++) {
            timeInterval.Previous();
        }
        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal(quarter.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, 1, "Apr-Jun 2024")]
    [InlineData(0, 2, "Jul-Sep 2024")]
    [InlineData(0, 3, "Oct-Dec 2024")]
    [InlineData(0, 4, "Jan-Mar 2025")]
    public void QuarterIntervalNext(int quarter, int offset, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Quarters, 0, "0");

        for(int i = 0; i < offset; i++) {
            timeInterval.Next();
        }

        Assert.NotNull(timeInterval);
        Assert.Equal(quarter.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, "Created:[2024-01-01,2024-03-31]")]
    [InlineData(-1, "Created:[2023-10-01,2023-12-31]")]
    [InlineData(-2, "Created:[2023-07-01,2023-09-30]")]
    [InlineData(-3, "Created:[2023-04-01,2023-06-30]")]
    [InlineData(-4, "Created:[2023-01-01,2023-03-31]")]
    public void QuarterIntervalSetFilter(int quarter, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Quarters, quarter, quarter.ToString(CultureInfo.InvariantCulture));

        timeInterval.SetFilter(filter);

        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, "2024")]
    [InlineData(-1, "2023")]
    [InlineData(-2, "2022-2023")]
    [InlineData(-3, "2021-2023")]
    public void YearIntervalCreate(int year, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Years, year, year.ToString(CultureInfo.InvariantCulture));

        Assert.NotNull(timeInterval);
        Assert.Equal(year.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, 1, "2023", "Created:[2023-01-01,2023-12-31]")]
    [InlineData(0, 2, "2022", "Created:[2022-01-01,2022-12-31]")]
    [InlineData(0, 3, "2021", "Created:[2021-01-01,2021-12-31]")]
    [InlineData(-1, 1, "2022", "Created:[2022-01-01,2022-12-31]")]
    [InlineData(-1, 2, "2021", "Created:[2021-01-01,2021-12-31]")]
    [InlineData(-1, 3, "2020", "Created:[2020-01-01,2020-12-31]")]
    [InlineData(-3, 1, "2018-2020", "Created:[2018-01-01,2020-12-31]")]
    [InlineData(-3, 2, "2015-2017", "Created:[2015-01-01,2017-12-31]")]
    [InlineData(-3, 3, "2012-2014", "Created:[2012-01-01,2014-12-31]")]
    public void YearIntervalPrevious(int year, int offset, string expectedDescription, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Years, year, year.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Previous();
        }
        timeInterval.SetFilter(filter);

        Assert.NotNull(timeInterval);
        Assert.Equal(year.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData(0, 3, "2027")]
    [InlineData(0, 2, "2026")]
    [InlineData(0, 1, "2025")]
    [InlineData(-1, 3, "2026")]
    [InlineData(-1, 2, "2025")]
    [InlineData(-1, 1, "2024")]
    [InlineData(-2, 0, "2022-2023")]
    [InlineData(-3, 3, "2030-2032")]
    [InlineData(-3, 2, "2027-2029")]
    [InlineData(-3, 1, "2024-2026")]
    public void YearIntervalNext(int year, int offset, string expectedDescription)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Years, year, year.ToString(CultureInfo.InvariantCulture));

        for(int i = 0; i < offset; i++) {
            timeInterval.Next();
        }

        Assert.NotNull(timeInterval);
        Assert.Equal(year.ToString(CultureInfo.InvariantCulture), timeInterval.Title);
        Assert.Equal(expectedDescription, timeInterval.Description);
        Assert.Equal(expectedDescription, timeInterval.Summary);
    }

    [Theory]
    [InlineData(0, "Created:[2024-01-01,2024-12-31]")]
    [InlineData(-1, "Created:[2023-01-01,2023-12-31]")]
    [InlineData(-2, "Created:[2022-01-01,2023-12-31]")]
    [InlineData(-3, "Created:[2021-01-01,2023-12-31]")]
    public void YearIntervalSetFilter(int year, string expectedFilter)
    {
        var timeInterval = new TestableTimeInterval(TimeIntervalType.Years, year, year.ToString(CultureInfo.InvariantCulture));

        timeInterval.SetFilter(filter);

        Assert.Equal(expectedFilter, filter.Build());
    }

    [Theory]
    [InlineData("Created:[2024-01-01,2024-12-31]", 0, "2024")]
    [InlineData("Created:[2023-01-01,2023-12-31]", 0, "2023")]
    [InlineData("Created:[2022-01-01,2023-12-31]", -2, "2022-2023")]
    [InlineData("Created:[2021-01-01,2023-12-31]", -3, "2021-2023")]
    public void YearIntervalParseFilter(string filterString, int expectedInterval, string expectedSummery)
    {
        var timeInterval = new TestableTimeInterval();
        filter.TryParseFilter(filterString);

        var result = timeInterval.TryParseFilter(filter);

        Assert.True(result);
        Assert.Equal(string.Empty, timeInterval.Title);
        Assert.Equal(TimeIntervalType.Years, timeInterval.Type);
        Assert.Equal(expectedInterval, timeInterval.Interval);
        Assert.Equal(expectedSummery, timeInterval.Summary);
        filter.Reset();
        timeInterval.SetFilter(filter);
        Assert.Equal(filterString, filter.Build());
    }

    /// <summary>
    /// Test class used to make the TimeInterval testable.
    /// </summary>
    private sealed class TestableTimeInterval : TimeInterval
    {
        public TestableTimeInterval()
        { }

        public TestableTimeInterval(TimeIntervalType type, int interval, string title) : base(type, interval, title)
        {
        }

        /// <summary>
        /// Override the DateTimeNow property to create a fixed point in time.
        /// </summary>
        protected override DateTime DateTimeNow => new(2024, 2, 28);
    }
}
