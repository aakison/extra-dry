using ExtraDry.Core;
using ExtraDry.Server;
using ExtraDry.Server.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Components.Api.Security;
using Sample.Components.Api.Services;

namespace Sample.Components.Api.Controllers;

/// <summary>
/// Endpoints for managing components, typically called by Agent process.
/// </summary>
[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
public class ComponentController(
    ComponentService components,
    IAuthorizationService auth)
{
    /// <summary>
    /// Retrieve a paged list of components for a tenant. Not typically very useful as it doesn't
    /// apply ABAC and can therefore only be used by Admins.
    /// </summary>
    [HttpGet("/{tenant}/components")]
    [Authorize(Policies.Admin)]
    [Produces("application/json")]
    public async Task<PagedCollection<Component>> ListComponents(string tenant, [FromQuery] PageQuery query)
    {
        // TODO: Apply ABAC as part of query
        var results = await components.ListComponentsAsync(tenant, query);
        // TODO: Double-check ABAC filter on results.
        return results;
    }

    /// <summary>
    /// Create a new component, enabling the use of `Attachments` and `Conversations` for it.
    /// Limited to Agents which populate this from an event feed from other microservices.
    /// </summary>
    [HttpPost("/{tenant}/components")]
    [Authorize(Policies.Agent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Component>> CreateComponent(string tenant, Component component)
    {
        var created = await components.CreateComponentAsync(tenant, component);
        return new ResourceReference<Component>(created);
    }

    /// <summary>
    /// Retrieve a single component by UUID.
    /// </summary>
    [HttpGet("/{tenant}/components/{uuid}")]
    [Authorize(Policies.User)]
    [Produces("application/json")]
    public async Task<Component> ReadComponent(string tenant, Guid uuid)
    {
        var component = await components.RetrieveComponentAsync(tenant, uuid);
        await auth.AssertAuthorizedAsync(component, AbacRequirement.Read);
        return component;
    }

    /// <summary>
    /// Updates an existing component. Limited to Agents which populate this from an event feed
    /// from other microservices.
    /// </summary>
    [HttpPut("/{tenant}/components/{uuid}")]
    [Authorize(Policies.Agent)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<ResourceReference<Component>> UpdateComponent(string tenant, Guid uuid, Component component)
    {
        ArgumentMismatchException.ThrowIfMismatch(uuid, component.Uuid, nameof(uuid));
        var updated = await components.UpdateComponentAsync(tenant, component);
        await auth.AssertAuthorizedAsync(updated, AbacRequirement.Update);
        return new ResourceReference<Component>(updated);
    }

    /// <summary>
    /// Deletes an existing `Component`. Limited to Agents which populate this from an event feed
    /// from other microservices.
    /// </summary>
    [HttpDelete("/{tenant}/components/{uuid}")]
    [Authorize(Policies.Agent)]
    public async Task DeleteComponent(string tenant, Guid uuid)
    {
        // No ABAC, agents can do whatever they want.
        await components.DeleteComponentAsync(tenant, uuid);
    }
}
