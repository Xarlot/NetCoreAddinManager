using AddinManager.Core;
using JKang.IpcServiceFramework.Hosting;

namespace AddinHost {
    public class EndpointRegistrator : IEndpointRegistrator {
        readonly IIpcHostBuilder builder;
        public EndpointRegistrator(IIpcHostBuilder builder) {
            this.builder = builder;
        }
        public void Register<TContract>(string endpoint) where TContract : class {
            this.builder.AddNamedPipeEndpoint<TContract>(endpoint);
        }
    }
}