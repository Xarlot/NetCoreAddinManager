using System.Reflection;
using System.Runtime.Versioning;
using AddinManager;

namespace AddinManagerClientCoreTests {
    public static class FrameworkVersionHelper {
        public static Runtime GetVersion() {
            var framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            if (framework?.ToLowerInvariant().Contains(".netcore") ?? false)
                return Runtime.NetCore3;
            return Runtime.Framework;
        }
    }
}