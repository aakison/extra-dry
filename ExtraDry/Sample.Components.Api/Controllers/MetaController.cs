using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Sample.Components.Api.Options;
using Sample.Components.Api.Security;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Sample.Components.Api.Controllers;

/// <summary>
/// Endpoints for managing the system, typically called by Admins.
/// </summary>
[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
[SuppressMessage("Usage", "DRY1018:API Controller Classes should not directly use DbContext.", Justification = "Special initialization case.")]
public class MetaController(
    ILogger<TenantController> logger,
    CosmosClient cosmos,
    ComponentContext database,
    ApiOptions options)
{
    /// <summary>
    /// For a new system, initialized the database to prepare for use. While this is a one-time
    /// operation, it is idempotent and multiple calls will not hurt.
    /// </summary>
    /// <returns></returns>
    [HttpPost(":initialize")]
    [Authorize(Policies.Admin)]
    [SuppressMessage("Usage", "DRY1104:Http Verbs should be named with their CRUD counterparts", Justification = "RPC Style")]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC Style")]
    [SuppressMessage("ApiUsage", "DRY1114:HttpPost actions should Produces ResourceReference output.", Justification = "RPC Style")]
    public async Task InitializeCommand()
    {
        try {
            logger.LogStaticInformation("Initializing database");
            if(options == null) {
                logger.LogStaticInformation("");
                return;
            }
            await cosmos.CreateDatabaseIfNotExistsAsync(options.CosmosDb.DatabaseName, 400);
            await database.Database.EnsureCreatedAsync();
        }
        catch(Exception ex) {
            logger.LogStaticInformation(ex.Message);
        }
    }

    /// <summary>
    /// Similar to a health check, this endpoint returns the version of the API, but requires
    /// authorization.
    /// </summary>
    [HttpGet("/version")]
    [Authorize(Policies.Admin)]
    [Produces("application/json")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Pattern for controllers is instance methods.")]
    public Version RetrieveVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version ?? new();
    }
}
