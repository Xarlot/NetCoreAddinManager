using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class NamedPipeAddinClient<T> : IpcClient<T>, IDisposable where T : class {
        readonly IAddinProcess<T> addinProcess;
        readonly NamedPipeAddinClientOptions options;
        readonly string pipeName;

        public object ProcessId => this.addinProcess.Id;
        
        public NamedPipeAddinClient(string name, IAddinProcess<T> addinProcess, NamedPipeAddinClientOptions options) : base(name, options) {
            this.options = options;
            this.addinProcess = addinProcess;
            this.addinProcess.Start();
            this.pipeName = this.addinProcess.Guid.ToString();
        }
        
        protected override async Task<Stream> ConnectToServerAsync(CancellationToken cancellationToken) {
            NamedPipeClientStream stream = new NamedPipeClientStream(".", this.pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            await stream.ConnectAsync(this.options.ConnectionTimeout, cancellationToken).ConfigureAwait(false);
            return stream;
        }
        public void Dispose() {
            this.addinProcess?.Dispose();
        }
    }
}