namespace Sample.Data.Services;

public class SectorService : IEntityResolver<Sector> {

    public SectorService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<FilteredCollection<Sector>> ListAsync(SortQuery query)
    {
        return await database.Sectors
            .QueryWith(query, e => e.State == SectorState.Active)
            .ToFilteredCollectionAsync();
    }

    public async Task<Sector> CreateAsync(Sector exemplar)
    {
        var sector = await rules.CreateAsync(exemplar);
        database.Sectors.Add(sector);
        await database.SaveChangesAsync();
        return sector;
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

    public async Task<Sector> UpdateAsync(Sector exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Uuid);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await rules.DeleteAsync(existing, () => database.Sectors.Remove(existing), async () => await database.SaveChangesAsync());
    }
    
    public async Task<Statistics<Sector>> StatsAsync(FilterQuery query)
    {
        return await database.Sectors
            .QueryWith(query)
            .ToStatisticsAsync();
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
