using System;
using CommandLine;

namespace Artivity.Api.Http
{
    public class Options
    {
        #region Members

        [Option('i', "interactive", Required = false, HelpText = "Starts the service in interactive mode.")]
        public bool Interactive { get; set; }

        [Option('u', "update", Required = false, HelpText = "Updates the ontologies in the database which are used for inferencing.")]
        public bool Update { get; set; }

        #endregion
    }
}

