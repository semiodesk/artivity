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
//
using AppKit;
using System.IO;
using System;
using System.Diagnostics;


namespace Artivity.Mac.Journal
{
    public class Program
    {
        public Program()
        {
        }

        public void Run(string[] args)
        {
            TestApid();
            NSApplication.Init();
            NSApplication.Main(args);
        }

        static void TestApid()
        {
            var uname = Environment.UserName;
            FileInfo globalAgent = new FileInfo("/Library/LaunchAgents/com.semiodesk.artivity.plist");

            if (globalAgent.Exists)
            {
                // The plist for a global agent exists, so we assume everything is fine and people know what they are doing...
                return;
            }

            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FileInfo userAgent = new FileInfo(Path.Combine(home, "Library/LaunchAgents/com.semiodesk.artivity.plist"));
            if (userAgent.Exists) // TODO: test if newer
            {
                // The plist for a local agent exists, so we assume everything is fine and people know what they are doing...
                return;
            }

            var current = Environment.CurrentDirectory;
            FileInfo agentFile = new FileInfo(Path.Combine(current,"..", "Resources", "com.semiodesk.artivity.plist"));

            var text = File.ReadAllText(agentFile.FullName);
            DirectoryInfo contentPath = new DirectoryInfo(Path.Combine(current, ".."));
            File.WriteAllText(userAgent.FullName, string.Format(text, contentPath.FullName));

            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = string.Format("-c \"launchctl bootstrap gui/$UID {0}\"", userAgent);
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
        }
    }
}

