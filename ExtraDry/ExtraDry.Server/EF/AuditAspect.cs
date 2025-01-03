using Microsoft.AspNetCore.Http;

namespace ExtraDry.Server.EF;

/// <summary>
/// A database aspect that automatically populates the audit fields on entities that implement
/// IAudited.
/// </summary>
public class AuditAspect(
    IHttpContextAccessor httpContextAccessor,
    AuditAspectOptions options)
    : IDbAspect
{
    /// <inheritdoc />
    public void EntitiesChanging(EntitiesChanged args)
    {
        // no-op
    }

    /// <inheritdoc />
    public void EntitiesChanged(EntitiesChanged args)
    {
        var entities = args.EntitiesAdded.Union(args.EntitiesModified);
        var username = GetClaim(options.UsernameClaim) ?? options.UsernameDefault;
        var timestamp = args.Timestamp;
        foreach(var entity in entities) {
            if(entity is IAudited audited) {
                audited.Audit.User = username;
                audited.Audit.Timestamp = timestamp;
            }
        }
    }

    private string? GetClaim(string type) => options.StrictClaimMatch
        ? httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == type)?.Value
        : httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type.EndsWith(type, StringComparison.OrdinalIgnoreCase))?.Value;
}
