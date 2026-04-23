namespace ExtraDry.Server.Tests.Internals;

public class FilterQuerySortTests
{
    [Fact]
    public void DefaultValues()
    {
        var query = ValidFilterQuery;

        Assert.Null(query.Filter);
        Assert.Null(query.Sort);
    }

    [Theory]
    [InlineData("Filter", null)]
    [InlineData("Filter", "not")]
    [InlineData("Sort", null)]
    [InlineData("Sort", "not")]
    public void RoundtripProperties(string propertyName, object? propertyValue)
    {
        var filter = ValidFilterQuery;
        var property = filter.GetType().GetProperty(propertyName);

        property?.SetValue(filter, propertyValue);
        var result = property?.GetValue(filter);

        Assert.Equal(propertyValue, result);
    }

    private static FilterQuery ValidFilterQuery => new();
}
