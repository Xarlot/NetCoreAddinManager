using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using AddinManager.Core;
using CommandLine;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AddinHost {
    class Program {
        public static int Main(string[] args) {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => {
                var process = Process.GetProcessById(o.Pid);
                var thread = new Thread(ListenProcessExit) {IsBackground = true};
                thread.Start(process);
                var eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "AddinProcess:" + o.Guid);
                eventWaitHandle.Set();
                eventWaitHandle.Close();

                CreateHostBuilder(args, o.Guid.ToString(), o.AddinsLocation, o.SearchPattern).Build().Run();
            }).WithNotParsed(HandleErrors);
            return 0;
        }
        static void HandleErrors(IEnumerable<Error> obj) {
        }
        static IHostBuilder CreateHostBuilder(string[] args, string guid, string addinsLocation, string searchPattern) {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureServices(services => {
                           services.AddDependencies(addinsLocation, searchPattern);
                       })
                       .ConfigureIpcHost(builder => {
                           builder.AddEndpoints(addinsLocation, searchPattern);
                       })
                       .ConfigureLogging(builder => {
                           builder.SetMinimumLevel(LogLevel.Debug);
                       });
        }
        static void ListenProcessExit(object parameter) {
            var process = (Process)parameter;
            process.WaitForExit();
            Environment.Exit(0);
        }
    }
}