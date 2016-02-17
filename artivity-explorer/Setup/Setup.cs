using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Semiodesk.Trinity;
using Artivity.DataModel;
using System.Linq;

namespace Artivity.Explorer
{
    public class Setup
    {
        #region Members

        private static string _appDataFolderName = "artivity";

        private static string _desktopFileSource = "/usr/share/applications/artivity-apid.desktop";

        private static string _desktopFileTarget = ".config/autostart/artivity-apid.desktop";

        #endregion

        #region Constructors
                    
        public static bool IsLinuxPlatform()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        public static bool IsMacPlatform()
        {
            return Environment.OSVersion.Platform == PlatformID.MacOSX;
        }

        public static bool IsWindowsPlatform()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        public static string GetIconExtension()
        {
            return IsWindowsPlatform() ? ".ico" : ".png";
        }

        public static string GetUserHomeFolder()
        {
            if (IsLinuxPlatform() || IsMacPlatform())
            {
                return Environment.GetEnvironmentVariable("HOME");
            }
            else
            {
                return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
        }

        public static string GetAppDataFolder()
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appDataFolderName);

            if(!Directory.Exists(appData))
            {
                try
                {
                    Directory.CreateDirectory(appData);
                }
                catch(IOException e)
                {
                    Console.WriteLine(e);
                }
            }

            return appData;
        }

        public static bool CheckEnvironment()
        {
            return HasApiDaemonAutostart() && HasUserAgent();
        }

        public static bool HasApiDaemonAutostart()
        {
            if (IsLinuxPlatform())
            {
                string desktopTarget = Path.Combine(GetUserHomeFolder(), _desktopFileTarget);

                return File.Exists(desktopTarget);
            }
            else
            {
                //throw new PlatformNotSupportedException();
                return true;
            }
        }

        public static bool InstallApiDaemonAutostart()
        {
            if (IsLinuxPlatform())
            {
                string desktopSource = Path.GetFullPath(_desktopFileSource);
                string desktopTarget = Path.Combine(GetUserHomeFolder(), _desktopFileTarget);

                try
                {
                    if(!File.Exists(desktopSource))
                    {
                        throw new FileNotFoundException(desktopSource);
                    }

                    Console.WriteLine("Installing Artivity API daemon autostart..");

                    // Make sure that the target directory exists..
                    string autostartDirectory = Path.GetDirectoryName(desktopTarget);

                    if(!Directory.Exists(autostartDirectory))
                    {
                        Directory.CreateDirectory(autostartDirectory);
                    }

                    // Copy the autostart .desktop file into the config directory for GNOME.
                    File.Copy(desktopSource, desktopTarget);

                    return true;
                }
                catch(IOException e)
                {
                    Console.WriteLine(e);

                    return false;
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public static void UninstallApiDaemonAutostart()
        {
            if (IsLinuxPlatform())
            {
                string desktopTarget = Path.Combine(GetUserHomeFolder(), _desktopFileTarget);

                try
                {
                    Console.WriteLine("Uninstalling Artivity API daemon autostart..");

                    File.Delete(desktopTarget);
                }
                catch(IOException e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public static bool TryStartApiDaemon()
        {
            if (IsLinuxPlatform())
            {
                try
                {
                    using(StreamReader file = new StreamReader(_desktopFileSource))
                    {
                        string line;

                        while((line = file.ReadLine()) != null)
                        {
                            Match match = Regex.Match(line, "Exec=(?<bin>.*)");

                            if(!match.Success) continue;

                            string bin = match.Groups["bin"].Value;

                            if(File.Exists(bin))
                            {
                                file.Close();

                                Console.WriteLine("Starting Artivity API daemon {0}..", bin);

                                Process process = new Process();
                                process.StartInfo.FileName = "/usr/bin/artivity-apid";
                                process.StartInfo.WorkingDirectory = GetUserHomeFolder();
                                process.StartInfo.RedirectStandardInput = false;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.UseShellExecute = false;
                                process.ErrorDataReceived += OnProcessErrorDataReceived;
                                process.OutputDataReceived += OnProcessOutputDataReceived;

                                process.Start();
                                process.EnableRaisingEvents = true;

                                process.BeginErrorReadLine();
                                process.BeginOutputReadLine();

                                return true;
                            }
                            else
                            {
                                throw new FileNotFoundException(bin);
                            }
                        }

                        file.Close();

                        return false;
                    }
                }
                catch(IOException e)
                {
                    Console.WriteLine(e);

                    return false;
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private static void OnProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void OnProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        public static bool HasModels()
        {
            bool result = true;

            IStore store = StoreFactory.CreateStore(Models.Instance.Provider.ConnectionString);

            result &= store.ContainsModel(Models.Instance.Provider.Agents) && !store.GetModel(Models.Instance.Provider.Agents).IsEmpty;
            result &= store.ContainsModel(Models.Instance.Provider.Activities);
            result &= store.ContainsModel(Models.Instance.Provider.WebActivities);

            return result;
        }

        public static bool InstallModels()
        {
            try
            {
                Console.WriteLine("Installing database models..");

                IStore store = StoreFactory.CreateStore(Models.Instance.Provider.ConnectionString);

                IModel agents;

                if (!store.ContainsModel(Models.Instance.Provider.Agents))
                {
                    agents = store.CreateModel(Models.Instance.Provider.Agents);
                }
                else
                {
                    agents = store.GetModel(Models.Instance.Provider.Agents);
                }
                    
                InstallAgentIfMissing(agents, "application://inkscape.desktop/", "Inkscape", "inkscape", "#EE204E", true);
                InstallAgentIfMissing(agents, "application://krita.desktop/", "Krita", "krita", "#926EAE", true);
                InstallAgentIfMissing(agents, "application://chromium-browser.desktop/", "Chromium", "chromium-browser", "#1F75FE");
                InstallAgentIfMissing(agents, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");
                InstallAgentIfMissing(agents, "application://photoshop.desktop", "Photoshop", "photoshop", "#EE2000", true);

                IModel activities;

                if (!store.ContainsModel(Models.Instance.Provider.Activities))
                {
                    activities = store.CreateModel(Models.Instance.Provider.Activities);
                }
                else
                {
                    activities = store.GetModel(Models.Instance.Provider.Activities);
                }

                IModel webActivities;

                if (!store.ContainsModel(Models.Instance.Provider.WebActivities))
                {
                    webActivities = store.CreateModel(Models.Instance.Provider.WebActivities);
                }
                else
                {
                    webActivities = store.GetModel(Models.Instance.Provider.WebActivities);
                }

                InstallMonitoring();

                IModel monitoring = store.GetModel(Models.Instance.Provider.Monitoring);

                // Load the ontologies into the database for inferencing support.
                store.LoadOntologySettings();

                return agents != null && !agents.IsEmpty && activities != null && webActivities != null && monitoring != null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        public static void InstallAgentIfMissing(IModel model, string uri, string name, string executableName, string colour, bool captureEnabled = false)
        {
            UriRef agentUri = new UriRef(uri);

            if (!model.ContainsResource(agentUri))
            {
                Console.WriteLine("Installing agent {0}..", name);
                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(agentUri);
                agent.Name = name;
                agent.ExecutableName = executableName;
                agent.IsCaptureEnabled = captureEnabled;
                agent.ColourCode = colour;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(agentUri);

                if (agent.Name != name)
                {
                    agent.Name = name;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = colour;

                    modified = true;
                }

                if (modified)
                {
                    Console.WriteLine("Updating agent {0}..", name);
                    agent.Commit();
                }
            }
        }

        public static void InstallMonitoring(IStore store = null)
        {
            Uri monitoringUri = Models.Instance.Provider.Monitoring;
            if(store == null)
            {
                store = StoreFactory.CreateStore(Models.Instance.ConnectionString);
            }

            IModel model;

            if (!store.ContainsModel(monitoringUri))
            {
                model = store.CreateModel(monitoringUri);
            }
            else
            {
                model = store.GetModel(monitoringUri);
                model.Clear();
            }

            Database database = model.CreateResource<Database>();

            if (IsLinuxPlatform())
            {
                database.Url = "file:///var/lib/virtuoso-opensource-6.1/db/virtuoso.db";
                database.IsMonitoringEnabled = false;
                database.Commit();
            }
        }

        public static bool UninstallModels()
        {
            try
            {
                Console.WriteLine("Uninstalling database models..");

                IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

                if (store.ContainsModel(Models.Instance.Provider.Agents))
                {
                    store.RemoveModel(Models.Instance.Provider.Agents);
                }

                if (store.ContainsModel(Models.Instance.Provider.Activities))
                {
                    store.RemoveModel(Models.Instance.Provider.Activities);
                }

                if (store.ContainsModel(Models.Instance.Provider.WebActivities))
                {
                    store.RemoveModel(Models.Instance.Provider.WebActivities);
                }

                if (store.ContainsModel(Models.Instance.Provider.Monitoring))
                {
                    store.RemoveModel(Models.Instance.Provider.Monitoring);
                }

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        public static bool HasUserAgent()
        {
            Uri agentsUri = Models.Instance.Provider.Agents;
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            if (!store.ContainsModel(agentsUri))
            {
                return false;
            }

            IModel agents = store.GetModel(agentsUri);

            Person user = agents.GetResources<Person>().FirstOrDefault();

            return user != null && user.EmailAddress != null;
        }

        internal static void VerfiyIntegrity()
        {
            Uri agentsUri = Models.Instance.Provider.Agents;
            IStore store = StoreFactory.CreateStore(Models.Instance.ConnectionString);

            IModel agents;

            if (!store.ContainsModel(agentsUri))
            {
                agents = store.CreateModel(agentsUri);
            }
            else
            {
                agents = store.GetModel(agentsUri);
            }

            InstallAgentIfMissing(agents, "application://inkscape.desktop/", "Inkscape", "inkscape", "#EE204E", true);
            InstallAgentIfMissing(agents, "application://krita.desktop/", "Krita", "krita", "#926EAE", true);
            InstallAgentIfMissing(agents, "application://chromium-browser.desktop/", "Chromium", "chromium-browser", "#1F75FE");
            InstallAgentIfMissing(agents, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");
            InstallAgentIfMissing(agents, "application://photoshop.desktop", "Photoshop", "photoshop", "#EE2000", true);
        }
        #endregion

    }
}

