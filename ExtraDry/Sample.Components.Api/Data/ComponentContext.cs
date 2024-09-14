using ExtraDry.Server.EF;
using Microsoft.EntityFrameworkCore;

namespace Sample.Components.Api.Data;

/// <summary>
/// Entity framework context for the components, implemented around CosmosDB.
/// </summary>
/// <param name="options"></param>
public class ComponentContext(
    DbContextOptions<ComponentContext> options,
    IEnumerable<IDbAspect> aspects) 
    : AspectDbContext(options, aspects)
{

    /// <summary>
    /// The Tenants in the system.
    /// </summary>
    public DbSet<Tenant> Tenants { get; set; } = null!;

    /// <summary>
    /// The Components for Tenants in the system.
    /// </summary>
    public DbSet<Component> Components { get; set; } = null!;

    /// <summary>
    /// The Metadata for the Components in the system.
    /// </summary>
    //public DbSet<Metadata> Metadata { get; set; } = null!;

    /// <summary>
    /// The Attachments for the Components in the system.
    /// </summary>
    //public DbSet<Attachment> Attachments { get; set; } = null!;

    /// <summary>
    /// A single CosmosDB container is created for all entities.  The parition key is taken from 
    /// the ITenanted interface's Partition property.  This should be the Tenant slug for 
    /// everything.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultContainer("Components");
        modelBuilder.Entity<Tenant>().HasPartitionKey(e => e.Partition);
        modelBuilder.Entity<Component>().HasPartitionKey(e => e.Partition);
        //modelBuilder.Entity<Attachment>().HasPartitionKey(e => e.Partition);
        //modelBuilder.Entity<Metadata>().HasPartitionKey(e => e.Partition);
    }

}
