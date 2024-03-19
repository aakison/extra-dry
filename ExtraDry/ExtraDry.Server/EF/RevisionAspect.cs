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
public class RevisionAspect {

    /// <summary>
    /// Creates the aspect which hooks into the AspectDbContext to automatically update VersionInfo properties of EF models.
    /// </summary>
    public RevisionAspect(AspectDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        context.EntitiesChangedEvent += Context_EntitiesChanged;
        this.httpContextAccessor = httpContextAccessor;
    }

    private void Context_EntitiesChanged(object sender, EntitiesChangedEventArgs args)
    {
        var entities = args.EntitiesAdded.Union(args.EntitiesModified);
        var username = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";
        foreach(var entity in entities) {
            if(entity is IRevisioned revisioned) {
                revisioned.Revision.User = username;
                revisioned.Revision.Timestamp = timestamp;
            }
            if(entity is IAudited audited) {
                audited.Audit.User = username;
                audited.Audit.Timestamp = timestamp;
            }
        }
    }

    private readonly DateTime timestamp = DateTime.UtcNow;

    private readonly IHttpContextAccessor httpContextAccessor;

}
