using Microsoft.AspNetCore.Http;

namespace ExtraDry.Server.EF;

public class RevisionAspect(
    IHttpContextAccessor httpContextAccessor,
    RevisionAspectOptions options)
    : IDbAspect
{
    public void EntitiesChanging(EntitiesChanged args)
    {
        // no-op
    }

    public void EntitiesChanged(EntitiesChanged args)
    {
        var hasExclusedRole = options.ExcludedRoles.Any(e => httpContextAccessor.HttpContext?.User?.IsInRole(e) ?? false);
        if(hasExclusedRole) {
            return;
        }
        var entities = args.EntitiesAdded.Union(args.EntitiesModified);
        var username = GetClaim(options.UsernameClaim) ?? options.UsernameDefault;
        var timestamp = args.Timestamp;
        foreach(var entity in entities) {
            if(entity is IRevisioned revisioned) {
                revisioned.Revision.User = username;
                revisioned.Revision.Timestamp = timestamp;
            }
        }
    }

    private string? GetClaim(string type) => options.StrictClaimMatch
        ? httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == type)?.Value
        : httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type.EndsWith(type, StringComparison.OrdinalIgnoreCase))?.Value;
}
