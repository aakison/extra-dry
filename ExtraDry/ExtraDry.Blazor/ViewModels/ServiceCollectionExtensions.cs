#nullable enable

using ExtraDry.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace ExtraDry.Blazor {

    public static class ServiceCollectionExtensions {

        /// <summary>
        /// Adds a strongly typed CrudService`T to the service collection.
        /// </summary>
        public static IServiceCollection AddCrudService<T>(this IServiceCollection services, string endpointTemplate)
        {
            services.AddScoped(e => {
                var client = e.GetService<HttpClient>();
                var service = new CrudService<T>(client!, endpointTemplate);
                return service;
            });
            return services;
        }

        /// <summary>
        /// Adds a strongly typed ListService`FilteredCollection`T to the service collection.
        /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
        /// </summary>
        public static IServiceCollection AddFilteredListService<T>(this IServiceCollection services, string endpointTemplate)
        {
            services.AddScoped(e => {
                var client = e.GetService<HttpClient>();
                var service = new ListService<FilteredCollection<T>, T>(client!, endpointTemplate);
                return service;
            });
            services.AddScoped(e => {
                IListService<T> upcasted = e.GetService<ListService<FilteredCollection<T>, T>>()!;
                return upcasted;
            });
            services.AddScoped(e => {
                IOptionProvider<T> upcasted = e.GetService<ListService<FilteredCollection<T>, T>>()!;
                return upcasted;
            });
            return services;
        }

        /// <summary>
        /// Adds a strongly typed ListService`PagedCollection`T to the service collection.
        /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
        /// </summary>
        public static IServiceCollection AddPagedListService<T>(this IServiceCollection services, string endpointTemplate)
        {
            services.AddScoped(e => {
                var client = e.GetService<HttpClient>();
                var service = new ListService<PagedCollection<T>, T>(client!, endpointTemplate);
                return service;
            });
            services.AddScoped(e => {
                IListService<T> upcasted = e.GetService<ListService<PagedCollection<T>, T>>()!;
                return upcasted;
            });
            services.AddScoped(e => {
                IOptionProvider<T> upcasted = e.GetService<ListService<PagedCollection<T>, T>>()!;
                return upcasted;
            });
            return services;
        }
    }
}
