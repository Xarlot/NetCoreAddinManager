namespace AddinManager.Core {
    public interface IAddinServerContract {
        bool ToTest();
        int Increment();
        public void RegisterDuplex<T>(string name, string pipeName, string assemblyPath = null) where T : class;
    }
}