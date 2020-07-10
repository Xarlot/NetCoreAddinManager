using System.ComponentModel.Composition;
using System.Diagnostics;
using AddinManager.Core;
using AddinManagerContractTests;

namespace AddinManagerClientCoreTests {
    [Export(typeof(IDependencyResolver))]
    public class DuplexClientDependencyResolver : IDependencyResolver {
        public void Initialize(IDependencyRegistrator dependencyRegister) {
            Debugger.Launch();
            dependencyRegister.AddScoped<IDuplexClientContract, DuplexClient>();
        }
    }
}