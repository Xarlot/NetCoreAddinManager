using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using FluentAssertions;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AddinManagerTests {
    [TestFixture]
    public class AddinProcessFactoryTests {
        [Test]
        public async Task CreateProcessPerRequestTest() {
            await using ServiceProvider serviceProvider = new ServiceCollection()
                .AddAddinProcess<IAddinServerContract>("perrequestprocess", (provider, options) => { options.Runtime = Runtime.Framework; })
                .AddNamedPipeAddinClient<IAddinServerContract>("perrequest", "perrequestprocess")
                .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = (NamedPipeAddinClient<IAddinServerContract>)clientFactory.CreateClient("perrequest");
            var output = await client.InvokeAsync(x => x.ToTest());
            output.Should().BeTrue();
            var client2 = (NamedPipeAddinClient<IAddinServerContract>)clientFactory.CreateClient("perrequest");
            var output2 = await client2.InvokeAsync(x => x.ToTest());
            output2.Should().BeTrue();
            client.ProcessId.Should().NotBeSameAs(client2.ProcessId);
        }
        [Test]
        public async Task CreateSharedProcessTest() {
            await using ServiceProvider serviceProvider = new ServiceCollection()
                .AddAddinProcess<IAddinServerContract>("perrequestprocess", (provider, options) => {
                    options.Runtime = Runtime.Framework;
                    options.Lifetime = ServiceLifetime.Singleton;
                })
                .AddNamedPipeAddinClient<IAddinServerContract>("perrequest", "perrequestprocess")
                .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = (NamedPipeAddinClient<IAddinServerContract>)clientFactory.CreateClient("perrequest");
            var output = await client.InvokeAsync(x => x.ToTest());
            output.Should().BeTrue();
            var client2 = (NamedPipeAddinClient<IAddinServerContract>)clientFactory.CreateClient("perrequest");
            var output2 = await client2.InvokeAsync(x => x.ToTest());
            output2.Should().BeTrue();
            client.ProcessId.Should().Be(client2.ProcessId);
        }
    }
}