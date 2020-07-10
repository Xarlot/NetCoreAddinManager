using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager.Core {
    public static class DependencyLoader {
        public static void LoadDependencies(this IServiceCollection serviceCollection, string path, string pattern) {
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

        static ImportDefinition BuildImportDefinition() {
            return new ImportDefinition(def => true, typeof(IDependencyResolver).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
}

