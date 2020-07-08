using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using AddinManagerContractTests;
using AddinManagerCoreTests;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AddinManagerClientCoreTests {
    [TestFixture]
    public class DuplexConnectionTests {
        [Test]
        public async Task RegisterDuplexTest() {
            string pipeName = "duplexHost";
            var serviceCollection = new ServiceCollection().AddNamedPipeAddinClient<IAddinServerContract>("addinServer", "addinServer")
                                                           .AddNamedPipeAddinClient<IDuplexClientContract>("duplexClient", "addinserver")
                                                           .AddNamedPipeIpcClient<IDuplexHostContract>("duplexHost", pipeName).AddSingleton<IDuplexHostContract, DuplexHost>();
            await using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("addinServer");
            await client.InvokeAsync(x => x.RegisterDuplex<IDuplexHostContract>("duplexClient", pipeName));

            var duplexClientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexClientContract>>();
            var duplexClient = duplexClientFactory.CreateClient("duplexClient");
            var result = await duplexClient.InvokeAsync(x => x.InvokeClient());
        }
    }
}