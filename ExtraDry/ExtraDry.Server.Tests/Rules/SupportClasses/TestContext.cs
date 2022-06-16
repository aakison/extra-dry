namespace ExtraDry.Server.Tests.Rules;

public class TestContext : DbContext {

    public TestContext(DbContextOptions<TestContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Address> Addresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
