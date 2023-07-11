using Microsoft.AspNetCore.Builder;

namespace ExtraDry.Server;

public static class ExtraDryApplicationBuilderExtensions {

    public static IApplicationBuilder UseAuthorizationResponse(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationResponse>();
    }
}
