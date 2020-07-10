using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinManager {
    public static class AddinProcessExtensions {
        public static IServiceCollection AddAddinProcess<TContract>(this IServiceCollection services, string name, Runtime runtime) where TContract: class =>
            services.AddAddinProcess<TContract>(name, (_, options) => { options.Runtime = runtime; });

        public static IServiceCollection AddAddinProcess<TContract>(this IServiceCollection services, string name, Action<IServiceProvider, AddinProcessOptions> configureOptions) 
            where TContract: class {
            services.AddAddinProcess(new AddinProcessRegistration<TContract, AddinProcessOptions>(name, 
                (serviceProvider, options) => {
                    IAddinProcess CreateHandler(string processName) => new AddinProcess(options.Runtime, options.Dependency.assemblyName, options.Dependency.searchPattern);
                    if (options.Lifetime != ServiceLifetime.Singleton) 
                        return CreateHandler(name);
                    var addinProcessPool = serviceProvider.GetRequiredService<IAddinProcessPool>();
                    return addinProcessPool.GetOrAdd(name, CreateHandler);
                }, configureOptions));
            return services;
        }
        public static IServiceCollection AddAddinProcess<TContract, TAddinProcessOptions>(this IServiceCollection services, AddinProcessRegistration<TContract, TAddinProcessOptions> registration)
            where TContract: class
            where TAddinProcessOptions : AddinProcessOptions {
            services.TryAddSingleton<IAddinProcessPool, AddinProcessPool>();
            services.TryAddScoped<IAddinProcessFactory<TContract>, AddinProcessFactory<TContract, TAddinProcessOptions>>();
            services.AddSingleton(registration);
            return services;
        }
    }
}