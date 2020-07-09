using System;
using System.Collections.Generic;
using System.Linq;
using JKang.IpcServiceFramework.Hosting.NamedPipe;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    class NamedPipeIpcEndpointFactory<TContract, TNamedPipeIpcEndpointOptions> : INamedPipeIpcEndpointFactory<TContract>  
        where TContract: class 
        where TNamedPipeIpcEndpointOptions : NamedPipeIpcEndpointOptions {
        readonly IServiceProvider serviceProvider;
        readonly IEnumerable<AddinProcessRegistration<TContract, TNamedPipeIpcEndpointOptions>> registrations;

        public NamedPipeIpcEndpointFactory(IServiceProvider serviceProvider, IEnumerable<AddinProcessRegistration<TContract, TNamedPipeIpcEndpointOptions>> registrations) {
            this.serviceProvider = serviceProvider;
            this.registrations = registrations;
        }

        public IAddinProcess Create(string name, ServiceLifetime lifetime) {
            var clientRegistration = this.registrations.FirstOrDefault(x => x.Name == name);
            if (clientRegistration == null)
                throw new ArgumentException("IPC client '" + name + "' is not configured.", nameof(name));
            using IServiceScope scope = this.serviceProvider.CreateScope();
            return clientRegistration.Create(scope.ServiceProvider);
        }
    }

    public interface INamedPipeIpcEndpointFactory<TContract> 
        where TContract: class {
        IAddinProcess Create(string name, ServiceLifetime lifetime);
    }
}