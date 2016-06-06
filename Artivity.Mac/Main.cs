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
using Artivity.Mac;
using Artivity.Apid;
using System.IO;
using ObjCRuntime;
using System.Reflection;
using System.Diagnostics;


namespace Artivity.Journal.Mac
{
    static class MainClass
    {
        static bool _forceApid = false;


        static string GetCurrentExecutingDirectory()
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }

        static void Main(string[] args)
        {
            #if APID
            _forceApid = true;
            #endif

            Options opts = new Options();
            CommandLine.Parser.Default.ParseArguments(args, opts);
            if (opts.Daemon || _forceApid)
            {
                Artivity.Mac.Apid.Program prog = new Artivity.Mac.Apid.Program();
                prog.Run(opts);
       
            } else
            {
                
                #if !DEBUG
                TestApidInstalled();
                if (!TestApidRunning())
                    StartApid();
                #endif

                InitSparkle();
                Artivity.Mac.Journal.Program prog = new Artivity.Mac.Journal.Program();
                prog.Run(args);
            }

        }

        static void InitSparkle()
        {
            var baseAppPath = Directory.GetParent(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString());
            var sparklePath = baseAppPath + "/Frameworks/Sparkle.Framework/Sparkle";
            if( Dlfcn.dlopen(sparklePath, 0) == IntPtr.Zero )
            {
                Console.Error.WriteLine(string.Format("Unable to load the dynamic library for Sparkle.\nTried {0}", sparklePath));
                Environment.Exit(1);
            }
        }

        static void TestApidInstalled()
        {
            Logger.LogInfo("Testing if APID is installed.");
            var uname = Environment.UserName;
            string globalApidPath = "/Library/LaunchAgents/com.semiodesk.artivity.plist";
            FileInfo globalAgent = new FileInfo(globalApidPath);

            if (globalAgent.Exists)
            {
                Logger.LogInfo(string.Format("Found Apid LaunchAgent in {0}", globalApidPath));
                // The plist for a global agent exists, so we assume everything is fine and people know what they are doing...
                return;
            }

           
            FileInfo userAgent = GetLocalPlist();
            if (userAgent.Exists) // TODO: test if newer
            {
                Logger.LogInfo(string.Format("Found Apid LaunchAgent in {0}", userAgent.FullName));
                // The plist for a local agent exists, so we assume everything is fine
                return;
            }
            Logger.LogInfo(string.Format("Did not find Apid LaunchAgent in {0}", userAgent.FullName));

            var current = Environment.CurrentDirectory;
            FileInfo agentFile = new FileInfo(Path.Combine(current,"..", "Resources", "com.semiodesk.artivity.plist"));

            var text = File.ReadAllText(agentFile.FullName);
            DirectoryInfo contentPath = new DirectoryInfo(Path.Combine(current, ".."));
            Logger.LogInfo(string.Format("Creating Apid LaunchAgent in {0}", userAgent.FullName));
            File.WriteAllText(userAgent.FullName, string.Format(text, contentPath.FullName));

            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl bootstrap gui/$UID {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            Logger.LogInfo(string.Format("Starting APID with {0} {1}", proc.StartInfo.FileName, proc.StartInfo.Arguments));
            proc.Start();
        }

        public static bool TestApidRunning()
        {
            Logger.LogInfo(string.Format("Testing if APID is running."));
            //launchctl list | grep 'com.semiodesk.artivity$' | awk '{print $2}'
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \"launchctl list | grep 'com.semiodesk.artivity$' | awk '{print $2}'\"";
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                if (line == "0")
                {
                    Logger.LogInfo(string.Format("APID is running."));
                    return true;
                }
            }
            Logger.LogInfo(string.Format("APID is not running."));
            return false;
        }

        static FileInfo GetLocalPlist()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return new FileInfo(Path.Combine(home, "Library/LaunchAgents/com.semiodesk.artivity.plist"));
        }

        public static void StopApid()
        {
            Logger.LogInfo(string.Format("Stopping APID."));
            FileInfo userAgent = GetLocalPlist();
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl unload {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }

        public static void StartApid()
        {
            Logger.LogInfo(string.Format("Starting APID."));
            FileInfo userAgent = GetLocalPlist();
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl load {0}\"", userAgent.FullName);
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }
    }


}
