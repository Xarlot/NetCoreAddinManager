using AddinManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinHost {
    public static class PluginClientServiceCollectionExtensions {
        public static IServiceCollection AddAddinProcess<TContract, TPluginClientOptions>(this IServiceCollection services, PluginClientRegistration<TContract, TPluginClientOptions> registration)
            where TContract : class where TPluginClientOptions : PluginClientOptions {
            services.TryAddScoped<IPluginClientFactory<TContract>, PluginClientFactory<TContract, TPluginClientOptions>>();
            services.AddSingleton(registration);
            return services;
        }
    }
}