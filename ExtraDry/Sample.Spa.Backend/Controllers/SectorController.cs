using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages collections of sectors for companies.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class SectorController(
    SectorService sectors)
{

    /// <summary>
    /// Filtered list of all company services
    /// </summary>
    [HttpGet("api/sectors"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Sector>> ListAsync([FromQuery] SortQuery query)
    {
        return await sectors.ListAsync(query);
    }

    /// <summary>
    /// Create a new global sector
    /// </summary>
    [HttpPost("api/sectors"), Consumes("application/json"), Produces("application/json")]
    [Authorize(nameof(SectorController))]
    public async Task<ResourceReference<Sector>> CreateAsync(Sector value)
    {
        var sector = await sectors.CreateAsync(value);
        return new ResourceReference<Sector>(sector);
    }

    /// <summary>
    /// Retreive a specific company sector
    /// </summary>
    [HttpGet("api/sectors/{uuid}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Sector> RetrieveAsync(Guid uuid)
    {
        return await sectors.RetrieveAsync(uuid);
    }

    /// <summary>
    /// Update an existing company sector
    /// </summary>
    [HttpPut("api/sectors/{uuid}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task UpdateAsync(Guid uuid, Sector value)
    {
        ArgumentMismatchException.ThrowIfMismatch(uuid, value.Uuid, nameof(uuid));
        await sectors.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing company service
    /// </summary>
    [HttpDelete("api/sectors/{sectorId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task DeleteAsync(Guid sectorId)
    {
        await sectors.DeleteAsync(sectorId);
    }

    /// <summary>
    /// Retrieve statistical information about a filters set of Sectors.
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/sectors/stats"), Produces("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<Statistics<Sector>> RetrieveStatsAsync([FromQuery] FilterQuery query)
    {
        return await sectors.StatsAsync(query);
    }
}
