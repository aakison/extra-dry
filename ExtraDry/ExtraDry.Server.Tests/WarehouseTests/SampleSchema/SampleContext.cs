using ExtraDry.Server.EF;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class SampleContext : AspectDbContext {

    public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; } = null!;

    public DbSet<BlobInfo> Blobs { get; set; } = null!;

    public DbSet<Region> Regions { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().Property(e => e.BankingDetails).HasConversion(
            e => JsonSerializer.Serialize(e, (JsonSerializerOptions?)null),
            e => JsonSerializer.Deserialize<BankingDetails>(e, (JsonSerializerOptions?)null) ?? new());

    }
}
