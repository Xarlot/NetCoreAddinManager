using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class NamedPipeAddinClientOptions : IpcClientOptions {
        public Runtime Runtime { get; set; }
    }
}