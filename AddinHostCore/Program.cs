using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AddinManagerContractTests;
using AddinManagerCoreTests;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AddinHostCore {
    class Program {
        public static int Main(string[] args) {
            if (args == null || args.Length != 2 || (!args[0].StartsWith("/guid:", StringComparison.Ordinal) || !args[1].StartsWith("/pid:", StringComparison.Ordinal)))
                return 1;
            string guid = args[0].Remove(0, 6);
            if (guid.Length != 36)
                return 1;
            int pid = Convert.ToInt32(args[1].Remove(0, 5), CultureInfo.InvariantCulture);
            try {
                Process process = Process.GetProcessById(pid);
                process.Exited += (sender, eventArgs) => { Environment.Exit(0); };
                EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "AddinProcess:" + guid);
                eventWaitHandle.Set();
                eventWaitHandle.Close();
            }
            catch {
                return 1;
            }
            CreateHostBuilder(args).Build().Run();
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => { services.AddScoped<ITestContract1, Test1>(); })
                .ConfigureIpcHost(builder => { builder.AddNamedPipeEndpoint<ITestContract1>("pipeinternal"); })
                .ConfigureLogging(builder => { builder.SetMinimumLevel(LogLevel.Debug); });
    }
}