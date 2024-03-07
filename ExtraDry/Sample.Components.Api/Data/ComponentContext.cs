using ExtraDry.Server.EF;
using Microsoft.EntityFrameworkCore;

namespace Sample.Components.Api.Data;

public class ComponentContext(
    DbContextOptions<ComponentContext> options) 
    : AspectDbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; } = null!;

    public DbSet<Component> Components { get; set; } = null!;

    public DbSet<Attachment> Attachments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultContainer("Components");
        modelBuilder.Entity<Tenant>().HasPartitionKey(e => e.Partition);
        modelBuilder.Entity<Component>().HasPartitionKey(e => e.Partition);
        modelBuilder.Entity<Attachment>().HasPartitionKey(e => e.Partition);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}
