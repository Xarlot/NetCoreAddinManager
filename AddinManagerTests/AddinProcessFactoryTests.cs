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
                .AddAddinProcess<IAddinServerContract>("perrequestprocess", (provider, options) => { options.Runtime = Runtime.Framework; }, ServiceLifetime.Transient)
                .AddNamedPipeAddinClient<IAddinServerContract>("perrequest", "perrequestprocess")
                .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("perrequest");
            var output = await client.InvokeAsync(x => x.ToTest());
            output.Should().BeTrue();
        }
    }
}