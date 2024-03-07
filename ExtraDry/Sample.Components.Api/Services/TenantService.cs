using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.EntityFrameworkCore;

namespace Sample.Components.Api.Services;

public class TenantService(
    ComponentContext database,
    RuleEngine rules)
{

    public async Task<PagedCollection<Tenant>> ListTenantsAsync(PageQuery query)
    {
        var q = database.Tenants.QueryWith(query);
        return await q.ToPagedCollectionAsync();
    }

    public async Task<Tenant> CreateTenantAsync(Tenant exemplar)
    {
        var tenant = await rules.CreateAsync(exemplar);
        database.Tenants.Add(tenant);
        await database.SaveChangesAsync();
        return tenant;
    }

    public async Task<Tenant?> TryRetrieveTenantAsync(string slug)
    {
        var tenant = await database.Tenants.WithPartitionKey(slug).FirstOrDefaultAsync();
        return tenant;
    }

    public async Task<Tenant> RetrieveTenantAsync(string slug)
    {
        return await TryRetrieveTenantAsync(slug)
            ?? throw new ArgumentException($"Tenant '{slug}' not found", nameof(slug));
    }

    public async Task<Tenant?> TryRetrieveTenantAsync(Guid uuid)
    {
        var tenant = await database.Tenants.FirstOrDefaultAsync(e => e.Uuid == uuid);
        return tenant;
    }

    public async Task<Tenant> RetrieveTenantAsync(Guid uuid)
    {
        return await TryRetrieveTenantAsync(uuid)
            ?? throw new ArgumentException($"Tenant '{uuid}' not found", nameof(uuid));
    }   

    public async Task<Tenant> UpdateTenantAsync(Tenant exemplar)
    {
        var existing = await RetrieveTenantAsync(exemplar.Slug);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteTenantAsync(Guid uuid)
    {
        var tenant = await RetrieveTenantAsync(uuid);
        await rules.DeleteAsync(tenant, database.Tenants.Remove(tenant), database.SaveChangesAsync());
    }

    public async Task DeleteTenantAsync(string slug)
    {
        var tenant = await RetrieveTenantAsync(slug);
        await rules.DeleteAsync(tenant, database.Tenants.Remove(tenant), database.SaveChangesAsync());
    }
}
