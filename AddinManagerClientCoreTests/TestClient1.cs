using System.Threading.Tasks;
using AddinManager;
using AddinManagerContractTests;
using FluentAssertions;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AddinManagerClientCoreTests {
    [TestFixture]
    public class TestClient1 {
        [Test]
        public async Task Test() {
            ServiceProvider serviceProvider = new ServiceCollection().AddNamedPipeAddinClient<ITestContract1>("client1", runtime: FrameworkVersionHelper.GetVersion()).BuildServiceProvider();
            
            IIpcClientFactory<ITestContract1> clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<ITestContract1>>();
            IIpcClient<ITestContract1> client = clientFactory.CreateClient("client1");
            var output = await client.InvokeAsync(x => x.DoTest());
            output.Should().BeTrue();
        }
    }
}