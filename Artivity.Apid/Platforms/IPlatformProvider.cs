﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Platforms
{
    public interface IPlatformProvider
    {
        #region Deployment Data
        string AppDataFolder { get; }
        string ArtivityUserDataFolder { get; }
        string UserFolder { get; }
        string ThumbnailFolder { get; }
        string DatabaseFolder { get; }
        string DatabaseName { get; }
        string DeploymentDir { get; }
        string PluginDir { get; }
        #endregion

        #region Platform Information
        bool IsLinux { get; }
        bool IsMac { get; }
        bool IsWindows { get; }
        #endregion

        #region Settings

        bool CheckForNewSoftwareAgents { get; set; }
        bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        #endregion



    }
}
