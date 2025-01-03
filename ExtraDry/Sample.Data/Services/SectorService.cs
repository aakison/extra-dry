namespace Sample.Data.Services;

public class SectorService(
    SampleContext sampleContext,
    RuleEngine ruleEngine)
    : IEntityResolver<Sector>
{
    public async Task<FilteredCollection<Sector>> ListAsync(SortQuery query)
    {
        return await sampleContext.Sectors
            .QueryWith(query, e => e.State == SectorState.Active)
            .ToFilteredCollectionAsync();
    }

    public async Task<Sector> CreateAsync(Sector exemplar)
    {
        var sector = await ruleEngine.CreateAsync(exemplar);
        sampleContext.Sectors.Add(sector);
        await sampleContext.SaveChangesAsync();
        return sector;
    }

    public async Task<Sector?> ResolveAsync(Sector exemplar)
    {
        return await TryRetrieveAsync(exemplar.Uuid);
    }

    public async Task<Sector?> TryRetrieveAsync(Guid uuid)
    {
        return await sampleContext.Sectors.FirstOrDefaultAsync(e => e.Uuid == uuid);
    }

    public async Task<Sector> RetrieveAsync(Guid uuid)
    {
        return await TryRetrieveAsync(uuid)
            ?? throw new ArgumentOutOfRangeException(nameof(uuid), "No sector exists with given uuid.");
    }

    public async Task<Sector> UpdateAsync(Sector exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Uuid);
        await ruleEngine.UpdateAsync(exemplar, existing);
        await sampleContext.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Sectors.Remove(existing), async () => await sampleContext.SaveChangesAsync());
    }

    public async Task<Statistics<Sector>> StatsAsync(FilterQuery query)
    {
        return await sampleContext.Sectors
            .QueryWith(query)
            .ToStatisticsAsync();
    }
}
