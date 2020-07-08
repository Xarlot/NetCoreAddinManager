namespace AddinManager.Core {
    public interface IAddinServerContract {
        bool ToTest();
        int Increment();
        public void RegisterDuplex<T>(string name, string pipeName) where T : class;
    }
}