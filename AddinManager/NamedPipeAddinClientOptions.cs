using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class NamedPipeAddinClientOptions : IpcClientOptions {
        public string PipeName { get; set; }
        public Runtime Runtime { get; set; }
    }
}