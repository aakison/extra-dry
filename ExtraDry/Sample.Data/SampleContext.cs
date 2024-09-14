using ExtraDry.Server.EF;
using System.Text.Json;

namespace Sample.Data;

public class SampleContext(
    DbContextOptions<SampleContext> options,
    IEnumerable<IDbAspect> aspects) 
    : AspectDbContext(options, aspects) 
{

    public DbSet<Sector> Sectors { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Company> Companies { get; set; } = null!;

    public DbSet<Content> Contents { get; set; } = null!;

    public DbSet<Region> Regions { get; set; } = null!;

    public DbSet<Template> Templates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().Property(e => e.BankingDetails).HasConversion(
            e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
            e => JsonSerializer.Deserialize<BankingDetails>(e, (JsonSerializerOptions?)null) ?? new());

        modelBuilder.Entity<Company>().Property(e => e.CustomFields).HasJsonConversion();

        modelBuilder.Entity<Content>().Property(e => e.Layout).HasConversion(
            e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
            e => JsonSerializer.Deserialize<ContentLayout>(e, (JsonSerializerOptions?)null) ?? new());

        //modelBuilder.Entity<Content>().Property(e => e.Layout).HasColumnType()

        modelBuilder.Entity<Company>().OwnsOne(e => e.Version);
        modelBuilder.Entity<Employee>().OwnsOne(e => e.Version);
        modelBuilder.Entity<Employee>().OwnsOne(e => e.Revision);
        modelBuilder.Entity<Region>().OwnsOne(e => e.Version);
        modelBuilder.Entity<Sector>().OwnsOne(e => e.Version);
        modelBuilder.Entity<Content>().OwnsOne(e => e.Version);
        modelBuilder.Entity<Template>().OwnsOne(e => e.Version);

        modelBuilder.Entity<Template>().Property(e => e.Schema).HasConversion(
            e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
            e => JsonSerializer.Deserialize<ExpandoSchema>(e, (JsonSerializerOptions?)null) ?? new());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new ImproveHierarchyQueryPerformance());
    }
}
