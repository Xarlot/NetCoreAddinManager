using System.Diagnostics;
using AddinManager.Core;
using JKang.IpcServiceFramework.Hosting;
using JKang.IpcServiceFramework.Hosting.NamedPipe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        public void RegisterDuplex<TClientContract, THostContract>(string name, string clientPipeName, string hostPipeName) 
            where TClientContract: class 
            where THostContract: class {
            Debugger.Launch();
            var serviceProvider = services.BuildServiceProvider();
            var options = new NamedPipeIpcEndpointOptions {PipeName = clientPipeName};
            var endPoint = new NamedPipeIpcEndpoint<TClientContract>(options, serviceProvider.GetRequiredService<ILogger<NamedPipeIpcEndpoint<TClientContract>>>(), serviceProvider);
            this.services.AddSingleton<IpcEndpoint<TClientContract>>(endPoint);
            
            this.services.AddNamedPipeIpcClient<THostContract>(name, hostPipeName);
        }
        public void LoadDependencies(string path, string pattern) {
            this.services.LoadDependencies(path, pattern);
        }
    }
}