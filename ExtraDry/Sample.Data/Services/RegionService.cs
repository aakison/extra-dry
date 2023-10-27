using System.Globalization;

namespace Sample.Data.Services;

public class RegionService : IEntityResolver<Region> {

    public RegionService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<PagedCollection<Region>> ListAsync(FilterQuery query)
    {
        return await database.Regions
            .Include(e => e.Parent)
            .OrderBy(e => e.Ancestry)
            .QueryWith(query)
            .ToPagedCollectionAsync();
    }

    public async Task<List<Region>> ListHierarchyAsync(HierarchyQuery query)
    {
        var level = string.IsNullOrEmpty(query.Filter)
            ? database.Regions.Where(e => e.Ancestry.GetLevel() <= query.Level)
            : database.Regions;
        var filtered = level.QueryWith(query);
        var ancestors = AncestorOf(filtered);
        var expansions = ChildrenOf(query.Expand);
        var collapses = DescendantOf(query.Collapse);
        
        //var all = filtered.Union(ancestors);
        var all = filtered.Union(expansions).Union(ancestors).Except(collapses).OrderBy(e => e.Ancestry);
        
        return await all.ToListAsync();

        IQueryable<Region> ChildrenOf(IEnumerable<string> parentSlugs) =>
            database.Regions.SelectMany(parent => database.Regions
                .Where(child => child.Ancestry.IsDescendantOf(parent.Ancestry)
                            && child.Ancestry.GetLevel() == parent.Ancestry.GetLevel() + 1
                            && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<Region> DescendantOf(IEnumerable<string> parentSlugs) =>
            database.Regions.SelectMany(parent => database.Regions
                .Where(child => child.Ancestry.IsDescendantOf(parent.Ancestry)
                    && child.Ancestry.GetLevel() > parent.Ancestry.GetLevel()
                    && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<Region> AncestorOf(IQueryable<Region> filteredSubset) =>
            database.Regions.SelectMany(ancestor => filteredSubset
                .Where(descendant => descendant.Ancestry.IsDescendantOf(ancestor.Ancestry)
                    && descendant.Ancestry.GetLevel() != ancestor.Ancestry.GetLevel()),
                (ancestor, descendant) => ancestor);
    }

    public async Task<List<Region>> ListChildrenAsync(string code)
    {
        var region = await RetrieveAsync(code);
        return await database.Regions.Where(e => e.Ancestry!.IsDescendantOf(region.Ancestry) && e.Ancestry.GetLevel() == region.Ancestry.GetLevel() + 1).Include(e => e.Parent).ToListAsync();
    }

    public async Task CreateAsync(Region item)
    {
        Region? parent = null;
        if(item.Level != RegionLevel.Global) {
            if(item.Parent == null) {
                throw new ArgumentException("A region must have a parent if it is not at the global level.");
            }
            parent = await TryRetrieveAsync(item.Parent.Slug);

            if(parent == null) {
                throw new ArgumentException("Invalid parent");
            }
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
        if(!allowMove) {
            // if we're not allowing a parent change, ensure they're not bypassing the parent update by explicitly resetting it.
            item.Parent = existing.Parent;
        }
        else if(allowMove && existing.Parent != null && item.Parent != null && existing.Parent.Slug != item.Parent.Slug) {
            var newParent = await RetrieveAsync(item.Parent.Slug);
            existing.Parent = newParent;

            await SetParent(existing, existing.Parent);
        }
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
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

    private async Task SetParent(Region child, Region? parent)
    {
        if(parent == null) { return; }

        if(database.Regions.Any(e => e.Uuid == child.Uuid && e.Ancestry.IsDescendantOf(parent.Ancestry))) {
            // Already a child of this entity in the DB, so lets not set it again, it'll change the sort and make the lineage numbers climb.
            return;
        }

        if(parent.Strata >= child.Strata) {
            throw new ArgumentException("Parent must be at a higher level than current entity.");
        }
        child.Parent = parent;

        var maxChildLineage = await database.Regions.Where(e => e.Ancestry!.IsDescendantOf(parent.Ancestry)).MaxAsync(c => c.Ancestry);
        if(maxChildLineage == HierarchyId.GetRoot() || maxChildLineage == parent.Ancestry) {
            // If this is the first child of the parent, pass null as the first param for GetDescendant
            maxChildLineage = null;
        }
        var newLineage = parent.Ancestry?.GetDescendant(maxChildLineage, null);
        child.Ancestry = newLineage ?? HierarchyId.GetRoot();
    }

    public async Task<Region?> ResolveAsync(Region exemplar)
    {
        return await TryRetrieveAsync(exemplar.Slug);
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;
}
