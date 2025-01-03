namespace Sample.Data.Services;

public class EmployeeService(
    SampleContext sampleContext,
    RuleEngine ruleEngine)
{
    public async Task<PagedCollection<Employee>> List(PageQuery query)
    {
        return await sampleContext.Employees.QueryWith(query).ToPagedCollectionAsync();
    }

    public async Task<Employee> CreateAsync(Employee exemplar)
    {
        var employee = await ruleEngine.CreateAsync(exemplar);
        sampleContext.Employees.Add(employee);
        await sampleContext.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> RetrieveAsync(Guid uuid)
    {
        var result = await TryRetrieveAsync(uuid) ?? throw new ArgumentOutOfRangeException(nameof(uuid));
        return result;
    }

    public async Task<Employee?> TryRetrieveAsync(Guid uuid)
    {
        return await sampleContext.Employees.FirstOrDefaultAsync(e => e.Uuid == uuid);
    }

    public async Task<Employee> Update(Employee exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Uuid);
        await ruleEngine.UpdateAsync(exemplar, existing);
        await sampleContext.SaveChangesAsync();
        return existing;
    }

    public async Task Delete(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Employees.Remove(existing), async () => await sampleContext.SaveChangesAsync());
    }
}
