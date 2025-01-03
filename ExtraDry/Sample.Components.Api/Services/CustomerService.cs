using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.EntityFrameworkCore;

namespace Sample.Components.Api.Services;

/// <summary>
/// Standard CRUD services for Customers.
/// </summary>
public class CustomerService(
    ComponentContext database,
    RuleEngine rules)
{
    public async Task<PagedCollection<Customer>> ListTenantsAsync(PageQuery query)
    {
        var q = database.Tenants.QueryWith(query);
        return await q.ToPagedCollectionAsync();
    }

    public async Task<Customer> CreateTenantAsync(Customer exemplar)
    {
        var tenant = await rules.CreateAsync(exemplar);
        try {
            database.Tenants.Add(tenant);
            await database.SaveChangesAsync();
        }
        catch(DbUpdateException ex) {
            throw new ArgumentException($"Tenant '{tenant.Slug}' already exists", ex);
        }
        return tenant;
    }

    public async Task<Customer?> TryRetrieveTenantAsync(string slug)
    {
        var tenant = await database.Tenants.WithPartitionKey(slug).FirstOrDefaultAsync();
        return tenant;
    }

    public async Task<Customer> RetrieveTenantAsync(string slug)
    {
        return await TryRetrieveTenantAsync(slug)
            ?? throw new ArgumentException($"Tenant '{slug}' not found", nameof(slug));
    }

    public async Task<Customer> UpdateTenantAsync(Customer exemplar)
    {
        var existing = await RetrieveTenantAsync(exemplar.Slug);
        await rules.UpdateAsync(exemplar, existing);
        await database.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteTenantAsync(string slug)
    {
        var tenant = await RetrieveTenantAsync(slug);
        await rules.DeleteAsync(tenant, () => database.Tenants.Remove(tenant), async () => await database.SaveChangesAsync());
    }
}
