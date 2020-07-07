using System;
using System.Collections.Generic;
using System.Linq;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    class NamedPipeAddinClientFactory<TContract, TIpcClientOptions> : IIpcClientFactory<TContract> where TContract : class where TIpcClientOptions : IpcClientOptions {
        readonly IServiceProvider serviceProvider;
        readonly IEnumerable<IpcClientRegistration<TContract, TIpcClientOptions>> registrations;

        public NamedPipeAddinClientFactory(IServiceProvider serviceProvider, IEnumerable<IpcClientRegistration<TContract, TIpcClientOptions>> registrations) {
            this.serviceProvider = serviceProvider;
            this.registrations = registrations;
        }

        public IIpcClient<TContract> CreateClient(string name) {
            var clientRegistration = this.registrations.FirstOrDefault(x => x.Name == name);
            if (clientRegistration == null)
                throw new ArgumentException("IPC client '" + name + "' is not configured.", nameof(name));
            using IServiceScope scope = this.serviceProvider.CreateScope();
            return clientRegistration.CreateClient(scope.ServiceProvider);
        }
    }
}