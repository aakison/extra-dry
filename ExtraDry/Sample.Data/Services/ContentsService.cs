#nullable enable


namespace Sample.Data.Services {
    public class ContentsService {

        public ContentsService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<FilteredCollection<Content>> ListAsync(FilterQuery query)
        {
            return await database.Contents
                .Select(e => new Content { Uuid = e.Uuid, Title = e.Title })
                .QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task CreateAsync(Content item)
        {
            database.Contents.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Content> RetrieveAsync(Guid uniqueId)
        {
            var result = await TryRetrieveAsync(uniqueId) ?? throw new ArgumentOutOfRangeException(nameof(uniqueId));
            return result;
        }

        public async Task<Content?> TryRetrieveAsync(Guid uniqueId)
        {
            return await database.Contents.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
        }

        public async Task UpdateAsync(Content item)
        {
            var existing = await RetrieveAsync(item.Uuid);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            await rules.DeleteAsync(existing, () => database.Contents.Remove(existing), async () => await database.SaveChangesAsync());
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
