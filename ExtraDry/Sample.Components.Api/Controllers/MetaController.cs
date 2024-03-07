using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Sample.Components.Api.Controllers;

[ApiController]
[SkipStatusCodePages]
[ApiExceptionStatusCodes]
//[ApiExplorerSettings(GroupName = "Components")]
public class MetaController(
    ILogger<TenantController> logger,
    ComponentContext database) 
{

    [HttpPost(":initialize")]
    [AllowAnonymous]
    [SuppressMessage("Usage", "DRY1104:Http Verbs should be named with their CRUD counterparts", Justification = "RPC Style")]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "RPC Style")]
    [SuppressMessage("ApiUsage", "DRY1114:HttpPost actions should Produces ResourceReference output.", Justification = "RPC Style")]
    public async Task Init()
    {
        //var dbr = await cosmos.CreateDatabaseIfNotExistsAsync("entities", 400);
        //var db = dbr.Database;
        //var containerResponse = await db.CreateContainerIfNotExistsAsync("tenants", "/slug");
        //var container = containerResponse.Container;
        logger.LogStaticInformation("Initializing database");
        await database.Database.EnsureDeletedAsync();
        await database.Database.EnsureCreatedAsync();
    }

    [HttpGet("/version")]
    [Authorize(Policies.User)]
    [Produces("application/json")]
    public Version RetrieveVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version ?? new();
    }   

}
