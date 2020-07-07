using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using AddinManager.Core;

namespace AddinManager {
    public class AddinProcess : IDisposable {
        readonly AddinHostBootstrapperOptions options;
        readonly object processLocker = new object();
        const string HostCorePath = "AddinHostCore";
        const string HostFwPath = "AddinHostFW";
        readonly string pathToAddinProcess;
        Guid guid;
        Process process;

        TimeSpan startupTimeout = new TimeSpan(0, 0, 100);

        public TimeSpan StartupTimeout {
            get => this.startupTimeout;
            set {
                if (value.TotalSeconds < 0) throw new ArgumentOutOfRangeException("startup timeout");

                lock (this.processLocker) {
                    if (this.process == null)
                        this.startupTimeout = value;
                    else
                        throw new InvalidOperationException("process already running");
                }
            }
        }

        public AddinProcess(Runtime runtime, AddinHostBootstrapperOptions options) {
            this.options = options;
            String folder = Path.GetDirectoryName(typeof(AddinProcess).Assembly.Location);
            String exeName = GetProcessName(runtime);
            this.pathToAddinProcess = Path.Combine(folder, exeName);
            if (!File.Exists(this.pathToAddinProcess))
                throw new InvalidOperationException(@$"Addin executable not found: {pathToAddinProcess}");
        }
        string GetProcessName(Runtime runtime) {
            return runtime == Runtime.NetCore3 ? Path.Combine(HostCorePath, $"{HostCorePath}.exe") : Path.Combine(HostFwPath, $"{HostFwPath}.exe");
        }

        public void Start() {
            if (this.process == null) {
                lock (this.processLocker) {
                    this.process ??= CreateProcess();
                }
            }
        }
        Process CreateProcess() {
            Process addinProcess = new Process();
            Guid guid = Guid.NewGuid();
            string args = string.Format(CultureInfo.InvariantCulture, "/guid:{0} /pid:{1}", guid, Process.GetCurrentProcess().Id);

            addinProcess.StartInfo.CreateNoWindow = true;
            addinProcess.StartInfo.UseShellExecute = false;
            addinProcess.StartInfo.Arguments = args;
            addinProcess.StartInfo.FileName = this.pathToAddinProcess;

            EventWaitHandle readyEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "AddinProcess:" + guid);
            addinProcess.Start();

            bool success = readyEvent.WaitOne(this.startupTimeout, false);
            readyEvent.Close();

            if (!success) {
                try {
                    addinProcess.Kill();
                }
                catch (Exception) {
                }

                throw new InvalidOperationException($"Could not create addin process with startup timeout {startupTimeout}");
            }

            this.guid = guid;

            return addinProcess;
        }
        public void Kill() {
            this.process.Kill();
        }
        public void Dispose() {
            this.process?.Dispose();
        }
    }
}