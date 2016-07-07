﻿using Artivity.Api.Platforms;
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

        string DatabaseFolder { get; }

        string DatabaseName { get; }

        string DeploymentDir { get; }

        string PluginDir { get; }

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
    }
}
