namespace Sample.Data.Services;

public class CompanyService {

    public CompanyService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<PagedCollection<Company>> List(PageQuery query)
    {
        return await database.Companies.Include(e => e.PrimarySector).QueryWith(query).ToPagedCollectionAsync();
    }

    public async Task<Company> Create(Company item)
    {
        var company = await rules.CreateAsync(item);
        database.Companies.Add(company);
        await database.SaveChangesAsync();
        return company;
    }

    public async Task<Company> RetrieveAsync(Guid companyId)
    {
        var result = await TryRetrieveAsync(companyId) ?? throw new ArgumentOutOfRangeException(nameof(companyId));
        return result;
    }

    public async Task<Company?> TryRetrieveAsync(Guid uniqueId)
    {
        return await database.Companies
            .Include(e => e.PrimarySector)
            .Include(e => e.AdditionalSectors)
            .FirstOrDefaultAsync(e => e.Uuid == uniqueId);
    }

    public async Task Update(Company item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
    }

    public async Task Delete(Guid uniqueId)
    {
        var existing = await RetrieveAsync(uniqueId);
        await rules.DeleteAsync(existing, () => database.Companies.Remove(existing), async () => await database.SaveChangesAsync());
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
