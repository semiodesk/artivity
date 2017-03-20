using Artivity.Api.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtivityTest
{
    public class TestPlatformProvider : IPlatformProvider
    {
        public Artivity.Api.ILogger Logger
        {
            get;
            set;
        }

        public UserConfig Config
        {
            get;
            set;
        }

        public string ConfigFile
        {
            get;
            set;
        }

        public string AppDataFolder
        {
            get;
            set;
        }

        public string ArtivityDataFolder
        {
            get;
            set;
        }

        public string AvatarsFolder
        {
            get;
            set;
        }

        public string UserFolder
        {
            get;
            set;
        }

        public string RenderingsFolder
        {
            get;
            set;
        }

        public string ExportFolder
        {
            get;
            set;
        }

        public string ImportFolder
        {
            get;
            set;
        }

        public string TempFolder
        {
            get;
            set;
        }

        public string DatabaseFolder
        {
            get;
            set;
        }

        public string DatabaseName
        {
            get;
            set;
        }

        public string DeploymentDir
        {
            get;
            set;
        }

        public string PluginDir
        {
            get;
            set;
        }

        public string OntologyDir
        {
            get;
            set;
        }

        public bool RequiresAuthentication
        {
            get;
            set;
        }

        public bool IsLinux
        {
            get;
            set;
        }

        public bool IsMac
        {
            get;
            set;
        }

        public bool IsWindows
        {
            get;
            set;
        }

        public bool CheckForNewSoftwareAgents
        {
            get;
            set;
        }

        public bool AutomaticallyInstallSoftwareAgentPlugins
        {
            get;
            set;
        }

        public bool DidSetupRun
        {
            get;
            set;
        }

        public void WriteConfig(UserConfig config)
        {

        }

        public void AddFile(Semiodesk.Trinity.UriRef uri, Uri url)
        {

        } 

        public string EncodeFileName(string str)
        {
            return "";
        }

        public string GetRenderOutputPath(Semiodesk.Trinity.UriRef entityUri)
        {
            return "";
        }
    }
}
