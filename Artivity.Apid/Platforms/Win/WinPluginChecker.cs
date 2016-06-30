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
using System.Text;

namespace Artivity.Api.Plugin.Win
{
    public class WinPluginChecker : PluginChecker
    {       
        #region Constructor

        public WinPluginChecker(IModelProvider modelProvider, DirectoryInfo dir) : base(modelProvider, dir)
        {
            
        }

        #endregion
        
        #region implemented abstract members of PluginChecker

        protected override DirectoryInfo GetApplicationLocation (PluginManifest manifest)
        {
            RegistryEntry entry = InstalledPrograms.FindInstalledProgram(manifest.ID);

            if (entry != null)
            {
                return new DirectoryInfo(entry.InstallLocation);
            }

            return null;
        }

        protected override void CreateLink (string target, string source)
        {
            IShellLink link = (IShellLink)new ShellLink();

            // setup shortcut information
            link.SetDescription("Artivity Plugin");
            link.SetPath(source);

            // save it
            System.Runtime.InteropServices.ComTypes.IPersistFile file = (System.Runtime.InteropServices.ComTypes.IPersistFile)link;
            file.Save(target, false);
        }

        protected override string GetApplicationVersion(FileSystemInfo app)
        {
            string res = null;

            if( app is FileInfo && app.Exists )
            {
                var fi = app as FileInfo;
                var info = FileVersionInfo.GetVersionInfo(fi.FullName);
                return info.ProductVersion;
            }


            return res;

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

