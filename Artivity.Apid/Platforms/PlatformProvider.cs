using Artivity.Api.Platforms;
using Newtonsoft.Json;
using Semiodesk.Trinity;
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

        public string AppDataFolder { get; private set; }

        public string ArtivityDataFolder { get; private set; }

        public string AvatarsFolder { get; private set; }

        public string ConfigFile { get; private set; }

        public UserConfig Config { get; private set; }

        public string UserName { get; private set; }

        public string UserFolder { get; private set; }

        public string DatabaseName { get; private set; }

        public string DatabaseFolder { get; private set; }

        public string RenderingsFolder { get; private set; }

        public string ExportFolder { get; private set; }

        public string ImportFolder { get; private set; }

        public bool IsLinux { get; protected set; }

        public bool IsMac { get; protected set; }

        public bool IsWindows { get; protected set; }

        public string PluginDir { get; set; }

        public string DeploymentDir { get; set; }

        public string OntologyDir { get; set; }

        public bool CheckForNewSoftwareAgents { get; set; }

        public bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        #endregion

        #region Constructors

        public PlatformProvider(string appDataFolder, string userFolder, string userName)
        {
            AppDataFolder = appDataFolder;

            // Don't create the folder if it doesn't exist. TinyVirtuoso does that properly.
            ArtivityDataFolder = Path.Combine(AppDataFolder, "Artivity");

            ConfigFile = Path.Combine(ArtivityDataFolder, "config.json");
            Config = GetUserConfig(ConfigFile);

            AvatarsFolder = Path.Combine(ArtivityDataFolder, "Avatars");
            EnsureFolderExists(AvatarsFolder);

            RenderingsFolder = Path.Combine(ArtivityDataFolder, "Renderings");
            EnsureFolderExists(RenderingsFolder);

            ExportFolder = Path.Combine(ArtivityDataFolder, "Export");
            EnsureFolderExists(ExportFolder);

            ImportFolder = Path.Combine(ArtivityDataFolder, "Import");
            EnsureFolderExists(ImportFolder);

            DatabaseName = "Data";
            DatabaseFolder = Path.Combine(ArtivityDataFolder, DatabaseName);
            EnsureFolderExists(DatabaseFolder);

            UserFolder = userFolder;
            UserName = userName;

            IsWindows = TestWindows();
            IsMac = TestMac();
            IsLinux = TestLinux();

            DeploymentDir = Environment.CurrentDirectory;
            PluginDir = Path.Combine(DeploymentDir, "Plugins");
        }

        #endregion

        #region Methods

        protected void EnsureFolderExists(string folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);

            if (!directory.Exists)
            {
                directory.Create();
            }
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

        private UserConfig GetUserConfig(string configFile)
        {
            UserConfig config;

            if(File.Exists(configFile))
            {
                Logger.LogInfo("Reading config file: {0}", configFile);

                using (StreamReader reader = new StreamReader(configFile))
                {
                    string json = reader.ReadToEnd();

                    config = JsonConvert.DeserializeObject<UserConfig>(json);
                }
            }
            else
            {
                Logger.LogInfo("Creating config file: {0}", configFile);

                config = new UserConfig();
                config.IsNew = true;
                config.Uid = "urn:art:uid:" + Guid.NewGuid();

                string json = JsonConvert.SerializeObject(config);

                File.WriteAllText(configFile, json);
            }

            return config;
        }

        public void WriteConfig(UserConfig config)
        {
            if (File.Exists(ConfigFile))
            {
                string json = JsonConvert.SerializeObject(config);

                File.WriteAllText(ConfigFile, json);
            }
        }

        #endregion
    }
}
