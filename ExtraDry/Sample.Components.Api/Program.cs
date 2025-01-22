using ExtraDry.Server;
using ExtraDry.Core;
using ExtraDry.Server.EF;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Models;
using Sample.Components.Api;
using Sample.Components.Api.Options;
using Sample.Components.Api.Security;
using Sample.Components.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Collect options, all stored in ApiOptions for both dependency injection and local use.
var apiOptions = new ApiOptions();
builder.Configuration.Bind(ApiOptions.SectionName, apiOptions);
builder.Services.AddSingleton(apiOptions);

// Add ExtraDry services, no sort key as CosmosDB is already stable.
builder.Services.AddExtraDry(options => {
    options.Stabilization = SortStabilization.None;
});

// Add the actual API endpoints
builder.Services.AddControllers();

// Add services for configuring Swagger and SwaggerUI (nee Swashbuckle)
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddRevisionAspect();
builder.Services.AddAuditAspect();
builder.Services.AddCosmos<ComponentContext>(apiOptions.CosmosDb.ConnectionString, apiOptions.CosmosDb.DatabaseName);

builder.Services.AddAuthentication()
    .AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAbacExtensions();
builder.Services.AddAuthorizationBuilder()
    //.AddPolicy(Policies.User, policy => policy.RequireRouteMatchesClaim("tenant", ["stakeholder", "manager", "vendor"], ClaimValueMatch.LastPath, roleOverrides: ["admin", "agent"]))
    .AddPolicy(Policies.User, policy => policy.AddRbacRequirement("User"))
    .AddPolicy(Policies.Admin, policy => policy.RequireRole("admin"))
    .AddPolicy(Policies.Agent, policy => policy.RequireRole("agent"))
    .AddPolicy(Policies.AdminOrAgent, policy => policy.RequireAssertion(e => e.User.IsInRole("admin") || e.User.IsInRole("agent")));

builder.Services.AddSingleton(sp => {
    var options = sp.GetRequiredService<ApiOptions>();
    var cosmos = new CosmosClient(options.CosmosDb.Endpoint, options.CosmosDb.AuthKey);
    return cosmos;
});
builder.Services
    .AddHealthChecks()
    .AddCheck<ApiHealthCheck>("api")
    .AddAzureCosmosDB();

builder.Services.AddScoped<RuleEngine>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ComponentService>();

var host = builder.Build();

// Configure the HTTP request pipeline.
if(host.Environment.IsDevelopment()) {
    host.UseSwagger();
    host.UseSwaggerUI();
}

// HealthCheck middleware
host.UseHealthChecks("/healthcheck", new HealthCheckOptions() {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
host.MapHealthChecks("/healthcheck");

host.UseHttpsRedirection();
host.UseAuthentication();
host.UseAuthorization();
host.MapControllers();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogProperties(apiOptions);

host.Run();
