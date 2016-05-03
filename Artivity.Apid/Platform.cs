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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Artivity.Apid
{
    class Platform
    {
        private const string _appDataFolderName = "Artivity";

        public static bool IsLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);

        public static bool IsMac()
        {
            string os = string.Empty;

            IntPtr buffer = IntPtr.Zero;

            try
            {
                buffer = Marshal.AllocHGlobal(8192);

                // This is a hacktastic way of getting sysname from uname()..
                if (uname(buffer) == 0)
                {
                    os = Marshal.PtrToStringAnsi(buffer).ToLowerInvariant();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

            return os == "darwin"; ;
        }

        public static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        public static string GetAppDataFolder()
        {
            return GetSpecialFolder(Environment.SpecialFolder.ApplicationData, _appDataFolderName);
        }

        public static string GetAppDataFolder(string subFolder)
        {
            string appData = GetSpecialFolder(Environment.SpecialFolder.ApplicationData, _appDataFolderName);

            return Path.Combine(appData, subFolder);
        }

        private static string GetSpecialFolder(Environment.SpecialFolder folder, string subfolder)
        {
            string appData = Path.Combine(Environment.GetFolderPath(folder), subfolder);

            if (!Directory.Exists(appData))
            {
                try
                {
                    Directory.CreateDirectory(appData);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }

            return appData;
        }
    }
}
