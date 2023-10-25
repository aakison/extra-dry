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
    const string IgnoreTests = null;//"Destructive tests disabled";

    // 2 services. This is a workaround so the context in one doesn't actively influence the other through tracking.
    // The alternative is to fetch items as no tracking, but that would influence the production system for a non-realistic situation.
    private RegionService regions;

    private SampleContext context;

    public TaxonomyExtensionTests()
    {
        var builder = new DbContextOptionsBuilder<SampleContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExtraDryIntegrationTests;Trusted_Connection=True;", opt => opt.UseHierarchyId()).Options;
        var arrangeContext = new SampleContext(builder);
        context = new SampleContext(builder);

        var rules = new RuleEngine(new ServiceProviderStub());

        regions = new RegionService(arrangeContext, rules);
        regions = new RegionService(context, rules);
        ClearData();
        SeedSampleData(regions).Wait();
    }

    //[Fact(Skip = IgnoreTests)]
    //public async Task AncestorsCountIsCorrect()
    //{
    //    var mel = await arrangeService.TryRetrieveAsync("AU-VIC-Melbourne");

    //    Assert.Equal(4, mel.Ancestor.Count); // Self, Victoria, Australia, Global
    //}

    //[Fact(Skip = IgnoreTests)]
    //public async Task MoveLeafNode()
    //{
    //    var nsw = await arrangeService.TryRetrieveAsync("AU-NSW");
    //    var nz = await arrangeService.TryRetrieveAsync("NZ");
    //    nsw.Parent = nz;

    //    await actService.UpdateAsync("AU-NSW", nsw, true);

    //    nsw = await actService.TryRetrieveAsync("AU-NSW");
    //    Assert.Equal("NZ", nsw.Parent.Slug);
    //    Assert.DoesNotContain(nsw.Ancestors, a => a.Slug == "AU");
    //}

    [Fact(Skip = IgnoreTests)]
    public async Task MoveLeafNodeIgnoredWithParameter()
    {
        var nsw = await regions.TryRetrieveAsync("AU-NSW");
        var nz = await regions.TryRetrieveAsync("NZ");
        nsw.Parent = nz;
        nsw.Description = "Value Changed";
        context.ChangeTracker.Clear(); // so that changes aren't tracked and we can emulate a call coming from the API.

        await regions.UpdateAsync("AU-NSW", nsw);

        nsw = await regions.TryRetrieveAsync("AU-NSW");
        Assert.Equal("AU", nsw.Parent.Slug);
        Assert.Equal("Value Changed", nsw.Description);
        //Assert.DoesNotContain(nsw.Ancestors, a => a.Slug == "NZ");
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveLeafNodeToDifferentStrata()
    {
        var nsw = await regions.TryRetrieveAsync("AU-NSW");
        var cnt = nsw.Parent;
        context.ChangeTracker.Clear(); // so that changes aren't tracked and we can emulate a call coming from the API.
        var auk = await regions.TryRetrieveAsync("NZ-AUK");
        nsw.Parent = auk;

        await Assert.ThrowsAsync<ArgumentException>(async () => await regions.UpdateAsync("AU-NSW", nsw, true));
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveSubtree()
    {
        var qld = await regions.TryRetrieveAsync("AU-QLD");
        var nz = await regions.TryRetrieveAsync("NZ");
        qld.Parent = nz;

        await regions.UpdateAsync("AU-QLD", qld, true);

        var qld2 = await regions.TryRetrieveAsync("AU-QLD");
        Assert.Equal("NZ", qld2.Parent.Slug);
        var bris = await regions.TryRetrieveAsync("AU-Qld-Brisbane");
        //Assert.Contains(bris.Ancestors, e => e.Slug == "AU-QLD");
        //Assert.Contains(bris.Ancestors, e => e.Slug == "NZ");
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveSubtreeToDifferentStrata()
    {
        var qld = await regions.TryRetrieveAsync("AU-QLD");
        var nz = await regions.TryRetrieveAsync("NZ-AUK");
        qld.Parent = nz;
        context.ChangeTracker.Clear(); // so that changes aren't tracked and we can emulate a call coming from the API.

        await Assert.ThrowsAsync<ArgumentException>(async () => await regions.UpdateAsync("AU-QLD", qld, true));
    }

    [Fact(Skip = IgnoreTests)]
    public async Task MoveSubtreeWithAdditionalChanges()
    {
        var qld = await regions.TryRetrieveAsync("AU-QLD");
        var nz = await regions.TryRetrieveAsync("NZ");
        qld.Parent = nz;
        qld.Title = "TestTitle";

        // Should do the move, then the change and not fail.
        await regions.UpdateAsync("AU-QLD", qld, true);

        qld = await regions.TryRetrieveAsync("AU-QLD");
        Assert.Equal("NZ", qld.Parent.Slug);
        var bris = await regions.TryRetrieveAsync("AU-Qld-Brisbane");
        //Assert.Contains(bris.Ancestors, e => e.Slug == "AU-QLD");
        //Assert.Contains(bris.Ancestors, e => e.Slug == "NZ");
        Assert.Equal("TestTitle", qld.Title);
    }

    private void ClearData()
    {
        context.Database.ExecuteSqlRaw("DELETE FROM [Regions]");
    }

    private async Task SeedSampleData(RegionService service)
    {
        var seed = new DummyData();
        await seed.PopulateRegionsAsync(service);
    }
}
