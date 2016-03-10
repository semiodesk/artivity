using Artivity.WinService.Plugin;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.WinService.Plugin
{
    public class PluginChecker
    {

        #region Members
        List<PluginManifest> _manifests = new List<PluginManifest>();

        private static log4net.ILog _logger;
        private static log4net.ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    Type type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
                    _logger = log4net.LogManager.GetLogger(type);
                }
                return _logger;
            }
        }
        #endregion

        #region Constructor
        public PluginChecker()
        {
            LoadManifests();
        }
        #endregion


        #region Methods

        private void LoadManifests()
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            foreach (var plugin in dir.EnumerateDirectories())
            {
                var manifest = PluginManifestReader.ReadManifest(plugin);
                if (manifest != null)
                    _manifests.Add(manifest);
                else
                    Logger.ErrorFormat("Directory {0} did not contain valid manifest.", plugin.FullName);
            }
        }

        public void Check()
        {
            foreach (var man in _manifests)
            {
                RegistryEntry entry = InstalledPrograms.FindInstalledProgram(man.ID);
                bool? res = IsPluginInstalled(man, entry);
                if (res.HasValue && !res.Value)
                {
                    InstallPlugin(man, entry);
                }
            }
        }

        private bool? IsPluginInstalled(PluginManifest manifest, RegistryEntry entry)
        {
            if(entry != null && !string.IsNullOrEmpty(entry.InstallLocation))
            {
                if(Directory.Exists(entry.InstallLocation))
                {
                    var pluginPath = Path.Combine(entry.InstallLocation, manifest.TargetPath, manifest.PluginFile.GetName());
                    return File.Exists(pluginPath);
                }
                return false;
            }
            return null;
        }

        private void InstallPlugin(PluginManifest manifest, RegistryEntry entry)
        {
            if (!string.IsNullOrEmpty(entry.InstallLocation))
            {
                if (Directory.Exists(entry.InstallLocation))
                {
                    var pluginPath = Path.Combine(entry.InstallLocation, manifest.TargetPath, manifest.PluginFile.GetName());
                    FileInfo source = manifest.GetPluginSourceFile();
                    if (source.Exists)
                    {
                        if (manifest.PluginFile.Link)
                        {
                            Win32.CreateShortcut(pluginPath, source.FullName);
                        }
                        else
                        {
                            File.Copy(source.FullName, pluginPath);
                        }
                    }
                }
            }
        }
        #endregion
    }
}


//var x = Win32.GetFileTypeDescription(".ai");
//var res = InstalledPrograms.GetInstalledPrograms();

//List<string> rr = new List<string>();
//ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
//foreach (ManagementObject mo in mos.Get())
//{
//    rr.Add(string.Format("{0} -> {1}",mo["Name"], mo["InstallLocation"]));
//}

/*
//string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
{
    foreach (string subkey_name in key.GetSubKeyNames())
    {
        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
        {
            Console.WriteLine(subkey.GetValue("DisplayName"));
        }
    }
}*/
