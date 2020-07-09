using System;
using JKang.IpcServiceFramework.Hosting;
using JKang.IpcServiceFramework.Hosting.NamedPipe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AddinManager {
    public static class NamedPipeIpcServiceCollectionExtensions {
        public static IServiceCollection AddNamedPipeEndpoint<TContract>(
            this IServiceCollection builder,
            string pipeName)
            where TContract : class => builder.AddNamedPipeEndpoint<TContract>((Action<NamedPipeIpcEndpointOptions>) (options => options.PipeName = pipeName));

        public static IServiceCollection AddNamedPipeEndpoint<TContract>(
            this IServiceCollection services,
            Action<NamedPipeIpcEndpointOptions> configure)
            where TContract : class
        {
            NamedPipeIpcEndpointOptions options = new NamedPipeIpcEndpointOptions();
            configure?.Invoke(options);
            services.AddSingleton((IIpcEndpoint)new NamedPipeIpcEndpoint<TContract>(options, null);
            services.AddIpcEndpoint(serviceProvider => (IIpcEndpoint) new NamedPipeIpcEndpoint<TContract>(options, ServiceProviderServiceExtensions.GetRequiredService<ILogger<NamedPipeIpcEndpoint<TContract>>>(serviceProvider), serviceProvider));
            return services;
        }
    }
}