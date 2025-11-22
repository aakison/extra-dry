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
using Scalar.AspNetCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Ensure Kestrel uses the correct HTTPS development certificate
if(builder.Environment.IsDevelopment()) {
    builder.WebHost.ConfigureKestrel(options => {
        options.ConfigureHttpsDefaults(httpsOptions => {
            // Find the ASP.NET Core development certificate by friendly name
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                "localhost",
                validOnly: false);
            
            // Filter to only ASP.NET Core HTTPS development certificates
            var devCert = certs
                .OfType<X509Certificate2>()
                .FirstOrDefault(c => c.FriendlyName == "ASP.NET Core HTTPS development certificate");
            
            if(devCert != null) {
                httpsOptions.ServerCertificate = devCert;
            }
            store.Close();
        });
    });
}

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

// Add services for configuring OpenAPI and Scalar UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options => {
    options.AddDocumentTransformer((document, context, cancellationToken) => {
        document.Info = new OpenApiInfo {
            Version = "v1",
            Title = "Sample Components API",
            Description = "API for managing components"
        };
        document.Servers = [new OpenApiServer { Url = "/" }];
        
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        };
        
        return Task.CompletedTask;
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
    host.UseDeveloperExceptionPage();
    host.MapOpenApi();
    host.MapScalarApiReference(options => {
        options
            .WithTitle("Sample Components API")
            .WithTheme(ScalarTheme.Default)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
else {
    host.UseExceptionHandler("/Error");
    host.UseHsts();
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
