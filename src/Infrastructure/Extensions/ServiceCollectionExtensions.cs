using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Synaplic.Inventory.Application.Interfaces.Repositories;
using Synaplic.Inventory.Application.Interfaces.Services.Storage;
using Synaplic.Inventory.Application.Interfaces.Services.Storage.Provider;
using Synaplic.Inventory.Application.Serialization.JsonConverters;
using Synaplic.Inventory.Infrastructure.Repositories;
using Synaplic.Inventory.Infrastructure.Services.Storage;
using Synaplic.Inventory.Application.Serialization.Options;
using Synaplic.Inventory.Infrastructure.Services.Storage.Provider;
using Synaplic.Inventory.Application.Serialization.Serializers;
using Synaplic.Inventory.Shared.Interfaces.Serialization.Serializers;

namespace Synaplic.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // TODO : Add Repositories
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

     

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}