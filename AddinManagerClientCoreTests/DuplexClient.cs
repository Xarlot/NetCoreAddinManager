using System.Diagnostics;
using System.Threading.Tasks;
using AddinManagerContractTests;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManagerClientCoreTests {
    public class DuplexClient : IDuplexClientContract {
        readonly ServiceProvider serviceProvider;
        public DuplexClient(IServiceCollection collection) {
            this.serviceProvider = collection.BuildServiceProvider();
        }

        public async Task<bool> InvokeClient() {
            Debugger.Launch();
            var clientFactory = this.serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexHostContract>>();
            var client = clientFactory.CreateClient("duplex");
            return await client.InvokeAsync(x => x.InvokeHost());
        }
    }
}