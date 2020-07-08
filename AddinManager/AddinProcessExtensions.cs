using System;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public static class AddinProcessExtensions {
        public static IServiceCollection AddAddinProcess(this IServiceCollection services, string name, Runtime runtime, ServiceLifetime lifetime) =>
            services.AddAddinProcess(name, (_, options) => { options.Runtime = runtime; }, lifetime);

        public static IServiceCollection AddAddinProcess(this IServiceCollection services, string name, Action<IServiceProvider, AddinProcessOptions> configureOptions, ServiceLifetime lifetime) {
            services.AddAddinProcess(new AddinProcessRegistration<AddinProcessOptions>(name, (_, options) => new AddinProcess(options.Runtime), configureOptions), lifetime);
            return services;
        }
    }
}