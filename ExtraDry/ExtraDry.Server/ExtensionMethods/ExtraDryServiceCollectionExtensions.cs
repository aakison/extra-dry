using ExtraDry.Server.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server;

public static class ExtraDryServiceCollectionExtensions {

    /// <summary>
    /// Add the Core ExtraDry functionality to the server side application
    /// </summary>
    public static IServiceCollection AddExtraDry(this IServiceCollection service, Action<ExtraDryOptions>? options = default)
    {
        options ??= (opts => { });

        service.Configure(options);

        StaticServiceProvider.Initialize(service);

        return service;
    }
}
