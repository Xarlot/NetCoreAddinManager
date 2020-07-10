namespace AddinManager.Core {
    public interface IEndpointResolver {
        void Initialize(IEndpointRegistrator registrator);
    }
}