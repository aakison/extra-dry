using ExtraDry.Server;
using ExtraDry.Server.EF;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sample.Components.Api;
using Sample.Components.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.AddScoped(services => {
    var optionsBuilder = new DbContextOptionsBuilder<ComponentContext>()
        .UseCosmos("https://localhost:8081",
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            "Components");
    var context = new ComponentContext(optionsBuilder.Options);

    var accessor = services.GetRequiredService<IHttpContextAccessor>();
    _ = new VersionInfoAspect(context, accessor);

    return context;
});
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy(Policies.User, policy => policy.RequireUserName("Adrian"))
  .AddPolicy(Policies.Admin, policy => policy.RequireRole("admin"))
  .AddPolicy(Policies.Agent, policy => policy.RequireRole("agent"));


//builder.Services.AddHealthChecks()
//    // Add a health check for a SQL Server database
//    .AddCheck("CosmosDB-check",
//        new CosmosDbHealthCheckBuilderExtensions(builder.Configuration["ConnectionString"]),
//        HealthStatus.Unhealthy,
//        new string[] { "orderingdb" });
builder.Services.AddSingleton(sp => new CosmosClient(
        "https://localhost:8081/", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));
builder.Services.AddHealthChecks().AddAzureCosmosDB();

builder.Services.AddScoped<RuleEngine>();
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<ComponentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HealthCheck middleware
app.UseHealthChecks("/healthcheck", new HealthCheckOptions() {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/healthcheck");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
