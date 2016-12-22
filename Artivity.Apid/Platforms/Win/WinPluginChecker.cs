// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Artivity.DataModel;
using Artivity.Apid.Platforms;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;
using System.Collections.Generic;

namespace Artivity.Apid.Plugin.Win
{
    public class WinPluginChecker : PluginChecker
    {       
        #region Constructors

        public WinPluginChecker(IPlatformProvider platformProvider, IModelProvider modelProvider, DirectoryInfo dir)
            : base(platformProvider, modelProvider, dir)
        {
        }

        #endregion
        
        #region Methods

        protected override string GetApplicationVersion(FileSystemInfo app)
        {
            string result = null;

            if(app is FileInfo && app.Exists)
            {
                var file = app as FileInfo;
                var info = FileVersionInfo.GetVersionInfo(file.FullName);

                return info.ProductVersion;
            }

            return result;
        }

        protected override IEnumerable<DirectoryInfo> GetApplicationLocations(PluginManifest manifest)
        {
            foreach (PluginManifestRegistryKey key in manifest.RegistryInfo.ApplicationKeys)
            {
                RegistryEntry entry = InstalledPrograms.FindInstalledProgram(key.Name);

                if (entry != null)
                {
                    yield return new DirectoryInfo(entry.InstallLocation);
                }
            }
        }

        protected override IEnumerable<string> TryGetInstalledSoftwareVersions(PluginManifest manifest)
        {
            foreach (DirectoryInfo location in GetApplicationLocations(manifest).Where(l => l.Exists))
            {
                // Search for the executable in the provided application directory.
                foreach (FileSystemInfo info in location.EnumerateFiles(manifest.ProcessName, SearchOption.AllDirectories))
                {
                    string version = GetApplicationVersion(info);

                    if (!string.IsNullOrEmpty(version) && manifest.IsMatch(version))
                    {
                        yield return version;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates a plugin for one of the supported application versions is fully installed.
        /// </summary>
        /// <param name="manifest">A plugin manifest.</param>
        /// <returns><c>false</c> if no plugins are fully installed, <c>true</c> if at least one version is.</returns>
        public override bool IsPluginInstalled(PluginManifest manifest)
        {
            foreach(DirectoryInfo location in GetApplicationLocations(manifest).Where(l => l.Exists))
            {
                // Check if all plugin files are installed in the plugin target folder.
                bool installed = true;

                foreach (PluginManifestPluginFile file in manifest.PluginFile)
                {
                    var targetFolder = Path.Combine(location.FullName, manifest.PluginInstallPath);
                    var targetFile = Path.Combine(targetFolder, file.GetName());

                    if (!File.Exists(targetFile))
                    {
                        installed = false;
                        break;
                    }
                }

                if(!installed)
                {
                    continue;
                }

                // Check if all plugin registry entries are installed.
                string currentPlatform = Environment.Is64BitOperatingSystem ? "win64" : "win32";

                IEnumerable<PluginManifestRegistryKey> pluginKeys = manifest.RegistryInfo.PluginKeys;

                foreach (PluginManifestRegistryKey key in pluginKeys.Where(k => k.Platform.ToLowerInvariant() == currentPlatform))
                {
                    if(!HasRegistryKey(key))
                    {
                        installed = false;
                        break;
                    }
                }

                // Only return true if all criteria are met.
                if (installed)
                {
                    return true;
                }
            }

            // No plugins are installed.
            return false;
        }

        public override bool InstallPlugin(SoftwareAgentPlugin plugin)
        {
            if (base.InstallPlugin(plugin))
            {
                // Only install registry keys which are supported for the current platform.
                string currentPlatform = Environment.Is64BitOperatingSystem ? "win64" : "win32";

                IEnumerable<PluginManifestRegistryKey> pluginKeys = plugin.Manifest.RegistryInfo.PluginKeys;

                foreach (PluginManifestRegistryKey key in pluginKeys.Where(k => k.Platform.ToLowerInvariant() == currentPlatform))
                {
                    if (!CreateRegistryKey(key))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override bool UninstallPlugin(SoftwareAgentPlugin plugin)
        {
            if (base.UninstallPlugin(plugin))
            {
                // Only remove registry keys which are supported for the current platform.
                string currentPlatform = Environment.Is64BitOperatingSystem ? "win64" : "win32";

                IEnumerable<PluginManifestRegistryKey> pluginKeys = plugin.Manifest.RegistryInfo.PluginKeys;

                foreach (PluginManifestRegistryKey key in pluginKeys.Where(k => k.Platform.ToLowerInvariant() == currentPlatform))
                {
                    DeleteRegistryKey(key);
                }

                return true;
            }

            return false;
        }

        protected override bool CreateLink(string source, string target)
        {
            // Shortcut informationn
            IShellLink link = (IShellLink)new ShellLink();
            link.SetDescription("Artivity Plugin");
            link.SetPath(source);

            IPersistFile file = (IPersistFile)link;
            file.Save(target, false);

            return File.Exists(target);
        }

        protected override void DeleteLink(string target)
        {
            if (File.Exists(target))
            {
                File.Delete(target);
            }
        }

        // TODO: We definitly need to add some security mechanism here - i.e. by adding a signature to the plugin Manifest.
        protected bool CreateRegistryKey(PluginManifestRegistryKey key)
        {
            string root = "HKEY_LOCAL_MACHINE\\SOFTWARE\\";

            if (!key.Path.StartsWith(root))
            {
                // We currently do not support any other hives..
                Logger.ErrorFormat("Registry key must be rooted in: {0}", root);

                return false;
            }

            RegistryKey localMachine = null;

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                }
                else
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                }

                // Remove HKEY_LOCAL_MACHINE from the start of the string.
                string path = key.Path.Substring(19);

                using (RegistryKey k = localMachine.OpenSubKey(path, true))
                {
                    if (k == null)
                    {
                        return false;
                    }

                    RegistryKey sk = k.OpenSubKey(key.Name);

                    if (sk == null)
                    {
                        sk = k.CreateSubKey(key.Name);
                    }

                    foreach (PluginManifestRegistryItem item in key.Items)
                    {
                        sk.SetValue(item.Name, item.Value);
                    }

                    sk.Dispose();

                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                localMachine.Dispose();
            }

            return false;
        }


        // TODO: We definitly need to add some security mechanism here - i.e. by adding a signature to the plugin Manifest.
        protected bool DeleteRegistryKey(PluginManifestRegistryKey key)
        {
            string root = "HKEY_LOCAL_MACHINE\\SOFTWARE\\";

            if (!key.Path.StartsWith(root))
            {
                // We currently do not support any other hives..
                Logger.ErrorFormat("Registry key must be rooted in: {0}", root);

                return false;
            }

            RegistryKey localMachine = null;

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                }
                else
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                }

                // Remove HKEY_LOCAL_MACHINE from the start of the string.
                string path = key.Path.Substring(19);

                using (RegistryKey k = localMachine.OpenSubKey(path, true))
                {
                    if (k != null)
                    {
                        k.DeleteSubKeyTree(key.Name);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                localMachine.Dispose();
            }

            return false;
        }

        protected bool HasRegistryKey(PluginManifestRegistryKey key)
        {
            string root = "HKEY_LOCAL_MACHINE\\SOFTWARE\\";

            if (!key.Path.StartsWith(root))
            {
                return false;
            }

            RegistryKey localMachine = null;

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                }
                else
                {
                    localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                }

                // Remove HKEY_LOCAL_MACHINE from the start of the string.
                string path = key.Path.Substring(19);

                using (RegistryKey k = localMachine.OpenSubKey(path))
                {
                    if (k == null)
                    {
                        return false;
                    }

                    using (RegistryKey sk = k.OpenSubKey(key.Name))
                    {
                        if (sk == null)
                        {
                            return false;
                        }

                        foreach (PluginManifestRegistryItem item in key.Items)
                        {
                            if (sk.GetValue(item.Name, null) == null)
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                localMachine.Dispose();
            }

            return false;
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        #endregion
    }
}

