namespace Sample.Data.Services;

public class TemplateService(
    SampleContext sampleContext, 
    RuleEngine ruleEngine) 
    : IExpandoSchemaResolver 
{
    public async Task<FilteredCollection<Template>> ListAsync(SortQuery query)
    {
        return await sampleContext.Templates
            .QueryWith(query, e => e.State == TemplateState.Active)
            .ToFilteredCollectionAsync();
    }

    public async Task<Template> CreateAsync(Template exemplar)
    {
        var template = await ruleEngine.CreateAsync(exemplar);
        sampleContext.Templates.Add(template);
        await sampleContext.SaveChangesAsync();
        return template;
    }

    public async Task<Template?> TryRetrieveAsync(string title)
    {
        return await sampleContext.Templates.FirstOrDefaultAsync(e => e.Title == title);
    }

    public async Task<Template> RetrieveAsync(string title)
    {
        return await TryRetrieveAsync(title) 
            ?? throw new ArgumentOutOfRangeException(nameof(title), "No template exists with given Target Type.");
    }

    public async Task<Template> UpdateAsync(Template exemplar)
    {
        var existing = await RetrieveAsync(exemplar.Title);
        await ruleEngine.UpdateAsync(exemplar, existing);
        await sampleContext.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(string title)
    {
        var existing = await RetrieveAsync(title);
        await ruleEngine.DeleteAsync(existing, () => sampleContext.Templates.Remove(existing), () => sampleContext.SaveChangesAsync());
    }

    public async Task<ExpandoSchema?> ResolveAsync(object target)
    {
        var targetType = target.GetType();
        var template = await TryRetrieveAsync(targetType.Name);
        return template?.Schema;
    }
}
