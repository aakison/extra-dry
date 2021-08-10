#nullable enable

using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Blazor.ExtraDry {

    public static class ServiceCollectionExtensions {

        /// <summary>
        /// Adds a strongly typed CrudService`T to the service collection.
        /// </summary>
        public static IServiceCollection AddCrudService<T>(this IServiceCollection services, string endpointTemplate)
        {
            var client = services.BuildServiceProvider().GetService<HttpClient>();
            var service = new CrudService<T>(client!, endpointTemplate);
            return services.AddScoped(e => service);
        }

        /// <summary>
        /// Adds a strongly typed ListService`FilteredCollection`T to the service collection.
        /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
        /// </summary>
        public static IServiceCollection AddFilteredListService<T>(this IServiceCollection services, string endpointTemplate)
        {
            var client = services.BuildServiceProvider().GetService<HttpClient>();
            var service = new ListService<FilteredCollection<T>, T>(client!, endpointTemplate);
            services.AddScoped(e => service);
            services.AddScoped(e => (IListService<T>)service);
            services.AddScoped(e => (IOptionProvider<T>)service);
            return services;
        }

        /// <summary>
        /// Adds a strongly typed ListService`PagedCollection`T to the service collection.
        /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
        /// </summary>
        public static IServiceCollection AddPagedListService<T>(this IServiceCollection services, string endpointTemplate)
        {
            var client = services.BuildServiceProvider().GetService<HttpClient>();
            var service = new ListService<PagedCollection<T>, T>(client!, endpointTemplate);
            services.AddScoped(e => service);
            services.AddScoped(e => (IListService<T>)service);
            services.AddScoped(e => (IOptionProvider<T>)service);
            return services;
        }
    }
}
