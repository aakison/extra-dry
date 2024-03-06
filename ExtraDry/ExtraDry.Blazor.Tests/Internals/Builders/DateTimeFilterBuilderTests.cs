namespace ExtraDry.Blazor.Tests.Internals.Builders;

public class DateTimeFilterBuilderTests
{
    [Fact]
    public void CreateObject()
    {
        var filter = new DateTimeFilterBuilder();

        Assert.NotNull(filter);
    }

    [Fact]
    public void BuildFilter()
    {
        var filter = new DateTimeFilterBuilder();

        var filterString = filter.Build();

        Assert.Equal(string.Empty, filterString);
    }

    [Fact]
    public void BuildFilterWithLower()
    {
        var filter = new DateTimeFilterBuilder {
            FilterName = "Created",
            Lower = new FilterableDateTime(2023, 12, 18)
        };

        var filterString = filter.Build();

        Assert.Equal("Created:[2023-12-18,]", filterString);
    }

    [Fact]
    public void BuildFilterWithUpper()
    {
        var filter = new DateTimeFilterBuilder {
            FilterName = "Created",
            Upper = new FilterableDateTime(2023, 12, 18)
        };

        var filterString = filter.Build();

        Assert.Equal("Created:[,2023-12-18]", filterString);
    }

    [Fact]
    public void BuildFilterWithLowerAndUpper()
    {
        var filter = new DateTimeFilterBuilder {
            FilterName = "Created",
            Lower = new FilterableDateTime(2022, 1, 1),
            Upper = new FilterableDateTime(2023, 12, 18)
        };

        var filterString = filter.Build();

        Assert.Equal("Created:[2022-1-1,2023-12-18]", filterString);
    }

    [Fact]
    public void ResetFilter()
    {
        var filter = new DateTimeFilterBuilder {
            FilterName = "Created",
            Lower = new FilterableDateTime(2022, 1, 1),
            Upper = new FilterableDateTime(2023, 12, 18)
        };

        filter.Reset();

        Assert.Equal("Created", filter.FilterName);
        Assert.Null(filter.Lower);
        Assert.Null(filter.Upper);
    }

    [Theory]
    [InlineData("Created:[2022,2023]", true, true)]
    [InlineData("Created:(2022,2023]", false, true)]
    [InlineData("Created:[2022,2023)", true, false)]
    [InlineData("Created:(2022,2023)", false, false)]
    public void CanParseFilter(string filterString, bool lowerInclusive, bool upperInclusive)
    {
        var filter = new DateTimeFilterBuilder();
        
        var result = filter.TryParseFilter(filterString);

        Assert.True(result);
        Assert.Equal("Created", filter.FilterName);
        Assert.NotNull(filter.Lower);
        Assert.Equal(2022, filter.Lower.Value.Year);
        Assert.Equal(lowerInclusive, filter.LowerInclusive);
        Assert.NotNull(filter.Upper);
        Assert.Equal(2023, filter.Upper.Value.Year);
        Assert.Equal(upperInclusive, filter.UpperInclusive);
    }

    [Theory]
    [InlineData("Created:[2023,]", true)]
    [InlineData("Created:(2023,]", false)]
    public void CanParseLowerOnlyFilter(string filterString, bool inclusive)
    {
        var filter = new DateTimeFilterBuilder();

        var result = filter.TryParseFilter(filterString);

        Assert.True(result);
        Assert.Equal("Created", filter.FilterName);
        Assert.NotNull(filter.Lower);
        Assert.Equal(2023, filter.Lower.Value.Year);
        Assert.Equal(inclusive, filter.LowerInclusive);
        Assert.Null(filter.Upper);
        
    }

    [Theory]
    [InlineData("Created:[,2023]", true)]
    [InlineData("Created:[,2023)", false)]
    public void CanParseUpperOnlyFilter(string filterString, bool inclusive)
    {
        var filter = new DateTimeFilterBuilder();

        var result = filter.TryParseFilter(filterString);

        Assert.True(result);
        Assert.Equal("Created", filter.FilterName);
        Assert.Null(filter.Lower);
        Assert.NotNull(filter.Upper);
        Assert.Equal(2023, filter.Upper.Value.Year);
        Assert.Equal(inclusive, filter.UpperInclusive);

    }

    [Theory]
    [InlineData(":[2022,2023]")]
    [InlineData("Created[2022,2023]")]
    [InlineData("[2022,2023]")]
    [InlineData("Created:2023]")]
    [InlineData("Created:[2023")]
    [InlineData("Created:[2023]")]
    [InlineData("Created:[,]")]
    [InlineData("Created:[2022,2023,2024]")]
    public void FailedParseFilter(string filterString)
    {
        var filter = new DateTimeFilterBuilder();
        
        var result = filter.TryParseFilter(filterString);

        Assert.False(result);
        Assert.Equal(string.Empty, filter.FilterName);
        Assert.Null(filter.Lower);
        Assert.Null(filter.Upper);
    }
}
