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
                (serviceProvider, options) => {
                    var processFactory = serviceProvider.GetRequiredService<IAddinProcessFactory<TContract>>();
                    var process = processFactory.Create(options.ProcessName, options.Lifetime);
                    return (IIpcClient<TContract>)new NamedPipeAddinClient<TContract>(name, process, options);
                }, configureOptions));
            return services;
        }
    }
}