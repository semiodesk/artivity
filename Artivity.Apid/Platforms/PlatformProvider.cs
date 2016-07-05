using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Artivity.Apid.Platforms
{
    public class PlatformProvider : IPlatformProvider
    {
        #region Members
        public string AppDataFolder
        {
            get;
            private set;
        }

        public string ArtivityUserDataFolder
        {
            get;
            private set;
        }

        public string ThumbnailFolder
        {
            get;
            private set;
        }

        public string UserFolder
        {
            get;
            private set;
        }

        public string DatabaseName { get; private set; }

        public string DatabaseFolder { get; private set; }

        public string UserName
        {
            get;
            private set;
        }

        public bool IsLinux
        {
            get;
            protected set;
        }

        public bool IsMac
        {
            get;
            protected set;
        }

        public bool IsWindows
        {
            get;
            protected set;
        }

        public string PluginDir { get; set; }

        public string DeploymentDir { get; set; }

        public bool CheckForNewSoftwareAgents { get; set; }

        public bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        #endregion

        #region Constructors

        public PlatformProvider(string appDataFolder, string userFolder, string userName)
        {
            AppDataFolder = appDataFolder;
            ArtivityUserDataFolder = Path.Combine(AppDataFolder, "Artivity");
            EnsureFolderExists(ArtivityUserDataFolder);

            ThumbnailFolder = Path.Combine(ArtivityUserDataFolder, "Renderings");
            EnsureFolderExists(ThumbnailFolder);

            DatabaseName = "Data";
            DatabaseFolder = Path.Combine(ArtivityUserDataFolder, DatabaseName);
            EnsureFolderExists(DatabaseFolder);

            UserFolder = userFolder;
            UserName = userName;

            IsWindows = TestWindows();
            IsMac = TestMac();
            IsLinux = TestLinux();

            DeploymentDir = Environment.CurrentDirectory;
            PluginDir = Path.Combine(DeploymentDir, "Plugins");

            CheckForNewSoftwareAgents = false;
            AutomaticallyInstallSoftwareAgentPlugins = false;
        }

        #endregion

        #region Methods

        protected void EnsureFolderExists(string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            if (!dir.Exists )
                dir.Create();
        }

        public bool TestLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);

        public bool TestMac()
        {
            #if OSX
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
            #else
            return false;
            #endif
        }

        public bool TestWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
        #endregion
    }
}
