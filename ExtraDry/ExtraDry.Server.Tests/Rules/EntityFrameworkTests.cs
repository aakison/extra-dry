namespace ExtraDry.Server.Tests.Rules;

public class EntityFrameworkTests {

    [Fact]
    public async Task HardDeleteUserWithoutAddress() {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var user = database.Users.First(e => e.Name == "Homeless");

        await rules.DeleteHardAsync(user, () => database.Remove(user), async () => await MockSaveChangesAsync(database));

        Assert.Equal(1, database.Users.Count());
    }

    [Fact]
    public async Task HardDeleteUserWithAddress()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var user = database.Users.First(e => e.Name == "Homebody");

        await rules.DeleteHardAsync(user, () => database.Remove(user), async () => await MockSaveChangesAsync(database));

        Assert.Equal(1, database.Users.Count());
    }

    [Fact]
    public async Task HardDeleteAddressUserSoftDelete()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var address = database.Addresses.First(e => e.Line == "123 Any Street");

        var result = await rules.DeleteHardAsync(address, () => database.Remove(address), async () => await MockSaveChangesAsync(database));

        Assert.Equal(DeleteResult.SoftDeleted, result);
        Assert.Equal(2, database.Addresses.Count());
    }


    private static async Task MockSaveChangesAsync(TestContext database)
    {
        var removing = database.ChangeTracker.Entries()
            .Where(e => e.Entity is Address && e.State == EntityState.Deleted)
            .Select(e => e.Entity as Address)
            .ToList();
        var riViolation = database.Users.Any(user => removing.Any(address => user.Address == address));
        if(riViolation) {
            throw new InvalidOperationException("Referential integrity violated, can't delete address when user referencing it.");
        }
        await database.SaveChangesAsync();
    }


    private static TestContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<TestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TestContext(options);
    }

    private static void PopulateSampleData(TestContext database)
    {
        database.Users.Add(new User { Name = "Homeless" });
        var vacant = new Address { Line = "Vacant" };
        database.Addresses.Add(vacant);
        var address = new Address { Line = "123 Any Street" };
        database.Addresses.Add(address);
        var user = new User { Name = "Homebody", Address = address };
        database.Users.Add(user);
        database.SaveChangesAsync();
    }

    private static TestContext GetPopulatedDatabase()
    {
        var database = GetDatabase();
        PopulateSampleData(database);
        return database;
    }

}
