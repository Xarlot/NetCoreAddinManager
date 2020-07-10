using System.ComponentModel.Composition;
using AddinManager.Core;
using AddinManagerContractTests;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManagerClientCoreTests {
    [Export(typeof(IDependencyResolver))]
    public class DuplexHostDependencyResolver : IDependencyResolver {
        public void Initialize(IServiceCollection registrator) {
            registrator.AddNamedPipeIpcClient<IDuplexHostContract>("duplexHost", "duplexHostPipeName");
        }
    }
}