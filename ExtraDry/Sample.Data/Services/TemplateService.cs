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

    public async Task<Template> CreateAsync(Template exemplar)
    {
        var template = await rules.CreateAsync(exemplar);
        database.Templates.Add(template);
        await database.SaveChangesAsync();
        return template;
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

    public async Task<Template> UpdateAsync(Template exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Title);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
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
