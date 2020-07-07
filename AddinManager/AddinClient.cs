using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JKang.IpcServiceFramework.Client;

namespace AddinManager {
    public class AddinClient<T> : IIpcClient<T>, IAddinToken where T: class {
        readonly IAddinProcess addinProcess;
        readonly IIpcClient<T> client;
        public AddinClient(IAddinProcess addinProcess, IIpcClientFactory<T> factory) {
            this.addinProcess = addinProcess;
            this.client = factory.CreateClient(addinProcess.Guid.ToString());
        }
        public Task InvokeAsync(Expression<Action<T>> exp, CancellationToken cancellationToken = new CancellationToken()) {
            return this.client.InvokeAsync(exp, cancellationToken);
        }
        public Task<TResult> InvokeAsync<TResult>(Expression<Func<T, TResult>> exp, CancellationToken cancellationToken = new CancellationToken()) {
            return this.client.InvokeAsync(exp, cancellationToken);
        }
        public Task InvokeAsync(Expression<Func<T, Task>> exp, CancellationToken cancellationToken = new CancellationToken()) {
            return this.client.InvokeAsync(exp, cancellationToken);
        }
        public Task<TResult> InvokeAsync<TResult>(Expression<Func<T, Task<TResult>>> exp, CancellationToken cancellationToken = new CancellationToken()) {
            return this.client.InvokeAsync(exp, cancellationToken);
        }
        public string Name => this.client.Name;
    }

    public interface IAddinToken {
        string Name { get; }
    }
}