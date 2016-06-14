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


using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Plugin
{
    public abstract class PluginChecker
    {
        #region Members
        List<PluginManifest> _manifests = new List<PluginManifest>();
        List<string> _errorManifests = new List<string>();
        public List<SoftwareAgentPlugin> Plugins { get; private set;}

        private static log4net.ILog _logger;
        private static log4net.ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    Type type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
                    _logger = log4net.LogManager.GetLogger(type);
                }
                return _logger;
            }
        }

        private DirectoryInfo _pluginDir;
        #endregion

        #region Constructor
        public PluginChecker(DirectoryInfo pluginDir)
        {
            _pluginDir = pluginDir;
            Plugins = new List<SoftwareAgentPlugin>();
            LoadManifests();
        }
        #endregion

        #region Methods

        private void LoadManifests()
        {
            if (_pluginDir == null || !_pluginDir.Exists)
            {
                Logger.ErrorFormat("Could not find plugin directory \"{0}\"", _pluginDir);
                return;
            }

            foreach (var plugin in _pluginDir.EnumerateDirectories())
            {

                var manifest = PluginManifestReader.ReadManifest(plugin);
                if (manifest != null)
                {
                    _manifests.Add(manifest);
                    Plugins.Add(new SoftwareAgentPlugin(manifest));
                }
                else
                {
                    _errorManifests.Add(plugin.FullName);
                    Logger.ErrorFormat("Directory {0} did not contain valid manifest.", plugin.FullName);
                }
            }
        }

        public void Check(bool autoInstall = false)
        {
            foreach (var p in Plugins)
            {
                bool? res = IsPluginInstalled(p);
                if (res.HasValue)
                {
                    p.IsInstalled = res.Value;
                    p.IsSoftwareInstalled = true;

                    if (!p.IsInstalled && autoInstall)
                        InstallPlugin(p.Manifest);
                }
                else
                {
                    p.IsSoftwareInstalled = false;
                }
            }
        }

        protected abstract DirectoryInfo GetApplicationLocation (PluginManifest manifest);
        protected abstract void CreateLink (string target, string source);
        protected abstract string GetApplicationVersion(FileSystemInfo app);

        public bool? IsPluginInstalled(SoftwareAgentPlugin plugin)
        {
            return IsPluginInstalled(plugin.Manifest);
        }

        public bool? IsPluginInstalled (PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation (manifest);
            if(location != null && location.Exists)
            {
                FileSystemInfo info;
                if (!string.IsNullOrEmpty(manifest.ExecPath))
                    info = new FileInfo(Path.Combine(location.FullName, manifest.ExecPath));
                else
                    info = location;
                var version = GetApplicationVersion(info);
                if (version != manifest.HostVersion)
                    return null;

                bool exists = true;
                foreach (var pluginFile in manifest.PluginFile)
                {
                    var pluginPath = Path.Combine(location.FullName, manifest.TargetPath, pluginFile.GetName());
                    exists = exists && ( File.Exists(pluginPath) || Directory.Exists(pluginPath) );
                }
                return exists;

            }
            return null;
        }

        public bool InstallPlugin(SoftwareAgentPlugin plugin)
        {
            return InstallPlugin(plugin.Manifest);
        }

        public bool InstallPlugin(PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation (manifest);
            if (location != null && location.Exists)
            {
                foreach (var pluginFile in manifest.PluginFile)
                {
                    var pluginPath = Path.Combine(location.FullName, manifest.TargetPath, pluginFile.GetName());
                    string source = pluginFile.GetPluginSource(manifest);
                    if (File.Exists(source) || Directory.Exists(source))
                    {
                        if (pluginFile.Link)
                        {
                            CreateLink(pluginPath, source);
                        }
                        else
                        {
                            File.Copy(source, pluginPath);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool InstallPlugin(string uri)
        {
            var p = Plugins.First((x) => x.Uri == uri);
            if (p != null)
                return InstallPlugin(p);
            return false;
        }
        #endregion
    }

    public class PluginCheckerFactory
    {
        public static PluginChecker CreatePluginChecker(DirectoryInfo dir)
        {
            #if WIN
            return new Artivity.Api.Plugin.Win.WinPluginChecker(dir);
            #elif OSX
            return new Artivity.Api.Plugin.OSX.OsxPluginChecker(dir);
            #endif
        }
    }
}
