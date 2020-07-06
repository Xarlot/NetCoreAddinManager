using AddinManager;
using AddinManager.Core;
using FluentAssertions;
using NUnit.Framework;

namespace AddinManagerTests {
    [TestFixture]
    public class AddinProcessTests {
        AddinProcess process;
        [Test]
        public void Test() {
            process = new AddinProcess(ProcessType.NetCore3, new AddinHostBootstrapperOptions());
            process.Start().Should().BeTrue();
        }
        [TearDown]
        public void TearDown() {
            this.process?.Dispose();
            this.process = null;
        }
    }
}