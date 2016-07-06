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
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Artivity.DataModel;
using Artivity.Api.Plugin;
using Artivity.Api.Plugin.Win;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;

namespace Artivity.Api.Plugin.Win
{
    public class WinPluginChecker : PluginChecker
    {       
        #region Constructors

        public WinPluginChecker(IModelProvider modelProvider, DirectoryInfo dir) : base(modelProvider, dir)
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

        protected override DirectoryInfo GetApplicationLocation(PluginManifest manifest)
        {
            RegistryEntry entry = InstalledPrograms.FindInstalledProgram(manifest.ID);

            if (entry != null)
            {
                return new DirectoryInfo(entry.InstallLocation);
            }

            return null;
        }

        public override bool IsPluginInstalled(PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation(manifest);

            if (location == null || !location.Exists)
            {
                return false;
            }

            foreach (PluginManifestPluginFile file in manifest.PluginFile)
            {
                var targetFolder = Path.Combine(location.FullName, manifest.TargetPath);
                var targetFile = Path.Combine(targetFolder, file.GetName());

                if (!File.Exists(targetFile))
                {
                    return false;
                }
            }

            bool is64Bit = Environment.Is64BitOperatingSystem;

            foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
            {
                if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                {
                    continue;
                }

                if(!HasRegistryKey(key))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool InstallPlugin(PluginManifest manifest)
        {
            if (base.InstallPlugin(manifest))
            {
                bool is64Bit = Environment.Is64BitOperatingSystem;

                foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
                {
                    if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                    {
                        continue;
                    }

                    if (!CreateRegistryKey(key))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override bool UninstallPlugin(PluginManifest manifest)
        {
            if (base.UninstallPlugin(manifest))
            {
                bool is64Bit = Environment.Is64BitOperatingSystem;

                foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
                {
                    if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                    {
                        continue;
                    }

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
        protected override bool CreateRegistryKey(PluginManifestRegistryKey key)
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
        protected override bool DeleteRegistryKey(PluginManifestRegistryKey key)
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

        protected override bool HasRegistryKey(PluginManifestRegistryKey key)
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

