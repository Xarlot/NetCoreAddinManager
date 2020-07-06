using System;
using System.Diagnostics;
using System.IO;
using AddinManager.Core;

namespace AddinManager {
    public class AddinProcess : IDisposable {
        const string HostCorePath = "AddinHostCore";
        const string HostFWPath = "AddinHostFW";
        readonly Process process;
        public AddinProcess(ProcessType processType, AddinHostBootstrapperOptions options) {
            this.process = CreateProcess(processType, options);
        }
        Process CreateProcess(ProcessType processType, AddinHostBootstrapperOptions options) {
            return processType == ProcessType.Framework ? CreateFWProcess(options) : CreateNetCore3Process(options);
        }
        Process CreateNetCore3Process(AddinHostBootstrapperOptions options) {
            return new Process() {StartInfo = new ProcessStartInfo(Path.Combine(HostCorePath, "AddinHostCore.exe"))};
        }
        Process CreateFWProcess(AddinHostBootstrapperOptions options) {
            return new Process() {StartInfo = new ProcessStartInfo(HostFWPath, "AddinHostFW.exe")};
        }

        public bool Start() {
            return this.process.Start();
        }
        public void Kill() {
            this.process.Kill();
        }
        public void Dispose() {
            this.process?.Dispose();
        }
    }
}