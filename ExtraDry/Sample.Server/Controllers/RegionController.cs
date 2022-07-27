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
[ApiExplorerSettings(GroupName = ApiGroupNames.ReferenceCodes)]
[ApiExceptionStatusCodes]
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
        await regions.UpdateAsync(code, item);
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
    public async Task CreateBaseDataAsync()
    {
        var baseRegions = new Region[] {
            new Region { ParentCode = "", Code = "", Title = "All", Description = "All Regions", Level = RegionLevel.Global },
            new Region { ParentCode = "", Code = "AU", Title = "Australia", Description = "Commonwealth of Australia", Level = RegionLevel.Country },
            new Region { ParentCode = "AU", Code = "AU-QLD", Title = "Queensland", Description = "Queensland", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-VIC", Title = "Victoria", Description = "Victoria", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-NSW", Title = "New South Wales", Description = "New South Wales", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-ACT", Title = "Canberra", Description = "Australian Capital Territory", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-TAS", Title = "Tasmania", Description = "Tasmania", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-SA", Title = "South Australia", Description = "South Australia", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-NT", Title = "Northern Territory", Description = "Northern Territory", Level = RegionLevel.Division },
            new Region { ParentCode = "AU", Code = "AU-WA", Title = "Western Australia", Description = "Western Australia", Level = RegionLevel.Division },
            new Region { ParentCode = "AU-QLD", Code = "AU-QLD-Brisbane", Title = "Brisbane", Description = "Brisbane", Level = RegionLevel.Subdivision },
            new Region { ParentCode = "AU-QLD", Code = "AU-QLD-Redlands", Title = "Redlands", Description = "City of Redlands", Level = RegionLevel.Subdivision },
        };
        foreach(var region in baseRegions) {
            await regions.CreateAsync(region);
        }
    }

    private readonly RegionService regions;
}
