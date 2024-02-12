using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages collections of schemas.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class TemplateController {
       
    /// <summary>
    /// Stanard DI Constructor
    /// </summary>
    public TemplateController(TemplateService templateService)
    {
        templates = templateService;
    }

    /// <summary>
    /// Filtered list of all templates
    /// </summary>
    [HttpGet("api/templates"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Template>> ListAsync([FromQuery] SortQuery query)
    {
        return await templates.ListAsync(query);
    }

    /// <summary>
    /// Create a new template
    /// </summary>
    [HttpPost("api/templates"), Consumes("application/json"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<ResourceReference<Template>> CreateAsync(Template value)
    {
        var template = await templates.CreateAsync(value);
        return new ResourceReference<Template>(template);
    }

    /// <summary>
    /// Retreive a specific template
    /// </summary>
    [HttpGet("api/templates/{title}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Template> RetrieveAsync(string title)
    {
        return await templates.RetrieveAsync(title);
    }

    /// <summary>
    /// Update an existing template
    /// </summary>
    [HttpPut("api/templates/{title}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task UpdateAsync(string title, Template value)
    {
        if(title != value?.Title) {
            throw new ArgumentMismatchException("Title in URI must match body.", nameof(title));
        }
        await templates.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing template
    /// </summary>
    [HttpDelete("api/templates/{title}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(string title)
    {
        await templates.DeleteAsync(title);
    }

    private readonly TemplateService templates;
}
