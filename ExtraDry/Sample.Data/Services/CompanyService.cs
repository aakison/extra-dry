namespace Sample.Data.Services;

public class CompanyService(
    SampleContext sampleContext, 
    RuleEngine ruleEngine)
{
    public async Task<PagedCollection<Company>> List(PageQuery query)
    {
        return await sampleContext.Companies.Include(e => e.PrimarySector).QueryWith(query).ToPagedCollectionAsync();
    }

    public async Task<Company> Create(Company item)
    {
        var company = await ruleEngine.CreateAsync(item);
        sampleContext.Companies.Add(company);
        await sampleContext.SaveChangesAsync();
        return company;
    }

    public async Task<Company> RetrieveAsync(Guid companyId)
    {
        var result = await TryRetrieveAsync(companyId) ?? throw new ArgumentOutOfRangeException(nameof(companyId));
        return result;
    }

    public async Task<Company?> TryRetrieveAsync(Guid uniqueId)
    {
        return await sampleContext.Companies
            .Include(e => e.PrimarySector)
            .Include(e => e.AdditionalSectors)
            .FirstOrDefaultAsync(e => e.Uuid == uniqueId);
    }

    public async Task Update(Company item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await ruleEngine.UpdateAsync(item, existing);
        await sampleContext.SaveChangesAsync();
    }

    public async Task Delete(Guid uniqueId)
    {
        var existing = await RetrieveAsync(uniqueId);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Companies.Remove(existing), async () => await sampleContext.SaveChangesAsync());
    }
}
