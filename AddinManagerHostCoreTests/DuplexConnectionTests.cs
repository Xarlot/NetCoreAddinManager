using System.IO;
using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using AddinManagerContractTests;
using AddinManagerCoreTests;
using FluentAssertions;
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
            string duplexHostProcess = nameof(duplexHostProcess);
            var dependencies = (Path.GetDirectoryName(typeof(DuplexConnectionTests).Assembly.Location), "AddinManagerClientCoreTests.dll");
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(x => {
                     x.AddAddinProcess<IDuplexClientContract>(duplexHostProcess, (provider, options) => {
                         options.Runtime = Runtime.Framework;
                         options.Lifetime = ServiceLifetime.Singleton;
                         options.Dependency = dependencies;
                     })
                     .AddNamedPipeAddinClient<IDuplexClientContract>("duplexClient", (_, options) => {
                         options.ProcessName = duplexHostProcess;
                         options.PipeName = "duplexClientPipeName";
                     })
                     .AddSingleton<IDuplexHostContract, DuplexHost>();
                })
                .ConfigureIpcHost(x => x.AddNamedPipeEndpoint<IDuplexHostContract>("duplexHostPipeName"))
                .Build();
            
            await host.StartAsync();
            var serviceProvider = host.Services;
            
            var duplexClientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IDuplexClientContract>>();
            var duplexClient = duplexClientFactory.CreateClient("duplexClient");
            var result = await duplexClient.InvokeAsync(x => x.InvokeClient());
            result.Should().BeTrue();
        }
    }
}