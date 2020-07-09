using System.Diagnostics;
using AddinManager.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AddinHost {
    public class AddinServer : IAddinServerContract {
        readonly IServiceCollection services;
        public AddinServer(IServiceCollection services) {
            this.services = services;
        }
        int count;
        public bool ToTest() {
            return true;
        }
        public int Increment() {
            this.count += 1;
            return this.count;
        }
        public void RegisterDuplex<T>(string name, string pipeName, string assemblyPath = null) where T: class {
            Debugger.Launch();
            this.services.AddNamedPipeIpcClient<T>(name, pipeName);
            this.services.AddPluginClient<T>(name, assemblyPath);
        }
    }
}