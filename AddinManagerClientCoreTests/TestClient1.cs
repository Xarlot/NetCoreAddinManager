using System.Threading.Tasks;
using AddinManager;
using AddinManager.Core;
using FluentAssertions;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AddinManagerClientCoreTests {
    [TestFixture]
    public class TestClient1 {
        [Test]
        public async Task Test() {
            ServiceProvider serviceProvider = new ServiceCollection().AddNamedPipeAddinClient<IAddinServerContract>("client1", runtime: FrameworkVersionHelper.GetVersion()).BuildServiceProvider();
            
            var clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<IAddinServerContract>>();
            var client = clientFactory.CreateClient("client1");
            var output = await client.InvokeAsync(x => x.ToTest());
            output.Should().BeTrue();
        }
    }
}