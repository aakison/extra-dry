#nullable enable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Server.Controllers;

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
    public SampleDataController(SampleContext sampleContext, RegionService regionService)
    {
        context = sampleContext;
        regions = regionService;
    }

    private readonly SampleContext context;
    private readonly RegionService regions;

    /// <summary>
    /// Load the set of sample data, idempotent so allowed to be anonymous.
    /// </summary>
    [HttpPost("api/load-sample-data")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC, not REST.")]
    public async Task CreateBaseDataRpcAsync()
    {
        var shouldLoadSamples = !await context.Sectors.AnyAsync();
        var sampleData = new DummyData();
        if(shouldLoadSamples) {
            sampleData.PopulateTemplates(context);
            sampleData.PopulateServices(context);
            sampleData.PopulateCompanies(context, 50);
            sampleData.PopulateEmployees(context, 5000);
            sampleData.PopulateContents(context);
            await sampleData.PopulateRegionsAsync(regions);
        }
    }
}
