using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Server.Controllers;

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
        template = templateService;
    }

    /// <summary>
    /// Filtered list of all templates
    /// </summary>
    [HttpGet("api/templates"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Template>> ListAsync([FromQuery] SortQuery query)
    {
        return await template.ListAsync(query);
    }

    /// <summary>
    /// Create a new template
    /// </summary>
    [HttpPost("api/templates"), Consumes("application/json")]
    [AllowAnonymous]
    public async Task CreateAsync(Template value)
    {
        await template.CreateAsync(value);
    }

    /// <summary>
    /// Retreive a specific template
    /// </summary>
    [HttpGet("api/templates/{title}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Template> RetrieveAsync(string title)
    {
        return await template.RetrieveAsync(title);
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
        await template.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing template
    /// </summary>
    [HttpDelete("api/templates/{title}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(string title)
    {
        await template.DeleteAsync(title);
    }

    private readonly TemplateService template;
}
