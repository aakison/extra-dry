namespace ExtraDry.Server.Tests.Internals;

public class SortQueryTests {

    [Fact]
    public void DefaultValues()
    {
        var sortQuery = ValidSortQuery;

        Assert.Null(sortQuery.Filter);
        Assert.Null(sortQuery.Sort);
    }

    [Theory]
    [InlineData("Filter", null)]
    [InlineData("Filter", "not")]
    [InlineData("Sort", null)]
    [InlineData("Sort", "not")]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var filter = ValidSortQuery;
        var property = filter.GetType().GetProperty(propertyName);

        property?.SetValue(filter, propertyValue);
        var result = property?.GetValue(filter);

        Assert.Equal(propertyValue, result);
    }

    private static SortQuery ValidSortQuery => new();

}
