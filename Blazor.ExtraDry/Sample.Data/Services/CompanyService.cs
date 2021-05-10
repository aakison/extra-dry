using Blazor.ExtraDry;
using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.Services {
    public class CompanyService {

        public CompanyService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<PartialCollection<Company>> List(PartialQuery query)
        {
            return await database.Companies.QueryWith(query).ToPartialCollectionAsync();
        }

        public async Task Create(Company item)
        {
            database.Companies.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Company> Retrieve(Guid uniqueId)
        {
            return await database.Companies.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task Update(Company item)
        {
            var existing = await Retrieve(item.UniqueId);
            rules.Update(item, existing);
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
