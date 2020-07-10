using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public class AddinProcessOptions {
        public Runtime Runtime { get; set; }
        public ServiceLifetime Lifetime { get; set; }
        public (string assemblyName, string searchPattern) Dependency { get; set; }
    }
}