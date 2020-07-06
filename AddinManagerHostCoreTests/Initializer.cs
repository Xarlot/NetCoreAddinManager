using System;
using AddinManagerContractTests;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManagerCoreTests {
    public class Initializer {
        public void Initialize() {
            
            // ServiceProvider serviceProvider = new ServiceCollection().AddNamedPipeIpcClient<ITestContract1>("client1", pipeName: "pipeinternal").BuildServiceProvider();
            // IIpcClientFactory<ITestContract1> clientFactory = serviceProvider.GetRequiredService<IIpcClientFactory<ITestContract1>>();
            // IIpcClient<ITestContract1> client = clientFactory.CreateClient("client1");
            // bool output = await client.InvokeAsync(x => x.DoTest());
        }
    }
}