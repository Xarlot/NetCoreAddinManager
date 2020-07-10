namespace AddinManager.Core {
    public interface IDependencyResolver {
        void Initialize(IDependencyRegistrator registrator);
    }
}