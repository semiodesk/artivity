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

        public PluginManifest Manifest { get; private set;}

        public bool IsSoftwareInstalled { get; set; }

        public bool IsPluginInstalled { get; set; }

        public bool IsPluginEnabled { get; set; }

        public string DetectedVersion { get; set; }

        public UriRef AssociationUri
        {
            get { return GetAssociationUri(); }
        }

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
            FileInfo file = new FileInfo(Path.Combine(Manifest.ManifestFile.DirectoryName, "Icon.png"));

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
