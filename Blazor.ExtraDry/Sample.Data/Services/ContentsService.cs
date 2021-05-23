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

        public async Task<FilteredCollection<Content>> List(FilterQuery query)
        {
            return await database.Contents
                .Select(e => new Content { UniqueId = e.UniqueId, Title = e.Title })
                .QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task Create(Content item)
        {
            database.Contents.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Content> Retrieve(Guid uniqueId)
        {
            return await database.Contents.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task Update(Content item)
        {
            var existing = await Retrieve(item.UniqueId);
            rules.Update(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task Delete(Guid uniqueId)
        {
            var existing = await Retrieve(uniqueId);
            rules.Delete(existing, () => database.Contents.Remove(existing));
            await database.SaveChangesAsync();
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
