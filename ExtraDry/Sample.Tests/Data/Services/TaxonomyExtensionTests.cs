// For testing Sample functionality, not ExtraDry functionality. Ignored tests causes warnings in
// ExtraDry, so exclude. Only return if some ExtraDry functionality for re-parenting is created.

//using ExtraDry.Server;
//using Microsoft.EntityFrameworkCore;
//using Sample.Data;
//using Sample.Data.Services;
//using System.ComponentModel;

//namespace Sample.Tests.Data.Services;

//public class TaxonomyExtensionTests {
//    // This cannot run on an in-memory database becuase it needs to run relational queries for subtree moving
//    // As a result we ignore unles it's specifically asked for
//    // To have these tests run again, set this constant to null;
//    const string IgnoreTests = "Destructive tests disabled";

// private RegionService regions;

// private SampleContext context;

// public TaxonomyExtensionTests() { var builder = new DbContextOptionsBuilder<SampleContext>()
// .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExtraDryIntegrationTests;Trusted_Connection=True;",
// opt => opt.UseHierarchyId()).Options; var arrangeContext = new SampleContext(builder); context =
// new SampleContext(builder);

// var rules = new RuleEngine(new ServiceProviderStub(), new ExtraDryOptions());

// regions = new RegionService(arrangeContext, rules); regions = new RegionService(context, rules);
// ClearData(); SeedSampleData(regions).Wait(); }

// //[Fact(Skip = IgnoreTests)] //public async Task AncestorsCountIsCorrect() //{ // var mel =
// await arrangeService.TryRetrieveAsync("AU-VIC-Melbourne");

// // Assert.Equal(4, mel.Ancestor.Count); // Self, Victoria, Australia, Global //}

// //[Fact(Skip = IgnoreTests)] //public async Task MoveLeafNode() //{ // var nsw = await
// arrangeService.TryRetrieveAsync("AU-NSW"); // var nz = await
// arrangeService.TryRetrieveAsync("NZ"); // nsw.Parent = nz;

// // await actService.UpdateAsync("AU-NSW", nsw, true);

// // nsw = await actService.TryRetrieveAsync("AU-NSW"); // Assert.Equal("NZ", nsw.Parent.Slug); //
// Assert.DoesNotContain(nsw.Ancestors, a => a.Slug == "AU"); //}

// [Fact(Skip = IgnoreTests)] public async Task MoveLeafNodeIgnoredWithParameter() { var nsw =
// await regions.TryRetrieveAsync("AU-NSW"); var nz = await regions.TryRetrieveAsync("NZ");
// Assert.NotNull(nsw); nsw.Parent = nz; nsw.Description = "Value Changed";
// context.ChangeTracker.Clear(); // so that changes aren't tracked and we can emulate a call
// coming from the API. Otherwise it passes because of the EF cache

// await regions.UpdateAsync("AU-NSW", nsw);

// nsw = await regions.TryRetrieveAsync("AU-NSW"); Assert.NotNull(nsw?.Parent); Assert.Equal("AU",
// nsw.Parent.Slug); Assert.Equal("Value Changed", nsw.Description); }

// [Fact(Skip = IgnoreTests)] public async Task MoveLeafNodeToDifferentStrata() { var nsw = await
// regions.TryRetrieveAsync("AU-NSW"); Assert.NotNull(nsw?.Parent); var cnt = nsw.Parent;
// context.ChangeTracker.Clear(); // so that changes aren't tracked and we can emulate a call
// coming from the API. Otherwise it passes because of the EF cache var auk = await
// regions.TryRetrieveAsync("NZ-AUK"); nsw.Parent = auk;

// await Assert.ThrowsAsync<ArgumentException>(async () => await regions.UpdateAsync("AU-NSW", nsw,
// true)); }

// [Fact(Skip = IgnoreTests)] public async Task MoveSubtree() { var qld = await
// regions.TryRetrieveAsync("AU-QLD"); var nz = await regions.TryRetrieveAsync("NZ");
// Assert.NotNull(qld?.Parent); qld.Parent = nz;

// await regions.UpdateAsync("AU-QLD", qld, true);

// var qld2 = await regions.TryRetrieveAsync("AU-QLD"); Assert.NotNull(qld2?.Parent);
// Assert.Equal("NZ", qld2.Parent.Slug); var bris = await
// regions.TryRetrieveAsync("AU-Qld-Brisbane"); Assert.NotNull(bris?.Parent);
// Assert.Equal("AU-QLD", bris.Parent.Slug); }

// [Fact(Skip = IgnoreTests)] public async Task MoveSubtreeToDifferentStrata() { var qld = await
// regions.TryRetrieveAsync("AU-QLD"); var nz = await regions.TryRetrieveAsync("NZ-AUK");
// Assert.NotNull(qld); qld.Parent = nz; context.ChangeTracker.Clear(); // so that changes aren't
// tracked and we can emulate a call coming from the API.

// await Assert.ThrowsAsync<ArgumentException>(async () => await regions.UpdateAsync("AU-QLD", qld,
// true)); }

// [Fact(Skip = IgnoreTests)] public async Task MoveSubtreeWithAdditionalChanges() { var qld =
// await regions.TryRetrieveAsync("AU-QLD"); var nz = await regions.TryRetrieveAsync("NZ");
// Assert.NotNull(qld); qld.Parent = nz; qld.Title = "TestTitle";

// // Should do the move, then the change and not fail. await regions.UpdateAsync("AU-QLD", qld,
// true);

// qld = await regions.TryRetrieveAsync("AU-QLD"); Assert.NotNull(qld?.Parent); Assert.Equal("NZ",
// qld.Parent.Slug); var bris = await regions.TryRetrieveAsync("AU-Qld-Brisbane");
// Assert.NotNull(bris?.Parent); Assert.Equal("AU-QLD", bris.Parent.Slug);
// Assert.Equal("TestTitle", qld.Title); }

// private void ClearData() { context.Database.ExecuteSqlRaw("DELETE FROM [Regions]"); }

//    private async Task SeedSampleData(RegionService service)
//    {
//        var seed = new DummyData();
//        await seed.PopulateRegionsAsync(service);
//    }
//}
