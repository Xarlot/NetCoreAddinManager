using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class NamedPipeAddinClientOptions : IpcClientOptions {
        public Runtime Runtime { get; set; }
        public string ProcessName { get; set; }
    }
}