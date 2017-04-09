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

using Artivity.Api;
using Artivity.Api.IO;
using Artivity.Api.Platform;
using Artivity.Apid.IO;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Artivity.Apid.Platform
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

        public string TempFolder { get; private set; }

        public bool IsLinux { get; protected set; }

        public bool IsMac { get; protected set; }

        public bool IsWindows { get; protected set; }

        public string PluginDir { get; protected set; }

        public string DeploymentDir { get; protected set; }

        public string OntologyDir { get; set; }

        public bool CheckForNewSoftwareAgents { get; set; }

        public bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        public ILogger Logger
        {
            get;
            private set;
        }

        public bool DidSetupRun { get { return Config.RunSetup; } set { Config.RunSetup = value; } }

        public bool RequiresAuthentication { get { return false; } }



        #endregion

        #region Constructors

        public PlatformProvider(string appDataFolder, string userFolder, string userName)
        {
            Logger = new Logger();

            AppDataFolder = appDataFolder;

            ArtivityDataFolder = Path.Combine(AppDataFolder, "Artivity");
            EnsureFolderExists(ArtivityDataFolder);

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

            TempFolder = Path.Combine(ArtivityDataFolder, "Temp");
            EnsureFolderExists(TempFolder);

            // Don't create the folder if it doesn't exist. TinyVirtuoso does that properly.
            DatabaseName = "Data";
            DatabaseFolder = Path.Combine(ArtivityDataFolder, DatabaseName);

            UserFolder = userFolder;
            UserName = userName;

            IsWindows = TestWindows();
            IsMac = TestMac();
            IsLinux = TestLinux();

            SetDeploymentDir(Environment.CurrentDirectory);


        }

        #endregion

        #region Methods

        public void SetDeploymentDir(string dir)
        {
            DeploymentDir = dir;
            PluginDir = Path.Combine(DeploymentDir, "Plugins");
        }

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
                config.RunSetup = true;
                config.Uid = "urn:art:uid:" + Guid.NewGuid();
                config.CreateActivityStepsRenderings = false;

                string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                File.WriteAllText(configFile, json);
            }

            return config;
        }

        public void WriteConfig(UserConfig config)
        {
            if (File.Exists(ConfigFile))
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                File.WriteAllText(ConfigFile, json);
            }
        }

        public void AddFile(UriRef uri, Uri url)
        {
            FileSystemMonitor.Instance.AddFile(uri, url);
        }

        public string EncodeFileName(string str)
        {
            return FileNameEncoder.Encode(str);
        }

        public string GetRenderOutputPath(UriRef entityUri)
        {
            string entityName = FileNameEncoder.Encode(entityUri.AbsoluteUri);

            return Path.Combine(RenderingsFolder, entityName);
        }
        #endregion


    }
}
