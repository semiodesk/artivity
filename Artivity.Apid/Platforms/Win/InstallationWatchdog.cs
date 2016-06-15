
using Microsoft.Win32;
using RegistryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Plugin.Win
{
    class InstallationWatchdog : IInstallationWatchdog
    {
        #region Members
        RegistryKey _key32;
        RegistryKey _key64;

        public event HandleProgramInstalledOrRemoved ProgrammInstalledOrRemoved;

        private RegistryMonitor _monitor32;
        private RegistryMonitor _monitor64;

        private string _registryKey;
        #endregion

        #region Constructor
        public InstallationWatchdog(string registryKey)
        {
            _registryKey = registryKey;
        }
        #endregion

        #region Methods
        public void Start()
        {
            //_key32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(InstalledPrograms.RegistryKeyString);
            _key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(_registryKey);

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
            if (ProgrammInstalledOrRemoved != null)
                ProgrammInstalledOrRemoved(this, null);
        }

        public void Stop()
        {
            if( _monitor32 != null)
                _monitor32.Stop();

            if( _monitor64 != null)
                _monitor64.Stop();

            if( _key32 != null)
            _key32.Dispose();

            if( _key64 != null)
                _key64.Dispose();
        }

        public void Dispose ()
        {
        }
        #endregion



    }
}
