using Blazor.ExtraDry;
using Sample.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.Services {
    public class EmployeeService {

        public EmployeeService(SampleContext sampleContext, RuleEngine ruleEngine)
        {
            database = sampleContext;
            rules = ruleEngine;
        }

        public async Task<PartialCollection<Employee>> List(PartialQuery query)
        {
            var items = await database.Employees.ToListAsync();
            return new PartialCollection<Employee>(items);
        }

        public async Task Create(Employee item)
        {
            database.Employees.Add(item);
            await database.SaveChangesAsync();
        }

        public async Task<Employee> Retrieve(Guid uniqueId)
        {
            return await database.Employees.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task Update(Employee item)
        {
            var existing = await Retrieve(item.UniqueId);
            rules.Update(item, existing);
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
