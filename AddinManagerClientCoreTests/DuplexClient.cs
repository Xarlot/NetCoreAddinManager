using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AddinManagerContractTests;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManagerClientCoreTests {
    public class DuplexClient : IDuplexClientContract {
        readonly IServiceProvider serviceProvider;
        public DuplexClient(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public async Task<bool> InvokeClient() {
            var clientFactory = this.serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexHostContract>>();
            var client = clientFactory.CreateClient("duplexHost");
            return await client.InvokeAsync(x => x.InvokeHost());
        }
    }
}