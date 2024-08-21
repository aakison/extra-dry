namespace Sample.Data.Services;

public class ContentsService(
    SampleContext sampleContext, 
    RuleEngine ruleEngine)
{
    public async Task<FilteredCollection<Content>> ListAsync(FilterQuery query)
    {
        return await sampleContext.Contents
            .Select(e => new Content { Uuid = e.Uuid, Title = e.Title })
            .QueryWith(query).ToFilteredCollectionAsync();
    }

    public async Task<Content> CreateAsync(Content item)
    {
        var content = await ruleEngine.CreateAsync(item);
        sampleContext.Contents.Add(content);
        await sampleContext.SaveChangesAsync();
        return content;
    }

    public async Task<Content> RetrieveAsync(Guid uniqueId)
    {
        var result = await TryRetrieveAsync(uniqueId) ?? throw new ArgumentOutOfRangeException(nameof(uniqueId));
        return result;
    }

    public async Task<Content?> TryRetrieveAsync(Guid uniqueId)
    {
        return await sampleContext.Contents.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
    }

    public async Task UpdateAsync(Content item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await ruleEngine.UpdateAsync(item, existing);
        await sampleContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uniqueId)
    {
        var existing = await RetrieveAsync(uniqueId);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Contents.Remove(existing), async () => await sampleContext.SaveChangesAsync());
    }
}
