using System;

namespace AddinManager {
    public class AddinProcessRegistration<TContract, TAddinProcessOptions> where TContract : class where TAddinProcessOptions : AddinProcessOptions {
        readonly Func<IServiceProvider, TAddinProcessOptions, IAddinProcess<TContract>> clientFactory;
        readonly Action<IServiceProvider, TAddinProcessOptions> configureOptions;

        public string Name { get; }

        public AddinProcessRegistration(string name, Func<IServiceProvider, TAddinProcessOptions, IAddinProcess<TContract>> clientFactory,
            Action<IServiceProvider, AddinProcessOptions> configureOptions) {
            Name = name;
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.configureOptions = configureOptions;
        }

        public IAddinProcess<TContract> Create(IServiceProvider serviceProvider) {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            TAddinProcessOptions instance = Activator.CreateInstance<TAddinProcessOptions>();
            Action<IServiceProvider, TAddinProcessOptions> configureOptions = this.configureOptions;
            configureOptions?.Invoke(serviceProvider, instance);
            return this.clientFactory(serviceProvider, instance);
        }
    }
}