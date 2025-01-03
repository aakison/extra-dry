﻿namespace Sample.Data.Services;

public class RegionService(
    SampleContext sampleContext,
    RuleEngine ruleEngine)
    : IEntityResolver<Region>
{
    public async Task<PagedCollection<Region>> ListAsync(PageQuery query)
    {
        return await sampleContext.Regions
            .Include(e => e.Parent)
            .QueryWith(query)
            .ToPagedCollectionAsync();
    }

    public async Task<PagedHierarchyCollection<Region>> ListHierarchyAsync(PageHierarchyQuery query)
    {
        return await sampleContext.Regions
            .Include(e => e.Parent)
            .QueryWith(query)
            .ToPagedHierarchyCollectionAsync();
    }

    public async Task<BaseCollection<Region>> ListChildrenAsync(string code)
    {
        var region = await RetrieveAsync(code);
        return await sampleContext.Regions
            .Where(e => e.Lineage!.IsDescendantOf(region.Lineage) && e.Lineage.GetLevel() == region.Lineage.GetLevel() + 1)
            .Include(e => e.Parent)
            .ToBaseCollectionAsync();
    }

    public async Task<Region> CreateAsync(Region exemplar)
    {
        var region = await ruleEngine.CreateAsync(exemplar);
        Region? parent = null;
        if(exemplar.Level != RegionLevel.Global) {
            if(exemplar.Parent == null) {
                throw new ArgumentException("A region must have a parent if it is not at the global level.");
            }
            parent = await TryRetrieveAsync(exemplar.Parent.Slug);
            if(parent == null) {
                throw new ArgumentException("Invalid parent");
            }
        }

        await SetParent(region, parent);
        sampleContext.Regions.Add(region);
        await sampleContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> TryRetrieveAsync(string code)
    {
        return await sampleContext.Regions
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
    /// Updates the provided Region. If allowMove is set to true it also allows this Region to be
    /// reparented.
    /// </summary>
    /// <param name="slug">The slug that is used to identify the region to update</param>
    /// <param name="item">The item to update</param>
    /// <param name="allowMove">
    /// If false (default), any parent changes will be ignored. If true, will attempt to reparent
    /// the provided item to the provided parent.
    /// </param>
    /// <returns></returns>
    public async Task UpdateAsync(string slug, Region item, bool allowMove = false)
    {
        var existing = await RetrieveAsync(slug);
        if(!allowMove) {
            // if we're not allowing a parent change, ensure they're not bypassing the parent
            // update by explicitly resetting it.
            item.Parent = existing.Parent;
        }
        else if(allowMove && existing.Parent != null && item.Parent != null && existing.Parent.Slug != item.Parent.Slug) {
            var newParent = await RetrieveAsync(item.Parent.Slug);
            existing.Parent = newParent;

            await SetParent(existing, existing.Parent);
        }
        await ruleEngine.UpdateAsync(item, existing);
        await sampleContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(string code)
    {
        var existing = await RetrieveAsync(code);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Regions.Remove(existing), () => sampleContext.SaveChangesAsync());
    }

    public async Task RestoreAsync(string code)
    {
        var existing = await RetrieveAsync(code);
        await ruleEngine.RestoreAsync(existing);
        await sampleContext.SaveChangesAsync();
    }

    private async Task SetParent(Region child, Region? parent)
    {
        if(parent == null) { return; }

        if(sampleContext.Regions.Any(e => e.Uuid == child.Uuid && e.Lineage.IsDescendantOf(parent.Lineage))) {
            // Already a child of this entity in the DB, so lets not set it again, it'll change the
            // sort and make the lineage numbers climb.
            return;
        }

        if(parent.Strata >= child.Strata) {
            throw new ArgumentException("Parent must be at a higher level than current entity.");
        }
        child.Parent = parent;

        var maxChildLineage = await sampleContext.Regions.Where(e => e.Lineage!.IsDescendantOf(parent.Lineage)).MaxAsync(c => c.Lineage);
        if(maxChildLineage == HierarchyId.GetRoot() || maxChildLineage == parent.Lineage) {
            // If this is the first child of the parent, pass null as the first param for
            // GetDescendant
            maxChildLineage = null;
        }
        var newLineage = parent.Lineage?.GetDescendant(maxChildLineage, null);
        child.Lineage = newLineage ?? HierarchyId.GetRoot();
    }

    public async Task<Region?> ResolveAsync(Region exemplar)
    {
        return await TryRetrieveAsync(exemplar.Slug);
    }
}
