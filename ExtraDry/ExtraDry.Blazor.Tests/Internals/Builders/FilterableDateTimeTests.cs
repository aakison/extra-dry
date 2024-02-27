using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Blazor.Tests.Internals.Builders;

public class FilterableDateTimeTests
{
    [Fact]
    public void FilterWithYear()
    {
        var filter = new FilterableDateTime(2023);

        var filterString = filter.Build();

        Assert.Equal("2023", filterString);
    }

    [Fact]
    public void FailedWithInvalidYear()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FilterableDateTime(0));
    }

    [Fact]
    public void FilterWithYearAndMonth()
    {
        var filter = new FilterableDateTime(2023, 12);

        var filterString = filter.Build();

        Assert.Equal("2023-12", filterString);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void FailedWithInvalidMonth(int month)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FilterableDateTime(2023, month));
    }

    [Fact]
    public void FilterWithYearMonthAndDay()
    {
        var filter = new FilterableDateTime(2023, 12, 18);

        var filterString = filter.Build();

        Assert.Equal("2023-12-18", filterString);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32)]
    public void FailedWithInvalidDay(int day)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FilterableDateTime(2023, 12, day));
    }

    [Theory]
    [InlineData("2023", 2023, null, null)]
    [InlineData("2023-12", 2023, 12, null)]
    [InlineData("2023-12-18", 2023, 12, 18)]
    public void CanParseDateTime(string filterString, int expectedYear, int? expectedMonth, int? expectedDay)
    {
        var filter = FilterableDateTime.TryParseFilterableDateTime(filterString);

        Assert.NotNull(filter);
        Assert.Equal(expectedYear, filter?.Year);
        Assert.Equal(expectedMonth, filter?.Month);
        Assert.Equal(expectedDay, filter?.Day);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2023-")]
    [InlineData("2023-12-")]
    [InlineData("2023--")]
    [InlineData("2023-12-18-")]
    [InlineData(@"2023\12\18")]
    [InlineData(@"abc-12-18")]
    [InlineData(@"2023-ab-18")]
    [InlineData(@"2023-12-ab")]
    public void FailedParseDateTime(string filterString)
    {
        var filter = FilterableDateTime.TryParseFilterableDateTime(filterString);

        Assert.Null(filter);
    }

    [Fact]
    public void AreEqual()
    {
        var filter = new FilterableDateTime(2024, 2, 23);
        var sameFilter = new FilterableDateTime(2024, 2, 23);

        var equal = filter.Equals(sameFilter);

        Assert.True(equal);
    }

    [Fact]
    public void AreNotEqual()
    {
        var filter = new FilterableDateTime(2024, 2, 23);
        var sameFilter = new FilterableDateTime(2023, 1, 22);

        var equal = filter.Equals(sameFilter);

        Assert.False(equal);
    }
}
