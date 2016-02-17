using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Artivity.Api.Http
{
    class Platform
    {
        public static bool IsRunningOnMac()
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

        [DllImport("libc")]
        static extern int uname(IntPtr buf);

    }
}
