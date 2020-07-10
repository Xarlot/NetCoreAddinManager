using System.IO;
using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using AddinManagerContractTests;
using JKang.IpcServiceFramework.Client;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace AddinManagerCoreTests {
    [TestFixture]
    public class LoadDependenciesTestFixture {
        [Test]
        public async Task LoadDependenciesTest() {
            var host = Host.CreateDefaultBuilder().ConfigureServices(x => {
               x.AddAddinProcess<IAddinServerContract>("addinServerProcess", (provider, options) => {
                    options.Runtime = Runtime.Framework;
                    options.Lifetime = ServiceLifetime.Singleton;
                })
                .AddNamedPipeAddinClient<IAddinServerContract>("addinServer", "addinServerProcess");
           })
           .Build();
            
            await host.StartAsync();
            var serviceProvider = host.Services;
            
            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("addinServer");
            await client.InvokeAsync(x => x.LoadDependencies(Path.GetDirectoryName(typeof(LoadDependenciesTestFixture).Assembly.Location), "AddinManagerClientCoreTests.dll"));
        }
    }
}