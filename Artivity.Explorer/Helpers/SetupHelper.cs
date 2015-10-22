using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ArtivityExplorer
{
    public class SetupHelper
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
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return Path.Combine(appData, _appDataFolderName);
        }

        public static bool CheckEnvironment()
        {
            return HasApiDaemonAutostart();
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
                throw new PlatformNotSupportedException();
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

        #endregion
    }
}

