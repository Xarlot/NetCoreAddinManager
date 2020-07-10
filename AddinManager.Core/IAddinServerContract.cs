namespace AddinManager.Core {
    public interface IAddinServerContract {
        bool ToTest();
        int Increment();
        public void RegisterDuplex<TClientContract, THostContract>(string name, string clientPipeName, string hostPipeName) 
            where TClientContract: class 
            where THostContract: class;
        public void LoadDependencies(string path, string pattern);
    }
}