using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace ExtraDry.Server;

public static class ExtraDryServiceCollectionExtensions {

    public static IServiceCollection AddAuthorizationResponse(this IServiceCollection service, Action<AuthorizationResponseOptions>? options = default)
    {
        options ??= (opts => { });

        service.Configure(options);
        return service;
    }
}


