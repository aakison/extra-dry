using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Models;

public class PagedHierarchyQueryableTests {

    [Fact]
    public void QueryableInterfacePublished()
    {
        var query = new PageHierarchyQuery();

        var queryable = Samples.Regions.AsQueryable().QueryWith(query);

        Assert.NotNull(queryable.ElementType);
        Assert.NotNull(queryable.Expression);
        Assert.NotNull(queryable.Provider);
        Assert.NotNull(queryable.GetEnumerator());
        Assert.NotNull(((System.Collections.IEnumerable)queryable).GetEnumerator());
    }

    [Theory]
    [InlineData(0, 0, 5, 1)]
    [InlineData(1, 0, 5, 3)]
    [InlineData(2, 0, 5, 5)] // 13 total
    [InlineData(2, 5, 5, 5)] // 13 total
    [InlineData(2, 10, 5, 3)] // 13 total
    [InlineData(3, 10, 10, 6)] // 16 total
    [InlineData(3, 0, 0, 16)] // 16 total, use default take
    public void HierarchyLevels(int level, int skip, int take, int count)
    {
        // filter to get rid of duplicate names
        var query = new PageHierarchyQuery { Level = level, Skip = skip, Take = take };
        var regions = Samples.Regions;
        var expected = regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage)
            .Skip(query.Skip).Take(query.Take); // make sure from Query for default override.

        var actual = regions.AsQueryable().QueryWith(query).ToPagedHierarchyCollection();

        Assert.Equal(count, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData(0, 0, 5, 1)]
    [InlineData(1, 0, 5, 3)]
    [InlineData(2, 0, 5, 5)] // 13 total
    [InlineData(2, 5, 5, 5)] // 13 total
    [InlineData(2, 10, 5, 3)] // 13 total
    [InlineData(3, 10, 10, 6)] // 16 total
    public async Task HierarchyLevelsAsync(int level, int skip, int take, int count)
    {
        // filter to get rid of duplicate names
        var query = new PageHierarchyQuery { Level = level, Skip = skip, Take = take };
        var regions = Samples.Regions;
        var expected = regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage)
            .Skip(skip).Take(take);

        var actual = await regions.AsQueryable().QueryWith(query).ToPagedHierarchyCollectionAsync();

        Assert.Equal(count, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public void ToHierarchyIgnoresPaging()
    {
        // filter to get rid of duplicate names
        int level = 3;
        var query = new PageHierarchyQuery { Level = level, Skip = 5, Take = 5 };
        var regions = Samples.Regions;
        var expected = regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage);

        var actual = regions.AsQueryable().QueryWith(query).ToHierarchyCollection();

        Assert.Equal(16, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

}
