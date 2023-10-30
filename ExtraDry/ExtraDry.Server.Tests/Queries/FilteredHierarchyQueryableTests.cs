using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class FilteredHierarchyQueryableTests {

    [Fact]
    public void QueryableInterfacePublished()
    {
        var query = new HierarchyQuery();

        var queryable = Samples.Regions.AsQueryable().QueryWith(query);

        Assert.NotNull(queryable.ElementType);
        Assert.NotNull(queryable.Expression);
        Assert.NotNull(queryable.Provider);
        Assert.NotNull(queryable.GetEnumerator());
        Assert.NotNull(((System.Collections.IEnumerable)queryable).GetEnumerator());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 3)]
    [InlineData(2, 13)]
    [InlineData(3, 16)] 
    public void HierarchyLevels(int level, int count)
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = level };
        var expected = Samples.Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage);

        var actual = Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollection();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(level, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(count, actual.Count);
        Assert.Equal(expected, actual.Items); 
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 3)]
    [InlineData(2, 13)]
    [InlineData(3, 16)]
    public async Task HierarchyLevelsAsync(int level, int count)
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = level };
        var expected = Samples.Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(level, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(count, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task LevelAndExpand()
    {
        // filter to get rid of duplicate names
        var expand = new List<string> { "NZ" };
        var query = new HierarchyQuery { Level = 1, Expand = expand };
        var expected = Samples.Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug.Contains("NZ"))
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(1, actual.Level);
        Assert.Equal(expand, actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(5, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task LevelAndCollapse()
    {
        // filter to get rid of duplicate names
        var collapse = new List<string> { "AU" };
        var query = new HierarchyQuery { Level = 2, Collapse = collapse };
        var expected = Samples.Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug.Contains("NZ"))
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(2, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Equal(collapse, actual.Collapse);
        Assert.Equal(5, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task FilterAndParentsIgnoreLevel()
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = 3, Filter = "Brisbane" };
        var expected = Samples.Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug == "AU-QLD" || e.Slug == "AU-QLD-Brisbane")
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Equal("Brisbane", actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(3, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(4, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task SupportCaseInsensitiveForInstructions()
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = 3, Filter = "brisbane" };
        var regions = Samples.Regions;
        var expected = regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug == "AU-QLD" || e.Slug == "AU-QLD-Brisbane")
            .OrderBy(e => e.Lineage);

        var actual = await regions.AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Equal("brisbane", actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(3, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(4, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

}
