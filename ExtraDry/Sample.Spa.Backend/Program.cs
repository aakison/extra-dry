using ExtraDry.Core.Models;
using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Spa.Backend;
using Sample.Spa.Backend.Components;
using Sample.Spa.Backend.SampleData;
using Sample.Spa.Backend.Security;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers()
    .AddJsonOptions(e => {
        e.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddExtraDry();

builder.Services.AddOpenApi(options => {
    options.AddDocumentTransformer((document, context, cancellationToken) => {
        document.Info = new OpenApiInfo {
            Version = "v1",
            Title = "Sample Blazor.ExtraDry APIs",
            Description = "A sample API for Blazor.ExtraDry"
        };
        document.Servers = [new OpenApiServer { Url = "/" }];

        document.SecurityRequirements.Add(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "http"
                    }
                },
                Array.Empty<string>()
            }
        });

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes["http"] = new OpenApiSecurityScheme {
            Description = "Basic",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
        };

        return Task.CompletedTask;
    });

    options.AddOperationTransformer<BasicAuthOperationFilter>();
});

builder.Services.AddAuthentication("WorthlessAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("WorthlessAuthentication", null);
builder.Services.AddAuthorizationCore(SamplePolicies.AddAuthorizationOptions);
builder.Services.AddSingleton<IAuthorizationHandler, SampleAccessHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddServiceBusQueue<EntityMessage>(options => {
    options.ConnectionStringKey = "WebAppServiceBus";
    options.QueueName = "warehouse-update";
});

builder.Services.AddScoped(services => {
    var connectionString = builder.Configuration.GetConnectionString("WebAppOltpDatabase");
    var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString, config => config.UseHierarchyId());
    var context = new SampleContext(dbOptionsBuilder.Options, []);

    var logger = services.GetRequiredService<ILogger<DataWarehouseAspect>>();
    var queue = services.GetRequiredService<ServiceBusQueue<EntityMessage>>();
    _ = new DataWarehouseAspect(context, queue, logger);

    //_ = new SearchIndexAspect(context, services.GetService<SearchService>());
    return context;
});

builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<ContentsService>();
builder.Services.AddScoped<SectorService>();
builder.Services.AddScoped<InMemoryBlobService>();
builder.Services.AddScoped<RuleEngine>();
builder.Services.AddScoped<RegionService>();
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<SampleDataService>();

builder.Services.AddScoped<IEntityResolver<Sector>, SectorService>();
builder.Services.AddScoped<IExpandoSchemaResolver, TemplateService>();

builder.Services.AddRevisionAspect();

builder.Services.AddFileValidation(options => {
    options.ValidateFilename = ValidationCondition.Always;
    options.ValidateExtension = ValidationCondition.Never;
    options.ValidateContent = ValidationCondition.ServerSide;
    options.ExtensionWhitelist.Add("cs");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see
    // https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapOpenApi();
app.MapScalarApiReference(options => {
    options
        .WithTitle("Sample Blazor.ExtraDry APIs")
        .WithTheme(ScalarTheme.Default)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Sample.Spa.Client._Imports).Assembly);
app.MapControllers();

app.Run();
