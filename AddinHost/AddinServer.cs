using AddinManager.Core;

namespace AddinHost {
    public class AddinServer : IAddinServerContract {
        int count = 0;
        public bool ToTest() {
            return true;
        }
        public int Increment() {
            this.count += 1;
            return this.count;
        }
    }
}