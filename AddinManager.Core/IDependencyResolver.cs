using Microsoft.Extensions.DependencyInjection;

namespace AddinManager.Core {
    public interface IDependencyResolver {
        void Initialize(IServiceCollection registrator);
    }
}