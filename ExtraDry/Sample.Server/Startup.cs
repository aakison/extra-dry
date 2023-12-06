using ExtraDry.Core.Models;
using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.EF;
using ExtraDry.Swashbuckle;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Server.SampleData;
using Sample.Server.Security;
using System.Text.Json.Serialization;

namespace Sample.Server;

/// <summary>
/// Generated startup object.
/// </summary>
public class Startup
{

    /// <summary>
    /// Standard startup constructor.
    /// </summary>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configuration for startup object.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container. 
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews()
            .AddExtraDry()
            .AddJsonOptions(j => {
                j.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddRazorPages();
        services.AddSwaggerGen(openapi => {
            openapi.AddExtraDry(options => {
                options.XmlComments.Files.Add("Sample.Shared.xml");
                options.XmlComments.Files.Add("Sample.Server.Xml");
            });
            openapi.SwaggerDoc(ApiGroupNames.SampleApi, new OpenApiInfo {
                Version = "v1",
                Title = "Sample APIs",
                Description = @"A sample API for Blazor.ExtraDry",
            });
            openapi.SwaggerDoc(ApiGroupNames.ReferenceCodes, new OpenApiInfo {
                Version = "v1",
                Title = "Reference Codes",
                Description = @"A sample API for Blazor.ExtraDry",
            });
            openapi.SwaggerDoc(ApiGroupNames.InternalUseOnly, new OpenApiInfo {
                Version = "v1",
                Title = "Internal Use Only",
                Description = @"An Internal Use Only set of APIs for our own interfaces.",
            });

            openapi.AddSecurityDefinition("http", new OpenApiSecurityScheme {
                Description = "Basic",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
            });
            openapi.OperationFilter<BasicAuthOperationFilter>();
        });

        services.AddAuthentication("WorthlessAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("WorthlessAuthentication", null);
        services.AddAuthorizationCore(options => SamplePolicies.AddAuthorizationOptions(options));
        services.AddSingleton<IAuthorizationHandler, SampleAccessHandler>();

        services.AddHttpContextAccessor();

        services.AddServiceBusQueue<EntityMessage>(options => {
            options.ConnectionStringKey = "WebAppServiceBus";
            options.QueueName = "warehouse-update";
        });

        services.AddScoped(services => {
            var connectionString = Configuration.GetConnectionString("WebAppOltpDatabase");
            var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString, config => config.UseHierarchyId());
            var context = new SampleContext(dbOptionsBuilder.Options);

            var accessor = services.GetRequiredService<IHttpContextAccessor>();
            _ = new VersionInfoAspect(context, accessor);

            var logger = services.GetRequiredService<ILogger<DataWarehouseAspect>>();
            var queue = services.GetRequiredService<ServiceBusQueue<EntityMessage>>();
            _ = new DataWarehouseAspect(context, queue, logger);

            //_ = new SearchIndexAspect(context, services.GetService<SearchService>());
            return context;
        });

        services.AddScoped<EmployeeService>();
        services.AddScoped<CompanyService>();
        services.AddScoped<ContentsService>();
        services.AddScoped<SectorService>();
        services.AddScoped<BlobService>();
        services.AddScoped<RuleEngine>();
        services.AddScoped<RegionService>();
        services.AddScoped<TemplateService>();
        services.AddScoped<SampleDataService>();

        services.AddScoped<IEntityResolver<Sector>, SectorService>();
        services.AddScoped<IExpandoSchemaResolver, TemplateService>();

        services.AddFileValidation();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(swagger => {
            swagger.UseExtraDry();
            swagger.SwaggerEndpoint($"/swagger/{ApiGroupNames.SampleApi}/swagger.json", "Sample APIs");
            swagger.SwaggerEndpoint($"/swagger/{ApiGroupNames.ReferenceCodes}/swagger.json", "Reference Codes");
            swagger.InjectStylesheet("/css/swagger-ui-extensions.css");
            swagger.InjectJavascript("/js/swagger-ui-extensions.js");
            swagger.DocumentTitle = "Sample Blazor.ExtraDry APIs";
            swagger.EnableDeepLinking();
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapRazorPages();
            endpoints.MapControllers();

            // Calls to API endpoints shouldn't fallback to Blazor
            endpoints.Map("api/{**slug}", context => {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return Task.CompletedTask;
            });
            endpoints.MapFallbackToFile("{**slug}", "index.html");
        });

    }

}
