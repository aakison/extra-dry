using Blazor.ExtraDry;
using Blazor.ExtraDry.Sample.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Sample.Data.Services {
    public class EmployeeService {

        public EmployeeService()
        {
            // Note: In non-sample/production app, replace this with DI.
            database = SampleContext.Current;
            rules = new RuleEngine();
        }

        public async Task<IEnumerable<Employee>> List()
        {
            return await database.Employees.ToListAsync();
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
