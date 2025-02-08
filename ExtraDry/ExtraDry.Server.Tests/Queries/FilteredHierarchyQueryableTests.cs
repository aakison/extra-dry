using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class FilteredHierarchyQueryableTests
{
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
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 13)]
    [InlineData(4, 16)]
    public void HierarchyLevels(int level, int count)
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = level };
        var expected = Samples.Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() < level)
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
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 13)]
    [InlineData(4, 16)]
    public async Task HierarchyLevelsAsync(int level, int count)
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = level };
        var expected = Samples.Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() < level)
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
        var query = new HierarchyQuery { Level = 2, Expand = expand };
        var slugs = new List<string> { "all", "AU", "NZ", "NZ-AUK", "NZ-TKI" };
        var expected = Samples.Regions
            .Where(e => slugs.Contains(e.Slug))
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(2, actual.Level);
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
        var query = new HierarchyQuery { Level = 3, Collapse = collapse };
        var slugs = new List<string> { "all", "AU", "NZ", "NZ-AUK", "NZ-TKI" };
        var expected = Samples.Regions
            .Where(e => slugs.Contains(e.Slug))
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Null(actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(3, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Equal(collapse, actual.Collapse);
        Assert.Equal(5, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task FilterAndParentsIgnoreLevel()
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = 4, Filter = "Brisbane" };
        var slugs = new List<string> { "all", "AU", "AU-QLD", "AU-QLD-Brisbane" };
        var expected = Samples.Regions
            .Where(e => slugs.Contains(e.Slug))
            .OrderBy(e => e.Lineage);

        var actual = await Samples.Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Equal("Brisbane", actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(4, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(4, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task SupportCaseInsensitiveForInstructions()
    {
        // filter to get rid of duplicate names
        var query = new HierarchyQuery { Level = 4, Filter = "brisbane", Comparison = StringComparison.OrdinalIgnoreCase };
        var regions = Samples.Regions;
        var slugs = new List<string> { "all", "AU", "AU-QLD", "AU-QLD-Brisbane" };
        var expected = Samples.Regions
            .Where(e => slugs.Contains(e.Slug))
            .OrderBy(e => e.Lineage);

        var actual = await regions.AsQueryable()
            .QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Equal("brisbane", actual.Filter);
        Assert.Equal("lineage", actual.Sort);
        Assert.Equal(4, actual.Level);
        Assert.Null(actual.Expand);
        Assert.Null(actual.Collapse);
        Assert.Equal(4, actual.Count);
    }
}
