using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinManager {
    public static class AddinProcessServiceCollectionExtensions {
        public static IServiceCollection AddAddinProcess<TAddinProcessOptions>(this IServiceCollection services, AddinProcessRegistration<TAddinProcessOptions> registration, ServiceLifetime lifetime)
            where TAddinProcessOptions : AddinProcessOptions {
            switch (lifetime) {
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IAddinProcess, AddinProcess>();
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IAddinProcess, AddinProcess>();
                    break;
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IAddinProcess, AddinProcess>();
                    break;
                default:
                    throw new Exception("lifetime");
            }

            services.AddSingleton(registration);
            return services;
        }
    }
}