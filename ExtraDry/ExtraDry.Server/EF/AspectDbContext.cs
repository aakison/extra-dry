using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.EF;

/// <summary>
/// Provides an extension to the Core EF DbContext for hooking aspects to changes to handle cross-cutting concerns.
/// E.g. VersionInfo should be updated consistently across the system.
/// </summary>
public abstract class AspectDbContext : DbContext {

    /// <summary>
    /// Create a new AspectDbContext, same usage as DbContext.
    /// </summary>
    public AspectDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Delegate for the callback for events
    /// </summary>
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Something wrong with CA rules causing a recursive error loop, this follows documentation guidance at https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines")]
    public delegate void EntitiesChangedEventHandler(object sender, EntitiesChangedEventArgs args);

    /// <summary>
    /// Event that notifies listeners about impending changes on EF context entities.
    /// Used by Aspects to hook into the context for de-coupled changes to the system, e.g. version updates.
    /// </summary>
    public event EntitiesChangedEventHandler EntitiesChangedEvent = null!;

    /// <summary>
    /// Saves all changes made to this context to the database, applying version information as necessary.
    /// </summary>
    /// <remarks>
    /// Only need to override 2 of the 4 SaveChanges as the other 2 call these.
    /// </remarks>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnEntitiesChanging();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    /// Saves all changes made to this context to the database, applying version information as necessary.
    /// </summary>
    /// <remarks>
    /// Only need to override 2 of the 4 SaveChanges as the other 2 call these.
    /// </remarks>
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnEntitiesChanging();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnEntitiesChanging()
    {
        if(EntitiesChangedEvent != null) {
            var added = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(e => e.Entity);
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Select(e => e.Entity);
            var deleted = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).Select(e => e.Entity);
            var args = new EntitiesChangedEventArgs(added, modified, deleted);
            EntitiesChangedEvent(this, args);
        }
    }

}
