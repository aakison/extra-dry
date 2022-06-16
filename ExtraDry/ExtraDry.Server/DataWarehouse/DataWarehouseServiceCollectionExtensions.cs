using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtraDry.Server.DataWarehouse;

public static class DataWarehouseServiceCollectionExtensions {

    public static IServiceCollection AddDataFactory<TModel, TOltpContext, TOlapContext>(this IServiceCollection services, Action<DataFactoryOptions>? dataFactoryOptionsAction = null)
        where TModel : WarehouseModel
        where TOltpContext : DbContext
        where TOlapContext : WarehouseContext
    {
        services.AddScoped(provider => {
            var model = provider.GetRequiredService<TModel>();
            var oltp = provider.GetRequiredService<TOltpContext>();
            var olap = provider.GetRequiredService<TOlapContext>();
            var logger = provider.GetService<ILogger<DataFactory>>();
            var options = new DataFactoryOptions();
            dataFactoryOptionsAction?.Invoke(options);
            return new DataFactory<TModel, TOltpContext, TOlapContext>(model, oltp, olap, logger, options);
        });
        return services;
    }

}
