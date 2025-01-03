using Microsoft.AspNetCore.Builder;

namespace ExtraDry.Server;

public static class ExtraDryApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExtraDry(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationResponse>();
    }
}
