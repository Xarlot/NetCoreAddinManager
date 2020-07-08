using System;

namespace AddinManager {
    public class AddinProcessRegistration<TAddinProcessOptions> where TAddinProcessOptions : AddinProcessOptions {
        readonly Func<IServiceProvider, TAddinProcessOptions, IAddinProcess> clientFactory;
        readonly Action<IServiceProvider, TAddinProcessOptions> configureOptions;

        public AddinProcessRegistration(string name, Func<IServiceProvider, TAddinProcessOptions, IAddinProcess> clientFactory, Action<IServiceProvider, AddinProcessOptions> configureOptions) {
            Name = name;
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.configureOptions = configureOptions;
        }

        public string Name { get; }

        public IAddinProcess Create(IServiceProvider serviceProvider) {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            TAddinProcessOptions instance = Activator.CreateInstance<TAddinProcessOptions>();
            Action<IServiceProvider, TAddinProcessOptions> configureOptions = this.configureOptions;
            configureOptions?.Invoke(serviceProvider, instance);
            return this.clientFactory(serviceProvider, instance);
        }
    }
}