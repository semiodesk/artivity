using System;
using CommandLine;

namespace ArtivityExplorer
{
    public class Options
    {
        #region Members

        [Option('s', "single-user", Required = false, HelpText = "Access a global (system-wide) model of older versions of the software.")]
        public bool SingleUser { get; set; }

        #endregion
    }
}

