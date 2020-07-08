using Microsoft.Extensions.DependencyInjection;

namespace AddinManager {
    public class AddinProcessOptions {
        public Runtime Runtime { get; set; }
        public string ProcessName { get; set; }
    }
}