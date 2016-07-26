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

using ObjCRuntime;
using AppKit;
using System.IO;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Xml.XPath;
using System.Net;
using log4net.Layout;
using log4net.Appender;
using System.Xml;

namespace Artivity.Journal.Mac
{
    public class Program
    {
        #region Members

        private bool _logInitialized;

        #endregion

        #region Constructors

        public Program()
        {
            InitializeLogging();

            Logger.LogInfo("--- Artivity Journal, Version 1.5.0 ---");
        }

        #endregion

        #region Methods

        public void Run(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }

        protected void InitializeLogging()
        {
            if (_logInitialized)
            {
                return;
            }

            bool consoleLogging = true;

#if DEBUG
            string logFile = Path.Combine(Environment.CurrentDirectory, "log.config.debug");
#else
            string logFile = Path.Combine(Environment.CurrentDirectory, "log.config");
#endif

            if (File.Exists(logFile))
            {
                try
                {
                    FileInfo logFileConfig = new FileInfo(logFile);

                    log4net.Config.XmlConfigurator.Configure(logFileConfig);

                    consoleLogging = false;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
            }

            if (consoleLogging)
            {
                PatternLayout layout = new PatternLayout();
                layout.ConversionPattern = layout.ConversionPattern = "%date{g} [%-5level] – %message%newline";
                layout.ActivateOptions();

                ConsoleAppender appender = new ConsoleAppender();
                appender.Name = "ConsoleAppender";
                appender.Layout = layout;

                log4net.Config.BasicConfigurator.Configure(appender);
            }

            _logInitialized = true;
        }

        public void InitializeSparkle()
        {
            Logger.LogInfo("Initializing Sparkle..");

            DirectoryInfo appBundleFolder = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString());

            string sparklePath = appBundleFolder + "/Frameworks/Sparkle.Framework/Sparkle";

            if (Dlfcn.dlopen(sparklePath, 0) == IntPtr.Zero)
            {
                Logger.LogError("Unable to load the dynamic library for Sparkle: {0}", sparklePath);

                Environment.Exit(1);
            }
        }

        public void InitializeApid()
        {
#if !DEBUG
            CheckApid();

            if (!IsApidRunning())
            {
                StartApid();
            }
#endif
        }

        private void CheckApid()
        {
            try
            {
                Logger.LogInfo("Initializing APID..");

                if (!CheckSystemApid())
                {
                    CheckUserApid();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public bool CheckSystemApid()
        {
            // 1. Check for a globally installed APID.
            string path = "/Library/LaunchAgents/com.semiodesk.artivity.plist";

            FileInfo agent = new FileInfo(path);

            if (agent.Exists)
            {
                Logger.LogInfo("Global launch agent found in {0}", path);

                // The plist for a global agent exists, so we assume everything is fine and people know what they are doing...
                return true;
            }

            return false;
        }

        public bool CheckUserApid()
        {
            // 2. Check for a user installed APID.
            FileInfo plist = GetUserAgentPlist();

            // Ensure that the $HOME/Library/LaunchAgents directory exists..
            EnsureDirectoryExists(plist.Directory);

            DirectoryInfo appBundle = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, ".."));

            if (plist.Exists)
            {
                // Test if current plist file contains this apid
                string currentApid = GetAgentPath();
                string thisApid = appBundle.FullName;

                int sameApid = string.Compare(thisApid, 0, currentApid, 0, thisApid.Length);

                if (sameApid == 0)
                {
                    Logger.LogInfo(string.Format("Launch agent found in {0}", plist.FullName));

                    return true;
                }
            }

            Process process;

            if (!plist.Exists)
            {
                Logger.LogInfo("Creating launch agent in {0}", plist.FullName);
            }
            else
            {
                Logger.LogInfo("Replacing launch agent in {0}", plist.FullName);

                process = new Process();
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = string.Format("-c \"launchctl unload {0}\"", plist.FullName);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                Logger.LogInfo("Unloaded launch agent: {0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);
            }

            FileInfo templateFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, "..", "Resources", "com.semiodesk.artivity.plist"));

            string template = File.ReadAllText(templateFile.FullName);

            File.WriteAllText(plist.FullName, string.Format(template, appBundle.FullName));

            process = new Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = string.Format("-c \"launchctl bootstrap gui/$UID {0}\"", plist.FullName);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            Logger.LogInfo("Bootstrapped launch agent: {0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

            return true;
        }

        public static bool IsApidRunning()
        {
            Process process = new Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = "-c \"launchctl list | grep 'com.semiodesk.artivity$' | awk '{print $2}'\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();

                if (line == "0")
                {
                    Logger.LogInfo("Running APID reported by launchctl..");

                    return true;
                }
            }

            Logger.LogInfo("No running APID reported by launchctl..");

            return false;
        }

        protected static void EnsureDirectoryExists(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();

                Logger.LogInfo("Created directory: {0}", directory.FullName);
            }
        }

        private string GetCurrentExecutingDirectory()
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

            return Path.GetDirectoryName(filePath);
        }

        private string GetAgentPath()
        {
            using (TextReader textReader = File.OpenText(GetUserAgentPlist().FullName))
            {
                // See: http://todotnet.com/archive/2006/07/27/8248.aspx
                // Prevent the XPathDocument from issueing a HTTP-Request to
                // resolve the document DTD. This causes exceptions if there is no
                // network connection.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Ignore;

                XmlReader reader = XmlReader.Create(textReader, settings);

                XPathDocument document = new XPathDocument(reader);

                XPathNavigator navigator = document.CreateNavigator();

                string path = "//plist/dict/array/string";

                return navigator.SelectSingleNode(path).Value;
            }
        }

        private static FileInfo GetUserAgentPlist()
        {
            string userHome = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return new FileInfo(Path.Combine(userHome, "Library/LaunchAgents/com.semiodesk.artivity.plist"));
        }

        public static void StopApid()
        {
            Logger.LogInfo(string.Format("Unloading APID with launchctl.."));

            FileInfo userAgent = GetUserAgentPlist();

            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl unload {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }

        public static void StartApid()
        {
            Logger.LogInfo(string.Format("Loading APID with launchctl.."));

            FileInfo plist = GetUserAgentPlist();

            EnsureDirectoryExists(plist.Directory);

            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl load {0}\"", plist.FullName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }

        public static bool IsApidAvailable(string port)
        {
            try
            {
                WebClient client = new WebClient();

                client.DownloadString(string.Format("http://127.0.0.1:{0}/artivity/app/journal/1.0/", port));

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        #endregion
    }
}

