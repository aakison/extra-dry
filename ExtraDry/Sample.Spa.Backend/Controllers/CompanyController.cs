using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages the collection of companies.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
[Display(Name = "The Corporations")]
public class CompanyController {
        
    /// <summary>
    /// Standard DI Constructor
    /// </summary>
    public CompanyController(CompanyService companyService)
    {
        companies = companyService;
    }

    /// <summary>
    /// Filtered list of all companies
    /// </summary>
    /// <remarks>
    /// Provides a complete list of all companies, as this list is not too large, all are returned on every call.
    /// </remarks>
    [HttpGet("api/companies"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<PagedCollection<Company>> List([FromQuery] PageQuery query)
    {
        return await companies.List(query);
    }

    /// <summary>
    /// Retrieve a specific company
    /// </summary>
    [HttpGet("api/companies/{companyId}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Company> Retrieve(Guid companyId)
    {
        return await companies.RetrieveAsync(companyId);
    }

    /// <summary>
    /// Create a new company
    /// </summary>
    /// <remarks>
    /// Create a new company at the URI.
    /// </remarks>
    [HttpPost("api/companies"), Produces("application/json"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<ResourceReference<Company>> Create(Company value)
    {
        var company = await companies.Create(value);
        return new ResourceReference<Company>(company);
    }

    /// <summary>
    /// Update an existing company
    /// </summary>
    /// <remarks>
    /// Update the company at the URI, the uniqueId in the URI must match the Id in the payload.
    /// </remarks>
    [HttpPut("api/companies/{uuid:guid}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid uuid, Company value)
    {
        if(uuid != value?.Uuid) {
            throw new ArgumentMismatchException("ID in URI must match body.", nameof(uuid));
        }
        await companies.Update(value);
    }

    /// <summary>
    /// Update an existing company
    /// </summary>
    /// <remarks>
    /// Update the company at the URI, the uniqueId in the URI must match the Id in the payload.
    /// </remarks>
    [HttpPut("api/companies/{slug}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    [ApiExplorerSettings(GroupName = ApiGroupNames.InternalUseOnly)]
    public async Task Update(string slug, Company value)
    {
        if(slug != value?.Uuid.ToString()) {
            throw new ArgumentMismatchException("ID in URI must match body.", nameof(slug));
        }
        await companies.Update(value);
    }

    /// <summary>
    /// Delete an existing company
    /// </summary>
    [HttpDelete("api/companies/{companyId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid companyId)
    {
        await companies.Delete(companyId);
    }

    private readonly CompanyService companies;
}
