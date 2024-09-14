using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.EF;

/// <summary>
/// Provides an extension to the Core EF DbContext for hooking aspects to changes to handle cross-cutting concerns.
/// E.g. VersionInfo should be updated consistently across the system.
/// </summary>
/// <remarks>
/// Create a new AspectDbContext, same usage as DbContext.
/// </remarks>
public abstract class AspectDbContext(
    DbContextOptions options, 
    IEnumerable<IDbAspect> aspects) 
    : DbContext(options) 
{

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
        if(aspects.Any()) {
            var changed = GetChanges();
            foreach(var aspect in aspects) {
                aspect.EntitiesChanging(changed);
            }
            var saving = GetChanges();
            saving.Timestamp = changed.Timestamp; // align the timestamp to the same value
            foreach(var aspect in aspects) {
                aspect.EntitiesChanged(saving);
            }
            var saved = GetChanges();
            if(saved.EntitiesAdded.Count() > saving.EntitiesAdded.Count()) {
                throw new InvalidOperationException("Aspects cannot add entities during the EntitiesChanged event.");
            }
        }
    }

    private EntitiesChanged GetChanges()
    {
        var entries = ChangeTracker.Entries();
        var added = entries.Where(e => e.State == EntityState.Added).Select(e => e.Entity);
        var modified = entries.Where(e => e.State == EntityState.Modified).Select(e => e.Entity);
        var deleted = entries.Where(e => e.State == EntityState.Deleted).Select(e => e.Entity);
        var args = new EntitiesChanged(added, modified, deleted, this);
        return args;
    }

}
