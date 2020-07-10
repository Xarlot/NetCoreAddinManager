using Microsoft.Extensions.DependencyInjection;

namespace AddinManager.Core {
    public class DependencyRegistrator : IDependencyRegistrator {
        readonly IServiceCollection serviceCollection;

        public DependencyRegistrator(IServiceCollection serviceCollection) {
            this.serviceCollection = serviceCollection;
        }

        void IDependencyRegistrator.AddScoped<TService>() {
            serviceCollection.AddScoped<TService>();
        }

        void IDependencyRegistrator.AddScoped<TService, TImplementation>() {
            serviceCollection.AddScoped<TService, TImplementation>();
        }

        void IDependencyRegistrator.AddScopedForMultiImplementation<TService, TImplementation>() {
            serviceCollection.AddScoped<TImplementation>().AddScoped<TService, TImplementation>(s => s.GetService<TImplementation>());
        }

        void IDependencyRegistrator.AddSingleton<TService>() {
            serviceCollection.AddSingleton<TService>();
        }

        void IDependencyRegistrator.AddSingleton<TService, TImplementation>() {
            serviceCollection.AddSingleton<TService, TImplementation>();
        }

        void IDependencyRegistrator.AddTransient<TService>() {
            serviceCollection.AddTransient<TService>();
        }

        void IDependencyRegistrator.AddTransient<TService, TImplementation>() {
            serviceCollection.AddTransient<TService, TImplementation>();
        }

        void IDependencyRegistrator.AddTransientForMultiImplementation<TService, TImplementation>() {
            serviceCollection.AddTransient<TImplementation>().AddTransient<TService, TImplementation>(s => s.GetService<TImplementation>());
        }
    }

    public interface IDependencyRegistrator {
        void AddScoped<TService>() where TService : class;

        void AddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        void AddSingleton<TService>() where TService : class;

        void AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        void AddTransient<TService>() where TService : class;

        void AddTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        void AddTransientForMultiImplementation<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        void AddScopedForMultiImplementation<TService, TImplementation>() where TService : class where TImplementation : class, TService;
    }
}