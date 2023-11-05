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
    public SampleDataController(SampleContext sampleContext, RegionService regionService, SampleDataService sampleDataService)
    {
        context = sampleContext;
        regions = regionService;
        samples = sampleDataService;
    }

    /// <summary>
    /// Load the set of sample data, idempotent so allowed to be anonymous.
    /// </summary>
    [HttpPost("api/load-sample-data")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC, not REST.")]
    public async Task CreateBaseDataRpcAsync()
    {
        var shouldLoadSamples = !await context.Sectors.AnyAsync();
        if(shouldLoadSamples) {
            samples.PopulateTemplates();
            samples.PopulateServices();
            samples.PopulateCompanies(50);
            samples.PopulateEmployees(5000);
            samples.PopulateContents();
            await samples.PopulateRegionsAsync(regions);
            //await sampleData.PopulateArbitaryRegions(regions, 5, 10, 10);
        }
    }

    /// <summary>
    /// Load the sample data with regions.
    /// </summary>
    /// <param name="country">A 2 character ISO 3166-2 country code to filter the data load.</param>
    /// <param name="includeSubdivisions"></param>
    /// <param name="includeLocalities"></param>
    /// <returns></returns>
    [HttpPost("api/load-sample-data/regions")]
    [AllowAnonymous]
    public async Task<RegionLoadStats> CreateRegionsAsync(string? country, bool includeSubdivisions, bool includeLocalities)
    {
        var sampleData = new SampleDataService();
        return await sampleData.PopulateRegionsAsync(regions, country, includeSubdivisions, includeLocalities);
    }

    private readonly SampleContext context;

    private readonly RegionService regions;

    private readonly SampleDataService samples;

}
