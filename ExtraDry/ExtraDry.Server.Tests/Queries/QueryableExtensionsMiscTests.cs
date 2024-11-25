namespace ExtraDry.Server.Tests;

public class QueryableExtensionsMiscTests {

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void QueryableFilterNull(string? filter)
    {
        var models = Samples.Models;
        var queryable = models.AsQueryable();

        var filtered = queryable.Filter(filter);

        Assert.Equal(queryable, filtered);
    }

    [Fact]
    public void UnfilterableModelNotFiltered()
    {
        var queryable = UnfilterableModels.AsQueryable();

        var filtered = queryable.Filter("One"); 

        Assert.Equal(queryable, filtered);
    }

    public class Unfilterable {
        public string Name { get; set; } = string.Empty;
    }

    public static List<Unfilterable> UnfilterableModels => [
            new Unfilterable { Name = "One" },
            new Unfilterable { Name = "Two" },
        ];

}
