#nullable enable

using Blazor.ExtraDry;
using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                .Select(e => new Content { UniqueId = e.UniqueId, Title = e.Title })
                .QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task CreateAsync(Content item)
        {
            database.Contents.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Content> RetrieveAsync(Guid uniqueId)
        {
            return await database.Contents.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task UpdateAsync(Content item)
        {
            var existing = await RetrieveAsync(item.UniqueId);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            rules.Delete(existing, () => database.Contents.Remove(existing));
            await database.SaveChangesAsync();
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
