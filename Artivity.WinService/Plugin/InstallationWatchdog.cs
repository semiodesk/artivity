using Microsoft.Win32;
using RegistryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.WinService.Plugin
{
    delegate void HandleProgramInstalled(object sender, EventArgs args);

    class InstallationWatchdog
    {
        #region Members
        RegistryKey _key32;
        RegistryKey _key64;

        public event HandleProgramInstalled ProgrammInstalledOrRemvoed;

        private RegistryMonitor _monitor32;
        private RegistryMonitor _monitor64;
        #endregion

        #region Constructor
        public InstallationWatchdog()
        {
        }
        #endregion

        #region Methods
        public void Start()
        {
            //_key32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(InstalledPrograms.RegistryKeyString);
            _key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(InstalledPrograms.RegistryKeyString);

            //_monitor32 = new RegistryMonitor(_key32);
            //_monitor32.RegChangeNotifyFilter = RegChangeNotifyFilter.Key;
            //_monitor32.RegChanged += RegChanged;
            //_monitor32.Start();

            _monitor64 = new RegistryMonitor(_key64);
            _monitor64.RegChangeNotifyFilter = RegChangeNotifyFilter.Key;
            _monitor64.RegChanged += RegChanged;
            _monitor64.Start();
        }

        void RegChanged(object sender, EventArgs e)
        {
            if (ProgrammInstalledOrRemvoed != null)
                ProgrammInstalledOrRemvoed(this, null);
        }

        public void Stop()
        {
            _monitor32.Stop();
            _monitor64.Stop();

            _key32.Dispose();
            _key64.Dispose();
        }
        #endregion
    }
}
