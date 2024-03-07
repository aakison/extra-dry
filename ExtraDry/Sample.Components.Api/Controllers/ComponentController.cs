using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Components.Api.Services;

namespace Sample.Components.Api.Controllers;

[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
public class ComponentController(
    ComponentService components) 
{

    [HttpGet("/{tenant}/components")]
    [Authorize(Policies.User)]
    [Produces("application/json")]
    public async Task<PagedCollection<Component>> ListComponents(string tenant, [FromQuery] PageQuery query)
    {
        // TODO: Apply ABAC as part of query
        return await components.ListComponentsAsync(tenant, query);
    }

    [HttpPost("/{tenant}/components")]
    [Authorize(Policies.Agent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Component>> CreateComponent(string tenant, Component component)
    {
        var created = await components.CreateComponentAsync(tenant, component);
        return new ResourceReference<Component>(created);
    }

    [HttpGet("/{tenant}/components/{uuid}")]
    [Authorize(Policies.User)]
    [Produces("application/json")]
    public async Task<Component> RetrieveComponent(string tenant, Guid uuid)
    {
        var component = await components.RetrieveComponentAsync(tenant, uuid);
        //Authorization.AssertAuthorized(); // TODO: Check ABAC attribute rules.
        return component;
    }

    [HttpPut("/{tenant}/components/{uuid}")]
    [Authorize(Policies.Agent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Component>> UpdateComponent(string tenant, Guid uuid, Component component)
    {
        ArgumentMismatchException.ThrowIfMismatch(uuid, component.Uuid, nameof(uuid));
        var updated = await components.UpdateComponentAsync(tenant, component);
        return new ResourceReference<Component>(updated);
    }

    [HttpDelete("/{tenant}/components/{uuid}")]
    [Authorize(Policies.Agent)]
    public async Task DeleteComponent(string tenant, Guid uuid)
    {
        await components.DeleteComponentAsync(tenant, uuid);
    }

}
