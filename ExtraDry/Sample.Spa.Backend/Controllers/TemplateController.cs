using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages collections of schemas.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class TemplateController(
    TemplateService templateService)
{

    /// <summary>
    /// Filtered list of all templates
    /// </summary>
    [HttpGet("api/templates"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Template>> ListAsync([FromQuery] SortQuery query)
    {
        return await templateService.ListAsync(query);
    }

    /// <summary>
    /// Create a new template
    /// </summary>
    [HttpPost("api/templates"), Consumes("application/json"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<ResourceReference<Template>> CreateAsync(Template value)
    {
        var template = await templateService.CreateAsync(value);
        return new ResourceReference<Template>(template);
    }

    /// <summary>
    /// Retreive a specific template
    /// </summary>
    [HttpGet("api/templates/{title}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Template> RetrieveAsync(string title)
    {
        return await templateService.RetrieveAsync(title);
    }

    /// <summary>
    /// Update an existing template
    /// </summary>
    [HttpPut("api/templates/{title}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task UpdateAsync(string title, Template value)
    {
        ArgumentMismatchException.ThrowIfMismatch(title, value.Title, nameof(title));
        await templateService.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing template
    /// </summary>
    [HttpDelete("api/templates/{title}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(string title)
    {
        await templateService.DeleteAsync(title);
    }
}
