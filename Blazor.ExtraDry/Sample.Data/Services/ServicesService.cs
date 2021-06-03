#nullable enable

using Blazor.ExtraDry;
using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Data.Services {

    public class ServicesService : IEntityResolver<Service> {

        public ServicesService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<FilteredCollection<Service>> ListAsync(FilterQuery query)
        {
            return await database.Services.QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task CreateAsync(Service item)
        {
            database.Services.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Service> ResolveAsync(Service exemplar)
        {
            return await RetrieveAsync(exemplar.UniqueId);
        }

        public async Task<Service> RetrieveAsync(Guid uniqueId)
        {
            return await database.Services.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task UpdateAsync(Service item)
        {
            var existing = await RetrieveAsync(item.UniqueId);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            rules.Delete(existing, () => database.Services.Remove(existing));
            await database.SaveChangesAsync();
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
