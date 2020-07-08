using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    class AddinProcessFactory<TContract, TAddinProcessOptions> : IAddinProcessFactory<TContract>  
        where TContract: class 
        where TAddinProcessOptions : AddinProcessOptions {
        readonly IServiceProvider serviceProvider;
        readonly IEnumerable<AddinProcessRegistration<TContract, AddinProcessOptions>> registrations;

        public AddinProcessFactory(IServiceProvider serviceProvider, IEnumerable<AddinProcessRegistration<TContract, AddinProcessOptions>> registrations) {
            this.serviceProvider = serviceProvider;
            this.registrations = registrations;
        }

        public IAddinProcess<TContract> Create(string name) {
            var clientRegistration = this.registrations.FirstOrDefault(x => x.Name == name);
            if (clientRegistration == null)
                throw new ArgumentException("IPC client '" + name + "' is not configured.", nameof(name));
            using IServiceScope scope = this.serviceProvider.CreateScope();
            return clientRegistration.Create(scope.ServiceProvider);
        }
    }

    public interface IAddinProcessFactory<TContract> 
        where TContract: class {
        IAddinProcess<TContract> Create(string name);
    }
}