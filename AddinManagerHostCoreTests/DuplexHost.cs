using System.Threading.Tasks;
using AddinManagerContractTests;

namespace AddinManagerCoreTests {
    public class DuplexHost : IDuplexHostContract {
        public Task<bool> InvokeHost() {
            return Task.FromResult(true);
        }
    }
}