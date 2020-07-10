using System;
using CommandLine;

namespace AddinHost {
    public class Options {
        [Option( "guid", Required = true, HelpText = "Guid")]
        public Guid Guid { get; set; }
        [Option( "pid", Required = true, HelpText = "Process id")]
        public int Pid { get; set; }
        [Option("location", Required = false, HelpText = "Addins location")]
        public string AddinsLocation { get; set; }
        [Option("pattern", Required = false, HelpText = "Addins search pattern")]
        public string SearchPattern { get; set; }
    }
}