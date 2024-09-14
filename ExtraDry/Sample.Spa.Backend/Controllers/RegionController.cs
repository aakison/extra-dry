using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages the taxonomy/collection of geo-political regions.
/// 
/// The geo-political regions are a three level taxonomy that drill down as:
///   * Country
///   * Division, such as a State or Territory
///   * Subdivision, such as a county, city, or municipality
///   
/// Regions follow the standard subject naming scheme with `Code` (unique), `Title` and `Description`.
/// 
/// Regions also follow the standard taxonomy structure, where each region is at a specific level;
/// and each region is the child of a region in the level above it.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.ReferenceCodes)]
[ApiExceptionStatusCodes]
public class RegionController(
    RegionService regionService)
{

    /// <summary>
    /// Paged list of all regions
    /// </summary>
    [HttpGet("api/regions"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<PagedCollection<Region>> ListAsync([FromQuery] PageQuery query)
    {
        return await regionService.ListAsync(query);
    }

    [HttpGet("api/regions/hierarchy"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<PagedHierarchyCollection<Region>> ListHierarchyAsync([FromQuery] PageHierarchyQuery query)
    {
        return await regionService.ListHierarchyAsync(query);
    }

    /// <summary>
    /// Retrieve a specific region
    /// </summary>
    [HttpGet("api/regions/{code}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Region> RetrieveAsync(string code)
    {
        return await regionService.RetrieveAsync(code);
    }

    /// <summary>
    /// List of all sub-regions of the given region.
    /// </summary>
    [HttpGet("api/regions/{code}/children"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<BaseCollection<Region>> ListChildrenAsync(string code)
    {
        return await regionService.ListChildrenAsync(code);
    }

    /// <summary>
    /// Create a new region
    /// </summary>
    [HttpPost("api/regions"), Consumes("application/json"), Produces("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<ResourceReference<Region>> CreateAsync(Region value)
    {
        var region = await regionService.CreateAsync(value);
        return new ResourceReference<Region>(region);
    }

    /// <summary>
    /// Update an existing region
    /// </summary>
    [HttpPut("api/regions/{code}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task UpdateAsync(string code, Region item)
    {
        await regionService.UpdateAsync(code, item, allowMove: true);
    }

    /// <summary>
    /// Delete an existing region
    /// </summary>
    [HttpDelete("api/regions/{code}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(string code)
    {
        await regionService.DeleteAsync(code);
    }
}
