using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages the collection of contents.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class ContentsController(
    ContentsService contents)
{

    /// <summary>
    /// Filtered list of all contents
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("api/contents"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Content>> List([FromQuery] FilterQuery query)
    {
        return await contents.ListAsync(query);
    }

    /// <summary>
    /// Create a new page of content
    /// </summary>
    /// <remarks>
    /// Create a new content entity at the URI, the uniqueId in the URI must match the Id in the payload.
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("api/contents"), Produces("application/json"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<ResourceReference<Content>> CreateAsync(Content value)
    {
        var content = await contents.CreateAsync(value);
        return new ResourceReference<Content>(content);
    }

    /// <summary>
    /// Retreive a specific page of content
    /// </summary>
    /// <param name="contentId"></param>
    /// <returns></returns>
    [HttpGet("api/contents/{contentId}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Content> Retrieve(Guid contentId)
    {
        return await contents.RetrieveAsync(contentId);
    }

    /// <summary>
    /// Update an existing page of content
    /// </summary>
    /// <remarks>
    /// Update the content at the URI, the uniqueId in the URI must match the Id in the payload.
    /// </remarks>
    /// <param name="uuid"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    [HttpPut("api/contents/{uuid}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid uuid, Content content)
    {
        ArgumentMismatchException.ThrowIfMismatch(uuid, content.Uuid, nameof(uuid));
        await contents.UpdateAsync(content);
    }

    /// <summary>
    /// Delete an existing page of content
    /// </summary>
    /// <param name="contentId"></param>
    /// <returns></returns>
    [HttpDelete("api/contents/{contentId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid contentId)
    {
        await contents.DeleteAsync(contentId);
    }
}
