using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Server.SampleData;

/// <summary>
/// Manages collections of sectors for companies.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.ReferenceCodes)]
[ApiExceptionStatusCodes]
public class SampleDataController {

    /// <summary>
    /// Stanard DI Constructor
    /// </summary>
    [SuppressMessage("Usage", "DRY1012:API Controller Classes should not directly use DbContext.", Justification = "Temporary sample data controller.")]
    public SampleDataController(SampleContext sampleContext, SampleDataService sampleDataService)
    {
        context = sampleContext;
        samples = sampleDataService;
    }

    /// <summary>
    /// Load the set of sample data, idempotent so allowed to be anonymous.
    /// </summary>
    [HttpPost("api/load-sample-data")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC, not REST.")]
    [SuppressMessage("ApiUsage", "DRY1114:HttpPost actions should Produces ResourceReference output.", Justification = "Not RESTful")]
    public async Task CreateBaseDataRpcAsync()
    {
        var shouldLoadSamples = !await context.Sectors.AnyAsync();
        if(shouldLoadSamples) {
            samples.PopulateTemplates();
            samples.PopulateServices();
            samples.PopulateCompanies(50);
            samples.PopulateEmployees(5000);
            samples.PopulateContents();
        }
        // Idempotent calls....
        await samples.PopulateRegionsAsync(new string[] { "AU", "CA", "GB", "NZ", "US" }, true, false);
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
        return await samples.PopulateRegionsAsync(countryFilter, includeSubdivisions, includeLocalities);
    }

    private readonly SampleContext context;

    private readonly SampleDataService samples;

}
