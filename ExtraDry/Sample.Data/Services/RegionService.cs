using System.Data.SqlTypes;

namespace Sample.Data.Services;

public class RegionService {

    public RegionService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<FilteredCollection<Region>> ListAsync(FilterQuery query)
    {
        return await database.Regions
            .OrderBy(e => e.Lineage)
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }
    
    public async Task<List<Region>> ListChildrenAsync(string code)
    {
        var region = await TryRetrieveAsync(code);
        if(region == null) {
            throw new ArgumentException("Invalid region code.");
        }

        return await database.Regions.Where(e => e.AncestorList!.IsDescendantOf(region.AncestorList) && e.AncestorList.GetLevel() == region.AncestorList.GetLevel() + 1).Include(e => e.Parent).ToListAsync();
    }

    public async Task CreateAsync(Region item)
    {
        Region? parent = null;
        if(item.Level != RegionLevel.Global) {
            if(item.Parent == null) {
                throw new ArgumentException("A region must have a parent if it is not at the global level.");
            }
            parent = await TryRetrieveAsync(item.Parent.Slug);
        }

        if(parent == null) {
            throw new ArgumentException("Invalid parent");
        }

        await SetParent(item, parent);

        database.Regions.Add(item);

        await database.SaveChangesAsync();
    }

    public async Task<Region?> TryRetrieveAsync(string code)
    {
        return await database.Regions
            .Include(e => e.Parent)
            .FirstOrDefaultAsync(e => e.Slug == code);
    }

    public async Task<Region> RetrieveAsync(string code)
    {
        var result = await TryRetrieveAsync(code)
            ?? throw new ArgumentOutOfRangeException(nameof(code));
        return result;
    }

    /// <summary>
    /// Updates the provided Region. If allowMove is set to true it also allows this Region to be reparented.
    /// </summary>
    /// <param name="slug">The slug that is used to identify the region to update</param>
    /// <param name="item">The item to update</param>
    /// <param name="allowMove">If false (default), any parent changes will be ignored. If true, will attempt to reparent the provided item to the provided parent.</param>
    /// <returns></returns>
    public async Task UpdateAsync(string slug, Region item, bool allowMove = false)
    {        
        var existing = await RetrieveAsync(slug);
        await database.Database.BeginTransactionAsync();
        try {
            if(allowMove && existing.Parent != null && item.Parent != null && existing.Parent.Slug != item.Parent.Slug) {
                var newParent = await RetrieveAsync(item.Parent.Slug);
                existing.Parent = newParent;

                await SetParent(existing, existing.Parent);
                
            }
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();

            await database.Database.CommitTransactionAsync();
        }
        catch {
            await database.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteAsync(string code)
    {
        var existing = await RetrieveAsync(code);
        await rules.DeleteAsync(existing, () => database.Regions.Remove(existing), () => database.SaveChangesAsync());
    }

    public async Task RestoreAsync(string code)
    {
        var existing = await RetrieveAsync(code);
        await rules.RestoreAsync(existing);
        await database.SaveChangesAsync();
    }

    public async Task SetParent(Region child, Region? parent)
    {
        if(parent == null) { return; }

        if(database.Regions.Any(e => e.Uuid == child.Uuid && e.Lineage.IsDescendantOf(parent.Lineage))) {
            // Already a child of this entity in the DB, so lets not set it again, it'll make the lineage numbers climb.
            return;
        }

        if(parent.Strata >= child.Strata) {
            throw new InvalidOperationException("Parent must be at a higher level than current entity.");
        }
        child.Parent = parent;

        var maxHierarchy = await database.Regions.Where(e => e.Lineage!.IsDescendantOf(parent.Lineage)).MaxAsync(c => c.Lineage);
        var newHierarchy = parent.Lineage?.GetDescendant(maxHierarchy, null);
        child.Lineage = newHierarchy;
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
