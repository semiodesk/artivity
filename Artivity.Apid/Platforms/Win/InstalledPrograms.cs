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
//

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Artivity.Api.Plugin.Win
{
    public class InstalledPrograms
    {
        #region Members

        public const string RegistryKeyString = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        const string registry_key2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        #endregion

        #region Methods

        public static RegistryEntry FindInstalledProgram(string key)
        {
            RegistryEntry entry = FindInstalledProgramFromRegistry(RegistryView.Registry32, key);

            if (entry == null)
            {
                entry = FindInstalledProgramFromRegistry(RegistryView.Registry64, key);
            }

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

            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            var releaseType = (string)subkey.GetValue("ReleaseType");

            if (!string.IsNullOrEmpty(releaseType))
            {
                return false;
            }

            //var unistallString = (string)subkey.GetValue("UninstallString");

            var parentName = (string)subkey.GetValue("ParentDisplayName");

            if (!string.IsNullOrEmpty(releaseType))
            {
                return false;
            }

            var systemComponent = subkey.GetValue("SystemComponent");

            if (systemComponent != null)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
