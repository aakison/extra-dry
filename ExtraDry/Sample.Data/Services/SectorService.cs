namespace Sample.Data.Services; 

public class SectorService : IEntityResolver<Sector> {

    public SectorService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<FilteredCollection<Sector>> ListAsync(FilterQuery query)
    {
        return await database.Sectors
            .QueryWith(query, e => e.State == SectorState.Active)
            .ToFilteredCollectionAsync();
    }

    public async Task CreateAsync(Sector item)
    {
        database.Sectors.Add(item);
        await database.SaveChangesAsync();
    }

    public async Task<Sector?> ResolveAsync(Sector exemplar)
    {
        return await TryRetrieveAsync(exemplar.Uuid);
    }

    public async Task<Sector?> TryRetrieveAsync(Guid uuid)
    {
        return await database.Sectors.FirstOrDefaultAsync(e => e.Uuid == uuid);
    }

    public async Task<Sector> RetrieveAsync(Guid uuid)
    {
        return await TryRetrieveAsync(uuid) 
            ?? throw new ArgumentOutOfRangeException(nameof(uuid), "No sector exists with given uuid.");
    }

    public async Task UpdateAsync(Sector item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        rules.Delete(existing, () => database.Sectors.Remove(existing));
        await database.SaveChangesAsync();
    }
    
    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
