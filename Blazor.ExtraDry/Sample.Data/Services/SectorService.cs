#nullable enable

using Blazor.ExtraDry;
using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Data.Services {

    public class SectorService : IEntityResolver<Sector> {

        public SectorService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<FilteredCollection<Sector>> ListAsync(FilterQuery query)
        {
            return await database.Sectors.QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task CreateAsync(Sector item)
        {
            database.Sectors.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Sector> ResolveAsync(Sector exemplar)
        {
            return await RetrieveAsync(exemplar.Uuid);
        }

        public async Task<Sector> RetrieveAsync(Guid uniqueId)
        {
            return await database.Sectors.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
        }

        public async Task UpdateAsync(Sector item)
        {
            var existing = await RetrieveAsync(item.Uuid);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            rules.Delete(existing, () => database.Sectors.Remove(existing));
            await database.SaveChangesAsync();
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
