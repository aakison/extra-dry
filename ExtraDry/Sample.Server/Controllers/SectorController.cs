using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Server.Controllers;

/// <summary>
/// Manages collections of sectors for companies.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class SectorController {
       
    /// <summary>
    /// Stanard DI Constructor
    /// </summary>
    public SectorController(SectorService sectorService)
    {
        sectors = sectorService;
    }

    /// <summary>
    /// Filtered list of all company services
    /// </summary>
    [HttpGet("api/sectors"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Sector>> List([FromQuery] FilterQuery query)
    {
        return await sectors.ListAsync(query);
    }

    /// <summary>
    /// Create a new global sector
    /// </summary>
    [HttpPost("api/sectors"), Consumes("application/json")]
    [Authorize(nameof(SectorController))]
    public async Task Create(Sector value)
    {
        await sectors.CreateAsync(value);
    }

    /// <summary>
    /// Retreive a specific company sector
    /// </summary>
    [HttpGet("api/sectors/{uuid}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Sector> Retrieve(Guid uuid)
    {
        return await sectors.RetrieveAsync(uuid);
    }

    /// <summary>
    /// Update an existing company sector
    /// </summary>
    [HttpPut("api/sectors/{sectorId}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid sectorId, Sector value)
    {
        if(sectorId != value?.Uuid) {
            throw new ArgumentMismatchException("ID in URI must match body.", nameof(sectorId));
        }
        await sectors.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing company service
    /// </summary>
    [HttpDelete("api/sectors/{sectorId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid sectorId)
    {
        await sectors.DeleteAsync(sectorId);
    }

    private readonly SectorService sectors;
}
