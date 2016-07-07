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

        public string PluginVersion { get { return Manifest.Version; } }

        public string AgentName { get { return Manifest.DisplayName; } }

        private UriRef _agentUri;

        public UriRef AgentUri
        {
            get
            {
                if(_agentUri == null)
                {
                    _agentUri = new UriRef(Manifest.Uri);
                }

                return _agentUri;
            }
        }

        public string AgentColor { get { return Manifest.Color; } }

        private UriRef _associationUri;

        public UriRef AssociationUri
        {
            get
            {
                if(_associationUri == null)
                {
                    _associationUri = new UriRef(string.Format("{0}/{1}", AgentUri, Manifest.HostVersion));
                }

                return _associationUri;
            }
        }

        public string ExecutablePath { get { return Manifest.ExecPath; } }

        public string ExecutableVersion { get { return Manifest.HostVersion; } }

        public Uri ExecutableIcon
        {
            get
            {
                FileInfo file = new FileInfo(Manifest.IconPath);

                return file.Exists ? file.ToUriRef() : null;
            }
        }

        #endregion

        #region Constructors

        public SoftwareAgentPlugin(PluginManifest manifest)
        {
            Manifest = manifest;
        }

        #endregion
    }
}
