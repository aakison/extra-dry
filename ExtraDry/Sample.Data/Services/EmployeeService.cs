#nullable enable

using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ExtraDry.Server;
using ExtraDry.Core;

namespace Sample.Data.Services {
    public class EmployeeService {

        public EmployeeService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<PagedCollection<Employee>> List(PageQuery query)
        {
            return await database.Employees.QueryWith(query).ToPagedCollectionAsync();
        }

        public async Task Create(Employee item)
        {
            database.Employees.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Employee> Retrieve(Guid uniqueId)
        {
            return await database.Employees.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
        }

        public async Task Update(Employee item)
        {
            var existing = await Retrieve(item.Uuid);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task Delete(Guid uniqueId)
        {
            var existing = await Retrieve(uniqueId);
            rules.Delete(existing, () => database.Employees.Remove(existing));
            await database.SaveChangesAsync();
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
