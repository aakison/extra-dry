#nullable enable

using ExtraDry.Server;
using ExtraDry.Server.EF;
using ExtraDry.Swashbuckle;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Data;
using Sample.Server.Security;
using System.Text.Json.Serialization;

namespace Sample.Server {

    /// <summary>
    /// Generated startup object.
    /// </summary>
    public class Startup {

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
                .AddJsonOptions(j => {
                    j.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddRazorPages();
            services.AddSwaggerGen(openapi => {
                openapi.SwaggerDoc("sample-api", new OpenApiInfo {
                    Version = "v1",
                    Title = "Sample API",
                    Description = @"A sample API for Blazor.ExtraDry",
                });
                openapi.SwaggerDoc("reference-codes", new OpenApiInfo {
                    Version = "v1",
                    Title = "Reference Codes",
                    Description = @"A sample API for Blazor.ExtraDry",
                });
                foreach(var docfile in new string[] { "Sample.Shared.xml", "Sample.Server.xml", "ExtraDry.Core.Xml" }) {
                    var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
                    openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
                }
                openapi.AddSecurityDefinition("http", new OpenApiSecurityScheme {
                    Description = "Basic",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                });
                openapi.OperationFilter<SignatureImpliesStatusCodes>();
                openapi.OperationFilter<BasicAuthOperationFilter>();
                openapi.OperationFilter<QueryDocumentationOperationFilter>();
            });

            services.AddAuthentication("WorthlessAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("WorthlessAuthentication", null);
            services.AddAuthorizationCore(options => SamplePolicies.AddAuthorizationOptions(options));
            services.AddSingleton<IAuthorizationHandler, SampleAccessHandler>();

            services.AddHttpContextAccessor();

            //services.AddDbContext<SampleContext>(opt => opt.UseInMemoryDatabase("sample"));
            services.AddScoped(services => {
                var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
                //var connectionString = Configuration.GetConnectionString("Sample");
                var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
                var context = new SampleContext(dbOptionsBuilder.Options);
                var accessor = services.GetService<IHttpContextAccessor>();
                if(accessor == null) {
                    throw new Exception("Need HTTP Accessor for VersionInfoAspect");
                }
                _ = new VersionInfoAspect(context, accessor);
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
            
            services.AddScoped<IEntityResolver<Sector>>(e => e.GetService<SectorService>()!);
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
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/sample-api/swagger.json", "Sample API");
                c.SwaggerEndpoint("/swagger/reference-codes/swagger.json", "Reference Codes");
                c.InjectStylesheet("/css/swagger-ui-extensions.css");
                c.InjectJavascript("/js/swagger-ui-extensions.js");
                c.DocumentTitle = "Sample Blazor.ExtraDry APIs";
                c.EnableTryItOutByDefault();
            });
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

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
}
