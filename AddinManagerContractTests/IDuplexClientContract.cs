using System.Threading.Tasks;

namespace AddinManagerContractTests {
    public interface IDuplexClientContract {
        Task<bool> InvokeClient();
    }

    public interface IDuplexHostContract {
        Task<bool> InvokeHost();
    }
}