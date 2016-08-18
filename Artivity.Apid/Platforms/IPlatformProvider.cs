using Artivity.Apid.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Platforms
{
    public interface IPlatformProvider
    {
        #region Deployment

        string AppDataFolder { get; }

        string ArtivityDataFolder { get; }

        string AvatarsFolder { get; }

        UserConfig Config { get; }

        string UserFolder { get; }

        string RenderingsFolder { get; }

        string ExportFolder { get; }

        string ImportFolder { get; }

        string DatabaseFolder { get; }

        string DatabaseName { get; }

        string DeploymentDir { get; }

        string PluginDir { get; }

        string OntologyDir { get; }

        #endregion

        #region Platform

        bool IsLinux { get; }

        bool IsMac { get; }

        bool IsWindows { get; }

        #endregion

        #region Settings

        bool CheckForNewSoftwareAgents { get; set; }

        bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        #endregion

        #region Methods

        void WriteConfig(UserConfig config);

        #endregion
    }
}
