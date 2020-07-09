using System;
using AddinManager;

namespace AddinHost {
    public class PluginClientRegistration<TContract, TPluginClientOptions> where TContract : class where TPluginClientOptions : PluginClientOptions {
        readonly Func<IServiceProvider, TPluginClientOptions, IAddinProcess> clientFactory;
        readonly Action<IServiceProvider, TPluginClientOptions> configureOptions;

        public string Name { get; }

        public PluginClientRegistration(string name, Func<IServiceProvider, TPluginClientOptions, IAddinProcess> clientFactory, Action<IServiceProvider, TPluginClientOptions> configureOptions) {
            Name = name;
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.configureOptions = configureOptions;
        }

        public IAddinProcess Create(IServiceProvider serviceProvider) {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            TPluginClientOptions instance = Activator.CreateInstance<TPluginClientOptions>();
            Action<IServiceProvider, TPluginClientOptions> configureOptions = this.configureOptions;
            configureOptions?.Invoke(serviceProvider, instance);
            return this.clientFactory(serviceProvider, instance);
        }
    }