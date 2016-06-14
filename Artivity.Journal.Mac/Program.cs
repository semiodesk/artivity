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

using AppKit;
using System.IO;
using System;
using System.Reflection;
using System.Diagnostics;
using ObjCRuntime;

namespace Artivity.Journal.Mac
{
    public class Program
    {
        #region Methods

        public void Run(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }

        public void InitSparkle()
        {
            var baseAppPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString());

            var sparklePath = baseAppPath + "/Frameworks/Sparkle.Framework/Sparkle";

            if (Dlfcn.dlopen(sparklePath, 0) == IntPtr.Zero)
            {
                Console.Error.WriteLine(string.Format("Unable to load the dynamic library for Sparkle: {0}", sparklePath));

                Environment.Exit(1);
            }
        }

        public void InitApid()
        {
#if !DEBUG
            TestApid();

            if (!IsApidRunning())
            {
                StartApid();
            }
#endif
        }

        private void TestApid()
        {
            Console.WriteLine("Testing if APID is installed..");

            string username = Environment.UserName;

            string globalApidPath = "/Library/LaunchAgents/com.semiodesk.artivity.plist";

            FileInfo globalAgent = new FileInfo(globalApidPath);

            if (globalAgent.Exists)
            {
                Console.WriteLine(string.Format("APID LaunchAgent found in {0}", globalApidPath));

                // The plist for a global agent exists, so we assume everything is fine and people know what they are doing...
                return;
            }

            FileInfo userAgent = GetLocalPlist();

            if (userAgent.Exists) // TODO: test if newer
            {
                Console.WriteLine(string.Format("APID launch agent found in {0}", userAgent.FullName));
            }
            else
            {
                Console.WriteLine(string.Format("No APID launch agent found in {0}", userAgent.FullName));

                var current = Environment.CurrentDirectory;

                FileInfo agentFile = new FileInfo(Path.Combine(current, "..", "Resources", "com.semiodesk.artivity.plist"));

                var text = File.ReadAllText(agentFile.FullName);

                DirectoryInfo contentPath = new DirectoryInfo(Path.Combine(current, ".."));

                Console.WriteLine(string.Format("Creating launch agent for APID in {0}", userAgent.FullName));

                File.WriteAllText(userAgent.FullName, string.Format(text, contentPath.FullName));

                Process process = new Process();
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = string.Format("-c \"launchctl bootstrap gui/$UID {0}\"", userAgent.FullName);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                Console.WriteLine(string.Format("Started APID: {0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments));
            }
        }

        public static bool IsApidRunning()
        {
            //launchctl list | grep 'com.semiodesk.artivity$' | awk '{print $2}'
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
                    Console.WriteLine(string.Format("Found running APID."));

                    return true;
                }
            }

            Console.WriteLine(string.Format("No running APID found."));

            return false;
        }

        private string GetCurrentExecutingDirectory()
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

            return Path.GetDirectoryName(filePath);
        }

        private static FileInfo GetLocalPlist()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return new FileInfo(Path.Combine(home, "Library/LaunchAgents/com.semiodesk.artivity.plist"));
        }

        public static void StopApid()
        {
            Console.WriteLine(string.Format("Stopping APID.."));

            FileInfo userAgent = GetLocalPlist();

            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl unload {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }

        public static void StartApid()
        {
            Console.WriteLine(string.Format("Starting APID.."));

            FileInfo userAgent = GetLocalPlist();

            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl load {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }

        #endregion
    }
}

