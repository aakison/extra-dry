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
            .OrderBy(e => e.AncestorList)
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }
    
    public async Task<List<Region>> ListChildrenAsync(string code)
    {
        var region = await TryRetrieveAsync(code);
        if(region == null) {
            throw new ArgumentException("Invalid region code.");
        }

        return await database.Regions.Where(e => e.AncestorList.GetAncestor(1) == region.AncestorList).ToListAsync();
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

        item.SetParent(parent);

        var maxHierarchy = await database.Regions.Where(e => e.AncestorList!.IsDescendantOf(parent.AncestorList)).MaxAsync(c => c.AncestorList);
        var newHierarchy = parent.AncestorList?.GetDescendant(maxHierarchy, null);
        item.AncestorList = newHierarchy;

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

            }
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();

            existing.SetParent(existing.Parent);

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

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
