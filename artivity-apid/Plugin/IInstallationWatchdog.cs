using Artivity.Api.Plugin.Win;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Plugin
{
    public delegate void HandleProgramInstalledOrRemoved(object sender, EventArgs args);

    public interface IInstallationWatchdog
    {
        void Start();
        event HandleProgramInstalledOrRemoved ProgrammInstalledOrRemoved;
        void Stop();
    }


    public class InstallationWatchdogFactory
    {
        public static IInstallationWatchdog CreateWatchdog()
        {
            #if WIN
            return new Artivity.Api.Plugin.Win.InstallationWatchdog(InstalledPrograms.RegistryKeyString);
            #elif OSX
            return new Artivity.Api.Plugin.OSX.InstallationWatchdog();
            #endif
        }
    }
}
