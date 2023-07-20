using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server;

public static class ExtraDryServiceCollectionExtensions {

    public static IServiceCollection AddExtraDry(this IServiceCollection service, Action<ExtraDryOptions>? options = default)
    {
        options ??= (opts => { });

        service.Configure(options);
        return service;
    }
}
