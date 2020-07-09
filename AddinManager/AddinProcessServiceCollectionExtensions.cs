using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinManager {
    public static class AddinProcessServiceCollectionExtensions {
        public static IServiceCollection AddAddinProcess<TContract, TAddinProcessOptions>(this IServiceCollection services, AddinProcessRegistration<TContract, TAddinProcessOptions> registration, ServiceLifetime lifetime)
            where TContract: class
            where TAddinProcessOptions : AddinProcessOptions {
            services.TryAddSingleton<IAddinProcessPool, AddinProcessPool>();
            services.TryAddScoped<IAddinProcessFactory<TContract>, AddinProcessFactory<TContract, TAddinProcessOptions>>();
            services.AddSingleton(registration);
            return services;
        }
    }
}