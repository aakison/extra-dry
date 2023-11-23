using Microsoft.AspNetCore.Http;

namespace ExtraDry.Server.EF;

/// <summary>
/// Watches for changes on the database context and automatically applies version info where appropriate.
/// To use: 
///   1. Add to your EF model objects that need versioning (all of them?)
///   2. Inherit your EF DbContext from ExtraDry.Server.AspectDbContext
///   3. In Startup.cs, instantiate a VersionInfoAspect and register with the Context
/// E.g.
/// ```
///   services.AddHttpContextAccessor();
///   services.AddScoped(services => {
///     var connectionString = Configuration.GetConnectionString("Sample");
///     var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
///     var context = new SampleContext(dbOptionsBuilder.Options);
///     var accessor = services.GetService<IHttpContextAccessor>();
///     if(accessor == null) {
///       throw new Exception("Need HTTP Accessor for VersionInfoAspect");
///     }
///     _ = new VersionInfoAspect(context, accessor);
///     return context;
///   });
/// ```
/// </summary>
public class VersionInfoAspect {

    /// <summary>
    /// Creates the aspect which hooks into the AspectDbContext to automatically update VersionInfo properties of EF models.
    /// </summary>
    public VersionInfoAspect(AspectDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        context.EntitiesChangedEvent += Context_EntitiesChanged;
        VersionInfo.CurrentUsername = () => httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";
    }

    private void Context_EntitiesChanged(object sender, EntitiesChangedEventArgs args)
    {
        VersionInfo.ResetTimestamp();
        var versionType = typeof(VersionInfo);
        var entities = args.EntitiesAdded.Union(args.EntitiesModified);
        foreach(var entity in entities) {
            var property = entity.GetType().GetProperties().FirstOrDefault(e => e.PropertyType == versionType);
            if(property == null) {
                continue;
            }
            var version = property.GetValue(entity) as VersionInfo;
            version?.UpdateVersion();
        }
    }
}
