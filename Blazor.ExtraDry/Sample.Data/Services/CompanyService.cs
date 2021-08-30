#nullable enable

using Blazor.ExtraDry;
using Microsoft.EntityFrameworkCore;
using Sample.Shared;
using System;
using System.Threading.Tasks;

namespace Sample.Data.Services {
    public class CompanyService {

        public CompanyService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<FilteredCollection<Company>> List(FilterQuery query)
        {
            return await database.Companies.QueryWith(query).ToFilteredCollectionAsync();
        }

        public async Task Create(Company item)
        {
            database.Companies.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Company> Retrieve(Guid uniqueId)
        {
            return await database.Companies
                .Include(e => e.PrimarySector)
                .Include(e => e.AdditionalSectors)
                .FirstOrDefaultAsync(e => e.Uuid == uniqueId);
        }

        public async Task Update(Company item)
        {
            var existing = await Retrieve(item.Uuid);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task Delete(Guid uniqueId)
        {
            var existing = await Retrieve(uniqueId);
            rules.Delete(existing, () => database.Companies.Remove(existing));
            await database.SaveChangesAsync();
        }

        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
