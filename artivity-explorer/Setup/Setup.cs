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
            // i.e. C:\Users\{User}\AppData\Roaming\artivity
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appDataFolderName);

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

            IStore store = StoreFactory.CreateStoreFromConfiguration(Models.DefaultStore);

            result &= store.ContainsModel(Models.Agents) && !store.GetModel(Models.Agents).IsEmpty;
            result &= store.ContainsModel(Models.Activities);
            result &= store.ContainsModel(Models.WebActivities);

            return result;
        }

        public static bool InstallModels()
        {
            try
            {
                Console.WriteLine("Installing database models..");

                IStore store = StoreFactory.CreateStoreFromConfiguration(Models.DefaultStore);

                IModel agents;

                if (!store.ContainsModel(Models.Agents))
                {
                    agents = store.CreateModel(Models.Agents);
                }
                else
                {
                    agents = store.GetModel(Models.Agents);
                }
                    
                InstallAgent(agents, "application://inkscape.desktop/", "Inkscape", "inkscape", "#EE204E", true);
                InstallAgent(agents, "application://krita.desktop/", "Krita", "krita", "#926EAE", true);
                InstallAgent(agents, "application://chromium-browser.desktop/", "Chromium", "chromium-browser", "#1F75FE");
                InstallAgent(agents, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");

                IModel activities;

                if (!store.ContainsModel(Models.Activities))
                {
                    activities = store.CreateModel(Models.Activities);
                }
                else
                {
                    activities = store.GetModel(Models.Activities);
                }

                IModel webActivities;

                if (!store.ContainsModel(Models.WebActivities))
                {
                    webActivities = store.CreateModel(Models.WebActivities);
                }
                else
                {
                    webActivities = store.GetModel(Models.WebActivities);
                }

                InstallMonitoring();

                IModel monitoring = store.GetModel(Models.Monitoring);

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

        public static void InstallAgent(IModel model, string uri, string name, string executableName, string colour, bool captureEnabled = false)
        {
            Console.WriteLine("Installing agent {0}..", name);

            UriRef agentUri = new UriRef(uri);

            if (!model.ContainsResource(agentUri))
            {
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

                if(!agent.HasProperty(rdf.type, prov.SoftwareAgent))
                {
                    agent.AddProperty(rdf.type, prov.SoftwareAgent);

                    modified = true;
                }

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
                    agent.Commit();
                }
            }
        }

        public static void InstallMonitoring(IStore store = null)
        {
            if(store == null)
            {
                store = StoreFactory.CreateStoreFromConfiguration(Models.DefaultStore);
            }

            IModel model;

            if (!store.ContainsModel(Models.Monitoring))
            {
                model = store.CreateModel(Models.Monitoring);
            }
            else
            {
                model = store.GetModel(Models.Monitoring);
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

                if (store.ContainsModel(Models.Agents))
                {
                    store.RemoveModel(Models.Agents);
                }

                if (store.ContainsModel(Models.Activities))
                {
                    store.RemoveModel(Models.Activities);
                }

                if (store.ContainsModel(Models.WebActivities))
                {
                    store.RemoveModel(Models.WebActivities);
                }

                if (store.ContainsModel(Models.Monitoring))
                {
                    store.RemoveModel(Models.Monitoring);
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
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            if (!store.ContainsModel(Models.Agents))
            {
                return false;
            }

            IModel agents = store.GetModel(Models.Agents);

            Person user = agents.GetResources<Person>().FirstOrDefault();

            return user != null && user.EmailAddress != null;
        }

        #endregion
    }
}

