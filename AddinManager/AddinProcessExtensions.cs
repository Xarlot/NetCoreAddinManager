using System;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public static class AddinProcessExtensions {
        public static IServiceCollection AddAddinProcess<TContract>(this IServiceCollection services, string name, Runtime runtime, ServiceLifetime lifetime) where TContract: class =>
            services.AddAddinProcess<TContract>(name, (_, options) => { options.Runtime = runtime; }, lifetime);

        public static IServiceCollection AddAddinProcess<TContract>(this IServiceCollection services, string name, Action<IServiceProvider, AddinProcessOptions> configureOptions, ServiceLifetime lifetime) 
            where TContract: class {
            services.AddAddinProcess(new AddinProcessRegistration<TContract, AddinProcessOptions>(name, 
                (serviceProvider, options) => {
                    IAddinProcess<TContract> CreateHandler(string processName) => new AddinProcess<TContract>(options.Runtime);
                    if (options.Lifetime != ServiceLifetime.Singleton) 
                        return CreateHandler(name);
                    var addinProcessPool = serviceProvider.GetRequiredService<IAddinProcessPool>();
                    return (IAddinProcess<TContract>)addinProcessPool.GetOrAdd(name, CreateHandler);
                }, configureOptions), lifetime);
            return services;
        }
    }
}