using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Server.Controllers;

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
[ApiExplorerSettings(GroupName = "reference-codes")]
[SkipStatusCodePages]
public class RegionController {
        
    /// <summary>
    /// Standard DI Constructor
    /// </summary>
    public RegionController(RegionService regionService)
    {
        regions = regionService;
    }

    /// <summary>
    /// Filtered list of all regions
    /// </summary>
    [HttpGet("api/regions"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Region>> ListAsync([FromQuery] FilterQuery query)
    {
        return await regions.ListAsync(query);
    }

    /// <summary>
    /// Retrieve a specific region
    /// </summary>
    [HttpGet("api/regions/{code}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Region> RetrieveAsync(string code)
    {
        return await regions.RetrieveAsync(code);
    }

    /// <summary>
    /// List of all sub-regions of the given region.
    /// </summary>
    [HttpGet("api/regions/{code}/children"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Region>> ListChildrenAsync(string code)
    {
        return await regions.ListChildrenAsync(code);
    }

    /// <summary>
    /// Create a new region
    /// </summary>
    [HttpPost("api/regions"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task CreateAsync(Region value)
    {
        await regions.CreateAsync(value);
    }

    /// <summary>
    /// Update an existing region
    /// </summary>
    [HttpPut("api/regions/{code}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task UpdateAsync(string code, Region item)
    {
        if(code != item.Code) {
            throw new ArgumentException("ID in URI must match body.", nameof(code));
        }
        await regions.UpdateAsync(item);
    }

    /// <summary>
    /// Delete an existing region
    /// </summary>
    [HttpDelete("api/regions/{code}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(string code)
    {
        await regions.DeleteAsync(code);
    }

    /// <summary>
    /// Populates the set of regions with commonly used regions
    /// </summary>
    [HttpPost("api/populate/regions")]
    [AllowAnonymous]
    public async Task PopulateAsync()
    {
        var au = new Region { ParentCode = "", Code = "AU", Title = "Australia", Description = "Commonwealth of Australia", Level = RegionLevel.Country };
        await regions.CreateAsync(au);
        var qld = new Region { ParentCode = "AU", Code = "AU-QLD", Title = "Queensland", Description = "Queensland", Level = RegionLevel.Division };
        await regions.CreateAsync(qld);
        var vic = new Region { ParentCode = "AU", Code = "AU-VIC", Title = "Victoria", Description = "Victoria", Level = RegionLevel.Division };
        await regions.CreateAsync(vic);
        var brisbane = new Region { ParentCode = "AU-QLD", Code = "AU-QLD-Brisbane", Title = "Brisbane", Description = "Brisbane", Level = RegionLevel.Subdivision };
        await regions.CreateAsync(brisbane);
        var redlands = new Region { ParentCode = "AU-QLD", Code = "AU-QLD-Redlands", Title = "Redlands", Description = "City of Redlands", Level = RegionLevel.Subdivision };
        await regions.CreateAsync(redlands);
    }

    private readonly RegionService regions;
}
