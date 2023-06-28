namespace ExtraDry.Server.Tests.Rules;

public class EntityFrameworkTests {

    [Fact]
    public async Task HardDeleteUserWithoutAddress() {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var user = database.Users.First(e => e.Name == "Homeless");

        await rules.TryHardDeleteAsync(user, () => database.Remove(user), async () => await MockSaveChangesAsync(database));

        Assert.Equal(1, database.Users.Count());
    }

    [Fact]
    public async Task HardDeleteUserWithAddress()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var user = database.Users.First(e => e.Name == "Homebody");

        await rules.TryHardDeleteAsync(user, () => database.Remove(user), async () => await MockSaveChangesAsync(database));

        Assert.Equal(1, database.Users.Count());
    }

    [Fact]
    public async Task HardDeleteAddressUserSoftDelete()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());
        var address = database.Addresses.First(e => e.Line == "123 Any Street");

        var result = await rules.DeleteAsync(address, () => database.Remove(address), async () => await MockSaveChangesAsync(database));

        Assert.Equal(DeleteResult.SoftDeleted, result);
        Assert.Equal(2, database.Addresses.Count());
    }

    [Fact]
    public async Task HardDeleteAddressUsingRegister()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());

        var address = database.Addresses.First(e => e.Line == "123 Any Street");

        rules.RegisterRemove<Address>(e => database.Remove(e));
        rules.RegisterCommit(() => MockSaveChangesAsync(database));

        var result = await rules.TryHardDeleteAsync(address);

        Assert.Equal(DeleteResult.NotDeleted, result);
        Assert.Equal(2, database.Addresses.Count());
    }

    [Fact]
    public async Task DeleteAddressWithLinkedUserSoftDeletes()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());

        var address = database.Addresses.First(e => e.Line == "123 Any Street");

        rules.RegisterRemove<Address>(e => database.Remove(e));
        rules.RegisterCommit(() => MockSaveChangesAsync(database));

        var result = await rules.DeleteAsync(address);

        Assert.Equal(DeleteResult.SoftDeleted, result);
        Assert.Equal(2, database.Addresses.Count());
    }

    [Fact]
    public async Task HardDeleteMutipleEntitiesMissingRemoveThrowsException()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());

        var address = database.Addresses.First(e => e.Line == "123 Any Street");
        var user = database.Users.First(e => e.Name == "Homebody");

        rules.RegisterRemove<Address>(e => database.Remove(e));
        rules.RegisterCommit(() => MockSaveChangesAsync(database));

        await Assert.ThrowsAsync<DryException>(() => rules.TryHardDeleteAsync(user, address));
    }


    [Fact]
    public async Task HardDeleteMutipleEntities()
    {
        var database = GetPopulatedDatabase();
        var rules = new RuleEngine(new ServiceProviderStub());

        var address = database.Addresses.First(e => e.Line == "Vacant");
        var user = database.Users.First(e => e.Name == "Homebody");

        rules.RegisterRemove<Address>(e => database.Remove(e));
        rules.RegisterRemove<User>(e => database.Remove(e));

        rules.RegisterCommit(() => MockSaveChangesAsync(database));

        var result = await rules.TryHardDeleteAsync(user, address);

        Assert.Equal(DeleteResult.HardDeleted, result);
        Assert.Equal(1, database.Addresses.Count());
    }

    private static async Task MockSaveChangesAsync(TestContext database)
    {
        //Address being removed
        var removingAddress = database.ChangeTracker.Entries()
            .Where(e => e.Entity is Address && e.State == EntityState.Deleted)
            .Select(e => e.Entity as Address)
            .ToList();

        //User being removed
        var removingUser = database.ChangeTracker.Entries()
            .Where(e => e.Entity is User && e.State == EntityState.Deleted)
            .Select(e => e.Entity as User)
            .ToList();

        //If an Address that has a linked User is being deleted, but not the User throw an Exception
        var riViolationUser = database.Users.Any(user => removingAddress.Any(address => user.Address == address) && user != removingUser.FirstOrDefault());
        if(riViolationUser) {
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
