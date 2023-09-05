using ExtraDry.Server;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Sample.Data.Services;
using System.ComponentModel;

namespace Sample.Tests.Data.Services;

public class TaxonomyExtensionTests {

    // This cannot run on an in-memory database becuase it needs to run relational queries for subtree moving
    // As a result we ignore unles it's specifically asked for
    // To have these tests run again, set this constant to null;
    const string IgnoreTests = "Destructive tests disabled";

    // 2 services. This is a workaround so the context in one doesn't actively influence the other through tracking.
    // The alternative is to fetch items as no tracking, but that would influence the production system for a non-realistic situation.
    private RegionService arrangeService;
    private RegionService actService;

    private SampleContext context;

    public TaxonomyExtensionTests()
    {
        var builder = new DbContextOptionsBuilder<SampleContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExtraDryIntegrationTests;Trusted_Connection=True;").Options;
        var arrangeContext = new SampleContext(builder);
        context = new SampleContext(builder);

        var rules = new RuleEngine(new ServiceProviderStub());

        arrangeService = new RegionService(arrangeContext, rules);
        actService = new RegionService(context, rules);
        ClearData();
        SeedSampleData(arrangeService).Wait();
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveLeafNode()
    {
        var nsw = await arrangeService.TryRetrieveAsync("AU-NSW");
        var nz = await arrangeService.TryRetrieveAsync("NZ");
        nsw.Parent = nz;

        await actService.UpdateAsync("AU-NSW", nsw);

        nsw = await arrangeService.TryRetrieveAsync("AU-NSW");
        Assert.Equal("NZ", nsw.Parent.Slug);
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveLeafNodeToDifferentStrata()
    {
        var nsw = await arrangeService.TryRetrieveAsync("AU-NSW");
        var auk = await arrangeService.TryRetrieveAsync("NZ-AUK");
        nsw.Parent = auk;

        await Assert.ThrowsAsync<ArgumentException>(async () => await actService.UpdateAsync("AU-NSW", nsw));
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveSubtree()
    {
        var qld = await arrangeService.TryRetrieveAsync("AU-QLD");
        var nz = await arrangeService.TryRetrieveAsync("NZ");
        qld.Parent = nz;

        await actService.UpdateAsync("AU-QLD", qld);

        qld = await arrangeService.TryRetrieveAsync("AU-QLD");
        Assert.Equal("NZ", qld.Parent.Slug);
        var bris = await arrangeService.TryRetrieveAsync("AU-Qld-Brisbane");
        Assert.Contains(bris.Ancestors, e => e.Slug == "AU-QLD");
        Assert.Contains(bris.Ancestors, e => e.Slug == "NZ");
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveSubtreeToDifferentStrata()
    {
        var qld = await arrangeService.TryRetrieveAsync("AU-QLD");
        var nz = await arrangeService.TryRetrieveAsync("NZ-AUK");
        qld.Parent = nz;

        await Assert.ThrowsAsync<ArgumentException>(async () => await actService.UpdateAsync("AU-QLD", qld));
    }

    private void ClearData()
    {
        context.Database.ExecuteSqlRaw("DELETE FROM [RegionRegion]");
        context.Database.ExecuteSqlRaw("DELETE FROM [Regions]");
    }

    private async Task SeedSampleData(RegionService service)
    {
        var seed = new DummyData();
        await seed.PopulateRegions(service);
    }
}
