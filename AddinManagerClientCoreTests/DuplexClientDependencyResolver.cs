using System.ComponentModel.Composition;
using System.Diagnostics;
using AddinManager.Core;
using AddinManagerContractTests;

namespace AddinManagerClientCoreTests {
    [Export(typeof(IDependencyResolver))]
    [Export(typeof(IEndpointResolver))]
    public class DuplexClientDependencyResolver : IDependencyResolver, IEndpointResolver {
        public void Initialize(IDependencyRegistrator dependencyRegister) {
            Debugger.Launch();
            dependencyRegister.AddScoped<IDuplexClientContract, DuplexClient>();
        }
        public void Initialize(IEndpointRegistrator registrator) {
            Debugger.Launch();
            registrator.Register<IDuplexClientContract>("duplexClientPipeName");
        }
    }
}