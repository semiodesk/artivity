using System;
using CommandLine;

namespace Artivity.Api.Http
{
    public class Options
    {
        #region Members

        [Option('u', "update", Required = false, HelpText = "Updates the ontologies in the database which are used for inferencing.")]
        public bool Update { get; set; }

        #endregion
    }
}

