using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Artivity.Api.Plugin
{
    public class SoftwareAgentPlugin
    {
        #region Members

        internal PluginManifest Manifest { get; private set;}

        public bool IsSoftwareInstalled { get; set; }

        public bool IsPluginInstalled { get; set; }

        public bool IsPluginEnabled { get; set; }

        public string DetectedVersion { get; set; }

        #endregion

        #region Constructors

        public SoftwareAgentPlugin(PluginManifest manifest)
        {
            Manifest = manifest;
        }

        #endregion

        #region Methods

        public Uri GetIcon()
        {
            FileInfo file = new FileInfo("Icon.png");

            return file.Exists ? file.ToUriRef() : null;
        }

        public UriRef GetAssociationUri()
        {
            string version = "unknown";

            if (!string.IsNullOrEmpty(DetectedVersion))
            {
                version = DetectedVersion;
            }
            else if(!string.IsNullOrEmpty(Manifest.GetVersion()))
            {
                version = Manifest.GetVersion();
            }

            return new UriRef(Manifest.AgentUri + "/" + version);
        }

        #endregion
    }
}
