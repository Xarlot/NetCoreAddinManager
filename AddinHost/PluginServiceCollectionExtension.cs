using System;
using AddinManager;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinHost {
    public static class PluginServiceCollectionExtension {
        public static IServiceCollection AddPluginClient<TContract>(this IServiceCollection services, string name, string assemblyPath) where TContract : class =>
            services.AddPluginClient<TContract>(name, (_, options) => {
                options.AssemblyPath = assemblyPath;
            });

        public static IServiceCollection AddPluginClient<TContract>(this IServiceCollection services, string name, Action<IServiceProvider, PluginClientOptions> configureOptions)
            where TContract : class {
            services.AddPluginClient(new PluginClientRegistration<TContract, PluginClientOptions>(name,
                (serviceProvider, options) => {
                    return new PluginClient
                }, configureOptions));
            return services;
        }
    }

    public class PluginClientOptions  {
        public string AssemblyPath { get; set; }
    }

}