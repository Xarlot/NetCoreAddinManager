using System;
using System.Collections.Concurrent;

namespace AddinManager {
    public class AddinProcessPool : IAddinProcessPool {
        readonly ConcurrentDictionary<string, IAddinProcess> processPool = new ConcurrentDictionary<string, IAddinProcess>();
        public AddinProcessPool() {
        }
        public IAddinProcess GetOrAdd(string name, Func<string, IAddinProcess> addinProcessFactory) {
            return this.processPool.GetOrAdd(name, addinProcessFactory);
        }
    }

    public interface IAddinProcessPool {
        IAddinProcess GetOrAdd(string name, Func<string, IAddinProcess> addinProcessFactory);
    }
}