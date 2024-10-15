using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Spa.Backend.SampleData;

/// <summary>
/// Manages collections of sectors for companies.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.ReferenceCodes)]
[ApiExceptionStatusCodes]
[SuppressMessage("Usage", "DRY1018:API Controller Classes should not directly use DbContext.", Justification = "Sample Setup")]
public class SampleDataController(
    SampleContext database, 
    SampleDataService sampleData)
{

    /// <summary>
    /// Load the set of sample data, idempotent so allowed to be anonymous.
    /// </summary>
    [HttpPost("api/load-sample-data")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC, not REST.")]
    [SuppressMessage("ApiUsage", "DRY1114:HttpPost actions should Produces ResourceReference output.", Justification = "Not RESTful")]
    public async Task CreateBaseDataRpcAsync()
    {
        var shouldLoadSamples = !await database.Sectors.AnyAsync();
        if(shouldLoadSamples) {
            sampleData.PopulateTemplates();
            sampleData.PopulateServices();
            sampleData.PopulateCompanies(50);
            await sampleData.PopulateEmployeesAsync(5000);
            sampleData.PopulateContents();
        }
        // Idempotent calls....
        await sampleData.PopulateRegionsAsync(["AU", "CA", "GB", "NZ", "US"], true, false);
    }

    /// <summary>
    /// Load the sample data with regions.
    /// </summary>
    /// <param name="countryFilter">A list of 2 character ISO 3166-2 country code to filter the data load.</param>
    /// <param name="includeSubdivisions"></param>
    /// <param name="includeLocalities"></param>
    /// <returns></returns>
    [HttpPost("api/load-sample-data/regions")]
    [AllowAnonymous]
    [Produces("application/json"), Consumes("application/json")]
    [SuppressMessage("ApiUsage", "DRY1114:HttpPost actions should Produces ResourceReference output.", Justification = "Not RESTful")]
    public async Task<RegionLoadStats> CreateRegionsRpcAsync(string[] countryFilter, bool includeSubdivisions, bool includeLocalities)
    {
        return await sampleData.PopulateRegionsAsync(countryFilter, includeSubdivisions, includeLocalities);
    }
}
