using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Api.Plugin
{
    public class SoftwareAgentPlugin
    {
        #region Members
        internal PluginManifest Manifest { get; private set;}

        public bool IsInstalled { get; set; }
        public bool IsSoftwareInstalled { get; set; }
        public string Version { get { return Manifest.Version; } }
        public string Name { get { return Manifest.DisplayName; } }
        public string SoftwareAgentVersion { get { return Manifest.HostVersion; } }
        public string AgentUri { get { return Manifest.Uri; } }
        public string Uri { get { return new Uri(string.Format("{0}#{1}", AgentUri, Manifest.HostVersion)).AbsoluteUri; } }
        #endregion

        #region Constructor
        public SoftwareAgentPlugin(PluginManifest manifest)
        {
            Manifest = manifest;
        }
        #endregion

        #region Methods
        #endregion
    }
}
