using System;
using System.Diagnostics;
using System.Globalization;
using AddinManager;
using FluentAssertions;
using NUnit.Framework;

namespace AddinManagerTests {
    public class TestAddinProcess : AddinProcess {
        protected internal TestAddinProcess(Runtime runtime, int parentProcessId, string assemblyLocation, string searchPattern) : base(runtime, parentProcessId, assemblyLocation, searchPattern) {
        }
        public TestAddinProcess(Runtime runtime, string assemblyLocation, string searchPattern) : base(runtime, assemblyLocation, searchPattern) {
        }
        public bool WaitForExit(int timeout) {
            return Process.WaitForExit(timeout);
        }
    }
    
    [TestFixture]
    public class AddinProcessTests {
        [Test]
        public void TerminateAddinHostNetCore3OnExitParent() {
            using Process testProcess = new Process();
            Guid guid = Guid.NewGuid();
            string args = string.Format(CultureInfo.InvariantCulture, "/guid:{0} /pid:{1}", guid, Process.GetCurrentProcess().Id);

            testProcess.EnableRaisingEvents = true;
            testProcess.StartInfo.CreateNoWindow = true;
            testProcess.StartInfo.UseShellExecute = false;
            testProcess.StartInfo.Arguments = args;
            testProcess.StartInfo.FileName = "TestHost.exe";
            testProcess.Start();
            
            using var addinProcess = new TestAddinProcess(Runtime.NetCore3, testProcess.Id, null, null);
            addinProcess.Start();
            
            testProcess.Kill();
            testProcess.WaitForExit();

            addinProcess.WaitForExit(5000).Should().BeTrue();
        }
    }
}