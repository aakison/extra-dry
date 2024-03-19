using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Components.Api.Security;
using Sample.Components.Api.Services;

namespace Sample.Components.Api.Controllers;

/// <summary>
/// Endpoints for managing metadata, enabling users to tag components.
/// </summary>
[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
public class MetadataController(
    ComponentService components,
    MetadataService metadata) 
{

    /// <summary>
    /// Retrieve a set of metadata for a component by UUID.  
    /// </summary>
    [HttpGet("/{tenant}/metadata/{uuid}")]
    [Authorize(Policies.User)]
    [Produces("application/json")]
    public async Task<Metadata> RetrieveMetadata(string tenant, Guid uuid)
    {
        var component = await components.RetrieveComponentAsync(tenant, uuid);
        //Authorization.AssertAuthorized(); // TODO: Check ABAC attribute rules.
        var result = await metadata.RetrieveMetadataAsync(component);
        return result;
    }

    /// <summary>
    /// Updates the set of metadata for a component by UUID.  
    /// </summary>
    [HttpPut("/{tenant}/metadata/{uuid}")]
    [Authorize(Policies.User)]
    [Consumes("application/json")]
    public async Task UpdateMetadata(string tenant, Guid uuid, Metadata exemplar)
    {
        var component = await components.RetrieveComponentAsync(tenant, uuid);
        //Authorization.AssertAuthorized(); // TODO: Check ABAC attribute rules.
        await metadata.UpdateMetadataAsync(component, exemplar);
    }

}
