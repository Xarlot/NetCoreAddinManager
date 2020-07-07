using System;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public static class NamedPipeAddinClientExtensions {
        public static IServiceCollection AddNamedPipeIpcClient<TContract>(this IServiceCollection services, string name, string pipeName) where TContract : class =>
            services.AddNamedPipeIpcClient<TContract>(name, (Action<IServiceProvider, NamedPipeIpcClientOptions>)((_, options) => options.PipeName = pipeName));

        public static IServiceCollection AddNamedPipeIpcClient<TContract>(this IServiceCollection services, string name, Action<IServiceProvider, NamedPipeIpcClientOptions> configureOptions)
            where TContract : class {
            services.AddIpcClient<TContract, NamedPipeIpcClientOptions>(new IpcClientRegistration<TContract, NamedPipeIpcClientOptions>(name,
                (Func<IServiceProvider, NamedPipeIpcClientOptions, IIpcClient<TContract>>)((_, options) => (IIpcClient<TContract>)new NamedPipeIpcClient<TContract>(name, options)), configureOptions));
            return services;
        }
    }
}