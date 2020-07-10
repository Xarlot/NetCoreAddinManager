using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using AddinHost;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager.Core {
    public static class DependencyLoader {
        public static void AddDependencies(this IServiceCollection serviceCollection, string path, string pattern) {
            Debugger.Launch();
            var dirCat = new DirectoryCatalog(path, pattern);
            var importDef = BuildImportDefinition();
            try {
                using var aggregateCatalog = new AggregateCatalog();
                aggregateCatalog.Catalogs.Add(dirCat);

                using var compositionContainer = new CompositionContainer(aggregateCatalog);
                var exports = compositionContainer.GetExports(importDef);
                var modules = exports.Select(export => export.Value as IDependencyResolver).Where(m => m != null);
                var registerComponent = new DependencyRegistrator(serviceCollection);
                foreach (IDependencyResolver module in modules) 
                    module.Initialize(registerComponent);
            }
            catch (ReflectionTypeLoadException typeLoadException) {
                var builder = new StringBuilder();
                foreach (Exception loaderException in typeLoadException.LoaderExceptions) 
                    builder.AppendFormat("{0}\n", loaderException.Message);
                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }
        public static void AddEndpoints(this IIpcHostBuilder builder, string path, string pattern) {
            Debugger.Launch();
            var dirCat = new DirectoryCatalog(path, pattern);
            var importDef = BuildImportDefinition();
            try {
                using var aggregateCatalog = new AggregateCatalog();
                aggregateCatalog.Catalogs.Add(dirCat);

                using var compositionContainer = new CompositionContainer(aggregateCatalog);
                var exports = compositionContainer.GetExports(importDef);
                var modules = exports.Select(export => export.Value as IEndpointResolver).Where(m => m != null);
                var registerComponent = new EndpointRegistrator(builder);
                foreach (IEndpointResolver module in modules) 
                    module.Initialize(registerComponent);
            }
            catch (ReflectionTypeLoadException typeLoadException) {
                var stringBuilder = new StringBuilder();
                foreach (Exception loaderException in typeLoadException.LoaderExceptions) 
                    stringBuilder.AppendFormat("{0}\n", loaderException.Message);
                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        static ImportDefinition BuildImportDefinition() {
            return new ImportDefinition(def => true, typeof(IDependencyResolver).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
}

