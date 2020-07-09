using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using AddinManagerContractTests;
using AddinManagerCoreTests;
using JKang.IpcServiceFramework.Client;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace AddinManagerClientCoreTests {
    [TestFixture]
    public class DuplexConnectionTests {
        [Test]
        public async Task RegisterDuplexTest() {
            var host = Host.CreateDefaultBuilder().ConfigureServices(x => {
                x.AddAddinProcess<IAddinServerContract>("duplexHostProcess", (provider, options) => {
                     options.Runtime = Runtime.Framework;
                     options.Lifetime = ServiceLifetime.Singleton;
                 })
                 .AddAddinProcess<IDuplexClientContract>("duplexHostProcess", (provider, options) => {
                     options.Runtime = Runtime.Framework;
                     options.Lifetime = ServiceLifetime.Singleton;
                 })
                 .AddNamedPipeAddinClient<IAddinServerContract>("addinServer", "duplexHostProcess").AddNamedPipeAddinClient<IDuplexClientContract>("duplexClient", "duplexHostProcess")
                 .AddSingleton<IDuplexHostContract, DuplexHost>();
            }).ConfigureIpcHost(x => x.AddNamedPipeEndpoint<IAddinServerContract>("duplexHostPipeName"))
            .Build();
            
            await host.StartAsync();
            var serviceProvider = host.Services;
            
            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("addinServer");
            await client.InvokeAsync(x => x.RegisterDuplex<IDuplexClientContract>("duplexClient", "duplexHostPipeName"));

            var duplexClientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexClientContract>>();
            var duplexClient = duplexClientFactory.CreateClient("duplexClient");
            var result = await duplexClient.InvokeAsync(x => x.InvokeClient());
        }
    }
}