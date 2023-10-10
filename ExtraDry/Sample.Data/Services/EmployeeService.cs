#nullable enable


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

        public async Task<Employee> RetrieveAsync(Guid uniqueId)
        {
            var result = await TryRetrieveAsync(uniqueId) ?? throw new ArgumentOutOfRangeException(nameof(uniqueId));
            return result;
        }

        public async Task<Employee?> TryRetrieveAsync(Guid uniqueId)
        {
            return await database.Employees.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
        }

        public async Task Update(Employee item)
        {
            var existing = await RetrieveAsync(item.Uuid);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task Delete(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            await rules.DeleteAsync(existing, () => database.Employees.Remove(existing), async () => await database.SaveChangesAsync());
        }
    
        private readonly SampleContext database;

        private readonly RuleEngine rules;

    }
}
