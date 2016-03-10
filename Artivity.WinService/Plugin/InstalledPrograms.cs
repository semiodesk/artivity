using Artivity.WinService.Plugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.WinService.Plugin
{
    public static class InstalledPrograms
    {
        public const string RegistryKeyString = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        const string registry_key2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        public static RegistryEntry FindInstalledProgram(string key)
        {
            RegistryEntry entry = FindInstalledProgramFromRegistry(RegistryView.Registry32, key);
            if (entry == null)
                entry = FindInstalledProgramFromRegistry(RegistryView.Registry64, key);

            return entry;
        }

        public static List<string> GetInstalledPrograms()
        {
            var result = new List<string>();
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry32));
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry64));
            return result;
        }

        private static RegistryEntry FindInstalledProgramFromRegistry(RegistryView view, string id)
        {
            RegistryEntry entry = null;
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view).OpenSubKey(RegistryKeyString))
            {
                using (RegistryKey subkey = key.OpenSubKey(id))
                {
                    if( subkey != null)
                    { 
                        entry = new RegistryEntry();
                        entry.Id = id;
                        entry.Name = (string)subkey.GetValue("DisplayName");
                        entry.InstallLocation = (string)subkey.GetValue("InstallLocation");
                    }
                }
            }
            return entry;
        }

        private static IEnumerable<string> GetInstalledProgramsFromRegistry(RegistryView registryView)
        {
            var result = new List<string>();

            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(RegistryKeyString))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (IsProgramVisible(subkey))
                        {
                            string name = (string)subkey.GetValue("DisplayName");
                            string loc = (string)subkey.GetValue("InstallLocation");
                            result.Add(string.Format("{0} => {1}", name, loc));
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            //var unistallString = (string)subkey.GetValue("UninstallString");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
