using System.ComponentModel.Composition;
using AddinManager.Core;
using AddinManagerContractTests;

namespace AddinManagerClientCoreTests {
    [Export(typeof(IDependencyResolver))]
    public class DuplexClientEndpointResolver : IEndpointResolver {
        public void Initialize(IEndpointRegistrator registrator) {
            registrator.Register<IDuplexClientContract>("duplexClientPipeName");
        }
    }
}