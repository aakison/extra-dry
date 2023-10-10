namespace Sample.Data.Services;

public class TemplateService : IExpandoSchemaResolver {

    public TemplateService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<FilteredCollection<Template>> ListAsync(SortQuery query)
    {
        return await database.Templates
            .QueryWith(query, e => e.State == TemplateState.Active)
            .ToFilteredCollectionAsync();
    }

    public async Task CreateAsync(Template item)
    {
        database.Templates.Add(item);
        await database.SaveChangesAsync();
    }

    public async Task<Template?> TryRetrieveAsync(string title)
    {
        return await database.Templates.FirstOrDefaultAsync(e => e.Title == title);
    }

    public async Task<Template> RetrieveAsync(string title)
    {
        return await TryRetrieveAsync(title) 
            ?? throw new ArgumentOutOfRangeException(nameof(title), "No template exists with given Target Type.");
    }

    public async Task UpdateAsync(Template item)
    {
        var existing = await RetrieveAsync(item.Title);
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
    }

    public async Task DeleteAsync(string title)
    {
        var existing = await RetrieveAsync(title);
        await rules.DeleteAsync(existing, () => database.Templates.Remove(existing), () => database.SaveChangesAsync());
    }

    public async Task<ExpandoSchema?> ResolveAsync(object target)
    {
        var targetType = target.GetType();
        var template = await TryRetrieveAsync(targetType.Name);
        return template?.Schema;
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

}
