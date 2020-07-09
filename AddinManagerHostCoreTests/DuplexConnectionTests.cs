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
            var serviceCollection = new ServiceCollection()
                .AddAddinProcess<IAddinServerContract>("duplexHostProcess", (provider, options) => {
                    options.Runtime = Runtime.Framework;
                    options.Lifetime = ServiceLifetime.Singleton;
                })
                .AddAddinProcess<IDuplexClientContract>("duplexHostProcess", (provider, options) => {
                    options.Runtime = Runtime.Framework;
                    options.Lifetime = ServiceLifetime.Singleton;
                })
                .AddNamedPipeAddinClient<IAddinServerContract>("addinServer", "duplexHostProcess")
                .AddNamedPipeAddinClient<IDuplexClientContract>("duplexClient", "duplexHostProcess")
                .AddNamedPipeEndpoint<IDuplexHostContract>("duplexHost", "duplexHostPipeName")
                .AddSingleton<IDuplexHostContract, DuplexHost>();
            await using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("addinServer");
            await client.InvokeAsync(x => x.RegisterDuplex<IDuplexHostContract>("duplexClient", "duplexHostPipeName"));

            var duplexClientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexClientContract>>();
            var duplexClient = duplexClientFactory.CreateClient("duplexClient");
            var result = await duplexClient.InvokeAsync(x => x.InvokeClient());
        }
    }
}