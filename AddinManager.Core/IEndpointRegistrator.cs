namespace AddinManager.Core {
    public interface IEndpointRegistrator {
        public void Register<TContract>(string endpoint) where TContract: class;
    }
}