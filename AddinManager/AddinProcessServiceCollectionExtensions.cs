using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinManager {
    public static class AddinProcessServiceCollectionExtensions {
        public static IServiceCollection AddAddinProcess<TContract, TAddinProcessOptions>(this IServiceCollection services, AddinProcessRegistration<TContract, TAddinProcessOptions> registration, ServiceLifetime lifetime)
            where TContract: class
            where TAddinProcessOptions : AddinProcessOptions {
            switch (lifetime) {
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IAddinProcess<TContract>, AddinProcess<TContract>>();
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IAddinProcess<TContract>, AddinProcess<TContract>>();
                    break;
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IAddinProcess<TContract>, AddinProcess<TContract>>();
                    break;
                default:
                    throw new Exception("lifetime");
            }

            services.AddSingleton(registration);
            return services;
        }
    }
}