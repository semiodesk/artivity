using Artivity.Apid.Plugin.Win;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Plugin
{
    public delegate void HandleProgramInstalledOrRemoved(object sender, EventArgs args);

    public interface IInstallationWatchdog : IDisposable
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
            return new Artivity.Apid.Plugin.Win.InstallationWatchdog(InstalledPrograms.RegistryKeyString);
            #elif OSX
            return new Artivity.Apid.Plugin.OSX.InstallationWatchdog();
            #endif
        }
    }
}
