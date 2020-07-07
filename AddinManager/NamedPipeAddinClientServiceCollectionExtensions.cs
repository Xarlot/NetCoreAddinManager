using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddinManager {
    public static class NamedPipeAddinClientServiceCollectionExtensions {
        public static IServiceCollection AddAddinClient<TContract, TIpcClientOptions>(
            this IServiceCollection services,
            IpcClientRegistration<TContract, TIpcClientOptions> registration)
            where TContract : class
            where TIpcClientOptions : IpcClientOptions
        {
            services.TryAddScoped<IIpcClientFactory<TContract>, NamedPipeAddinClientFactory<TContract, TIpcClientOptions>>();
            services.AddSingleton(registration);
            return services;
        }
    }
}