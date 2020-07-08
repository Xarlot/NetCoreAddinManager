using System;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public static class NamedPipeAddinClientExtensions {
        public static IServiceCollection AddNamedPipeAddinClient<TContract>(this IServiceCollection services, string name, string processName) where TContract : class =>
            services.AddNamedPipeAddinClient<TContract>(name, (_, options) => {
                options.ProcessName = processName;
            });

        public static IServiceCollection AddNamedPipeAddinClient<TContract>(this IServiceCollection services, string name, Action<IServiceProvider, NamedPipeAddinClientOptions> configureOptions)
            where TContract : class {
            services.AddAddinClient(new IpcClientRegistration<TContract, NamedPipeAddinClientOptions>(name,
                (_, options) => (IIpcClient<TContract>)new NamedPipeAddinClient<TContract>(name, options), configureOptions));
            return services;
        }
    }
}