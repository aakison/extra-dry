using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtraDry.Server.DataWarehouse;

public static class DataWarehouseServiceCollectionExtensions
{
    public static IServiceCollection AddDataFactory<TModel, TOltpContext, TOlapContext>(this IServiceCollection services, Action<DataFactoryOptions>? options = null)
        where TModel : WarehouseModel
        where TOltpContext : DbContext
        where TOlapContext : WarehouseContext
    {
        services.AddScoped(provider => {
            var model = provider.GetRequiredService<TModel>();
            var oltp = provider.GetRequiredService<TOltpContext>();
            var olap = provider.GetRequiredService<TOlapContext>();
            var logger = provider.GetRequiredService<ILogger<DataFactory>>();
            var opt = new DataFactoryOptions();
            options?.Invoke(opt);
            return new DataFactory<TModel, TOltpContext, TOlapContext>(model, oltp, olap, logger, opt);
        });
        return services;
    }

    public static IServiceCollection AddServiceBusQueue<TMessage>(this IServiceCollection services, Action<ServiceBusSenderOptions>? options = null)
    {
        services.AddSingleton(provider => {
            var config = provider.GetService<IConfiguration>();
            var opt = new ServiceBusSenderOptions();
            options?.Invoke(opt);
            return new ServiceBusQueue<TMessage>(opt, config);
        });
        return services;
    }
}
