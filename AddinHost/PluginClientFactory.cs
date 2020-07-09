using System;
using System.Collections.Generic;
using System.Linq;
using AddinManager;
using Microsoft.Extensions.DependencyInjection;

namespace AddinHost {
    class PluginClientFactory<TContract, TPluginClientOptions> : IPluginClientFactory<TContract> where TContract : class where TPluginClientOptions : PluginClientOptions {
        readonly IServiceProvider serviceProvider;
        readonly IEnumerable<PluginClientRegistration<TContract, TPluginClientOptions>> registrations;

        public PluginClientFactory(IServiceProvider serviceProvider, IEnumerable<PluginClientRegistration<TContract, TPluginClientOptions>> registrations) {
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

    public interface IPluginClientFactory<TContract> where TContract : class {
        IAddinProcess Create(string name, ServiceLifetime lifetime);
    }
}