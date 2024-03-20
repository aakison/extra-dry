using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.EntityFrameworkCore;

namespace Sample.Components.Api.Services;

/// <summary>
/// Standard CRUD services for Components, each requiring a tenant slug for scope.
/// </summary>
public class ComponentService(
    ComponentContext database,
    TenantService tenants,
    RuleEngine rules)
{

    public async Task<PagedCollection<Component>> ListComponentsAsync(string tenant, PageQuery query)
    {
        var q = database.Components.WithPartitionKey(tenant).QueryWith(query);
        return await q.ToPagedCollectionAsync();
    }

    public async Task<Component> CreateComponentAsync(string tenant, Component exemplar)
    {
        var component = await rules.CreateAsync(exemplar);
        var t = await tenants.RetrieveTenantAsync(tenant);
        component.Tenant = t.Slug;
        database.Components.Add(component);
        await database.SaveChangesAsync();
        return component;
    }

    public async Task<Component?> TryRetrieveComponentAsync(string tenant, string slug)
    {
        var component = await database.Components.WithPartitionKey(tenant).FirstOrDefaultAsync(e => e.Slug == slug);
        return component;
    }

    public async Task<Component> RetrieveComponentAsync(string tenant, string slug)
    {
        return await TryRetrieveComponentAsync(tenant, slug)
            ?? throw new ArgumentException($"Component '{slug}' not found", nameof(slug));
    }

    public async Task<Component?> TryRetrieveComponentAsync(string tenant, Guid uuid)
    {
        var component = await database.Components.WithPartitionKey(tenant).FirstOrDefaultAsync(e => e.Uuid == uuid);
        return component;
    }

    public async Task<Component> RetrieveComponentAsync(string tenant, Guid uuid)
    {
        return await TryRetrieveComponentAsync(tenant, uuid)
            ?? throw new ArgumentException($"Component '{uuid}' not found", nameof(uuid));
    }   

    public async Task<Component> UpdateComponentAsync(string tenant, Component exemplar)
    {
        var existing = await RetrieveComponentAsync(tenant, exemplar.Slug);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteComponentAsync(string tenant, Guid uuid)
    {
        var existing = await RetrieveComponentAsync(tenant, uuid);
        await rules.DeleteAsync(tenant, database.Components.Remove(existing), database.SaveChangesAsync());
    }

    public async Task DeleteComponentAsync(string tenant, string slug)
    {
        var existing = await RetrieveComponentAsync(tenant, slug);
        await rules.DeleteAsync(tenant, database.Components.Remove(existing), database.SaveChangesAsync());
    }
}
