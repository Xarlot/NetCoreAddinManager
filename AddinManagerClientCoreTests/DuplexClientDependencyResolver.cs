using System.ComponentModel.Composition;
using AddinManager.Core;
using AddinManagerContractTests;
using Microsoft.Extensions.DependencyInjection;

namespace AddinManagerClientCoreTests {
    [Export(typeof(IDependencyResolver))]
    public class DuplexClientDependencyResolver : IDependencyResolver {
        public void Initialize(IServiceCollection  dependencyRegister) {
            dependencyRegister.AddScoped<IDuplexClientContract, DuplexClient>();
        }
    }
}