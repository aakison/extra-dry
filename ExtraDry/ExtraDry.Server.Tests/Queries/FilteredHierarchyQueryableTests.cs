using ExtraDry.Server.Internal;
using ExtraDry.Server.Tests.WarehouseTests;
using Microsoft.Extensions.Azure;

namespace ExtraDry.Server.Tests.Models;

public class FilteredHierarchyQueryableTests {

    [Fact]
    public void QueryableInterfacePublished()
    {
        var query = new HierarchyQuery();

        var queryable = Regions.AsQueryable().QueryWith(query);

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
        var expected = Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage);

        var actual = Regions.AsQueryable().QueryWith(query).ToHierarchyCollection();

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
        var expected = Regions
            .Where(e => e.Lineage.ToString().Split("/").Where(e => !string.IsNullOrEmpty(e)).Count() <= level)
            .OrderBy(e => e.Lineage);

        var actual = await Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

        Assert.Equal(count, actual.Count);
        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task LevelAndExpand()
    {
        // filter to get rid of duplicate names
        var expand = new List<string> { "NZ" };
        var query = new HierarchyQuery { Level = 1, Expand = expand };
        var expected = Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug.Contains("NZ"))
            .OrderBy(e => e.Lineage);

        var actual = await Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

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
        var expected = Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug.Contains("NZ"))
            .OrderBy(e => e.Lineage);

        var actual = await Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

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
        var expected = Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug == "AU-QLD" || e.Slug == "AU-QLD-Brisbane")
            .OrderBy(e => e.Lineage);

        var actual = await Regions.AsQueryable().QueryWith(query).ToHierarchyCollectionAsync();

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
        var expected = Regions
            .Where(e => e.Slug == "all" || e.Slug == "AU" || e.Slug == "AU-QLD" || e.Slug == "AU-QLD-Brisbane")
            .OrderBy(e => e.Lineage);

        var actual = await Regions.AsQueryable()
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

    private List<Region>? regions;
    private List<Region> Regions {
        get {
            if(regions == null) {
                // Tier 1
                var allRegions = new Region { Uuid = Guid.NewGuid(), Slug = "all", Title = "All Regions", Description = "The World", Level = RegionLevel.Global, Lineage = HierarchyId.Parse("/") };

                var auRegion = new Region { Parent = allRegions, Slug = "AU", Title = "Australia", Description = "Australia", Level = RegionLevel.Country, Lineage = HierarchyId.Parse("/1/") };
                var nzRegion = new Region { Parent = allRegions, Slug = "NZ", Title = "New Zealand", Description = "New Zealand", Level = RegionLevel.Country, Lineage = HierarchyId.Parse("/2/") };

                // Tier 2
                var vicRegion = new Region { Parent = auRegion, Slug = "AU-VIC", Title = "Victoria", Description = "Victoria, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/1/") };
                var qldRegion = new Region { Parent = auRegion, Slug = "AU-QLD", Title = "Queensland", Description = "Queensland, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/2/") };
                var nswRegion = new Region { Parent = auRegion, Slug = "AU-NSW", Title = "New South Wales", Description = "NSW, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/3/") };
                var actRegion = new Region { Parent = auRegion, Slug = "AU-ACT", Title = "Canberra", Description = "Australian Capital Territory", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/4/") };
                var tasRegion = new Region { Parent = auRegion, Slug = "AU-TAS", Title = "Tasmania", Description = "Tasmania", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/5/") };
                var saRegion = new Region { Parent = auRegion, Slug = "AU-SA", Title = "South Australia", Description = "South Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/6/") };
                var ntRegion = new Region { Parent = auRegion, Slug = "AU-NT", Title = "Northern Territory", Description = "Northern Territory", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/7/") };
                var waRegion = new Region { Parent = auRegion, Slug = "AU-WA", Title = "Western Australia", Description = "Western Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/8/") };

                var aukRegion = new Region { Parent = nzRegion, Slug = "NZ-AUK", Title = "Auckland", Description = "Auckland, NZ", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/2/1/") };
                var tkiRegion = new Region { Parent = nzRegion, Slug = "NZ-TKI", Title = "Taranaki", Description = "Taranaki, NZ", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/2/2/") };

                // Tier 3
                var melbRegion = new Region { Parent = vicRegion, Slug = "AU-VIC-Melbourne", Title = "Melbourne City", Description = "Melbourne, Victoria, Australia", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/1/1/") };
                var brisRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Brisbane", Title = "Brisbane", Description = "Brisbane", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/2/1/") };
                var redRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Redlands", Title = "Redlands", Description = "City of Redlands", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/2/2/") };

                regions = new List<Region> { allRegions, auRegion, nzRegion, vicRegion, qldRegion, nswRegion, actRegion, tasRegion, saRegion, ntRegion, waRegion, aukRegion, tkiRegion, melbRegion, brisRegion, redRegion };
            }
            return regions;
        }
    }

}
