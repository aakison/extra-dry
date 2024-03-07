using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Components.Api.Services;

namespace Sample.Components.Api.Controllers;

[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
//[ApiExplorerSettings(GroupName = "Components")]
public class TenantController(
    TenantService tenants) 
{

    [HttpGet("/tenants")]
    [Authorize(Policies.Admin)]
    [Produces("application/json")]
    public async Task<PagedCollection<Tenant>> ListTenants([FromQuery] PageQuery query)
    {
        return await tenants.ListTenantsAsync(query);
    }

    [HttpPost("/tenants")]
    [Authorize(Policies.AdminOrAgent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Tenant>> CreateTenant(Tenant exemplar)
    {
        var tenant = await tenants.CreateTenantAsync(exemplar);
        return new ResourceReference<Tenant>(tenant);
    }

    /// <summary>
    /// Retrieve a tenant by its UUID.  For efficiency, prefer the slug version of this method.
    /// </summary>
    [HttpGet("/tenants/{uuid:guid}")]
    [Authorize(Policies.Admin)]
    [Produces("application/json")]
    public async Task<Tenant> RetrieveTenant(Guid uuid)
    {
        return await tenants.RetrieveTenantAsync(uuid);
    }

    /// <summary>
    /// Retrieve a tenant by its Slug.
    /// </summary>
    [HttpGet("/tenants/{slug}")]
    [Authorize(Policies.Admin)]
    [Produces("application/json")]
    public async Task<Tenant> RetrieveTenant(string slug)
    {
        return await tenants.RetrieveTenantAsync(slug);
    }

    [HttpPut("/tenants/{slug}")]
    [Authorize(Policies.Agent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Tenant>> UpdateTenant(string slug, Tenant exemplar)
    {
        if(slug != exemplar.Slug) {
            throw new ArgumentException("Slug mismatch", nameof(slug));
        }
        var tenant = await tenants.UpdateTenantAsync(exemplar);
        return new ResourceReference<Tenant>(tenant);
    }

    [HttpDelete("/tenants/{slug}")]
    [Authorize(Policies.Agent)]
    public async Task<ResourceReference<Tenant>> DeleteTenant(string slug)
    {
        await tenants.DeleteTenantAsync(slug);
        return new ResourceReference<Tenant>();
    }

}
