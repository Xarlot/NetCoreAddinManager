using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace TestHost {
    class Program {
        static int Main(string[] args) {
            if (args == null || args.Length != 2 || (!args[0].StartsWith("/guid:", StringComparison.Ordinal) || !args[1].StartsWith("/pid:", StringComparison.Ordinal)))
                return 1;
            string guid = args[0].Remove(0, 6);
            if (guid.Length != 36)
                return 1;
            int pid = Convert.ToInt32(args[1].Remove(0, 5), CultureInfo.InvariantCulture);

            EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "AddinProcess:" + guid);
            eventWaitHandle.Set();
            eventWaitHandle.Close();

            try {
                Process.GetProcessById(pid).WaitForExit();
            }
            catch {
            }
            
            return 0;
        }
    }
}