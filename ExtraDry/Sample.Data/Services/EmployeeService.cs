namespace Sample.Data.Services;

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

    public async Task<Employee> CreateAsync(Employee exemplar)
    {
        var employee = await rules.CreateAsync(exemplar);
        database.Employees.Add(employee);
        await database.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> RetrieveAsync(Guid uuid)
    {
        var result = await TryRetrieveAsync(uuid) ?? throw new ArgumentOutOfRangeException(nameof(uuid));
        return result;
    }

    public async Task<Employee?> TryRetrieveAsync(Guid uuid)
    {
        return await database.Employees.FirstOrDefaultAsync(e => e.Uuid == uuid);
    }

    public async Task<Employee> Update(Employee exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Uuid);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
    }

    public async Task Delete(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await rules.DeleteAsync(existing, () => database.Employees.Remove(existing), async () => await database.SaveChangesAsync());
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
