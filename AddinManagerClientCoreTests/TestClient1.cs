using System.IO.Pipes;
using System.Threading.Tasks;
using AddinManagerContractTests;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AddinManagerClientCoreTests {
    [TestFixture]
    public class TestClient1 {
        [Test]
        public async Task Test() {
            ServiceProvider serviceProvider = new ServiceCollection().AddNamedPipeIpcClient<ITestContract1>("client1", pipeName: "pipeinternal").BuildServiceProvider();
            IIpcClientFactory<ITestContract1> clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<ITestContract1>>();
            IIpcClient<ITestContract1> client = clientFactory.CreateClient("client1");
            bool output = await client.InvokeAsync(x => x.DoTest());
        }
    }
}