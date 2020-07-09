using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public class NamedPipeAddinClientOptions : IpcClientOptions {
        public Runtime Runtime { get; set; }
        public string ProcessName { get; set; }
        public ServiceLifetime Lifetime { get; set; }
    }
}