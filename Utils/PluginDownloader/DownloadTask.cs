using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PluginDownloader
{
    class DownloadTask : Task
    {
        #region Members
        [Required]
        public string Host { get; set; }

        [Required]
        public string TargetDirectory { get; set; }
        #endregion

        #region Methods

        public override bool Execute()
        {
            Downloader.DownloadPlugins(new Uri(Host), new System.IO.DirectoryInfo(TargetDirectory));
            return true;
        }

        #endregion
    }
}
