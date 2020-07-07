using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class NamedPipeAddinClient<T> : IpcClient<T> where T : class {
        readonly IAddinProcess addinProcess;
        readonly NamedPipeAddinClientOptions options;
        
        public NamedPipeAddinClient(string name, NamedPipeAddinClientOptions options) : base(name, options) {
            this.options = options;
            this.addinProcess = new AddinProcess(Runtime.NetCore3);
            this.addinProcess.Start();
        }
        protected override async Task<Stream> ConnectToServerAsync(CancellationToken cancellationToken) {
            NamedPipeClientStream stream = new NamedPipeClientStream(".", this.options.PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            await stream.ConnectAsync(this.options.ConnectionTimeout, cancellationToken).ConfigureAwait(false);
            return stream;
        }
    }
}