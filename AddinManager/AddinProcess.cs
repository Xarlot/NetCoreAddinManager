using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace AddinManager {
    public class AddinProcess : IAddinProcess {
        readonly string assemblyLocation;
        readonly string searchPattern;
        readonly object processLocker = new object();
        const string HostCoreFolder = "AddinHost";
        const string HostCoreExe = "AddinHostCore.exe";
        const string HostFwExe = "AddinHostFW.exe";
        readonly string pathToAddinProcess;
        Guid guid;
        Process process;

        TimeSpan startupTimeout = new TimeSpan(0, 0, 100);

        protected Process Process => this.process;
        public int ParentProcessId { get; }
        public int Id => this.process.Id;
        public Guid Guid => this.guid;
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

        protected internal AddinProcess(Runtime runtime, int parentProcessId, string assemblyLocation, string searchPattern) {
            this.assemblyLocation = assemblyLocation;
            this.searchPattern = searchPattern;
            ParentProcessId = parentProcessId;
            string folder = Path.GetDirectoryName(typeof(AddinProcess).Assembly.Location);
            string exeName = GetProcessName(runtime);
            this.pathToAddinProcess = Path.Combine(folder, exeName);
            if (!File.Exists(this.pathToAddinProcess))
                throw new InvalidOperationException(@$"Addin executable not found: {pathToAddinProcess}");
        }
        public AddinProcess(Runtime runtime, string assemblyLocation, string searchPattern) : this(runtime, Process.GetCurrentProcess().Id, assemblyLocation, searchPattern) {
        }

        string GetProcessName(Runtime runtime) {
            return runtime == Runtime.NetCore3 
                ? Path.Combine(HostCoreFolder, "netcoreapp3.1", HostCoreExe) : 
                Path.Combine(HostCoreFolder, "net48", HostFwExe);
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
            string args = string.Format(CultureInfo.InvariantCulture, "--guid {0} --pid {1} --location {2} --pattern {3}", guid, ParentProcessId, this.assemblyLocation, this.searchPattern);

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

    public interface IAddinProcess : IDisposable {
        int Id { get; }
        TimeSpan StartupTimeout { get; }
        Guid Guid {  get; }
        void Start();
    }
}