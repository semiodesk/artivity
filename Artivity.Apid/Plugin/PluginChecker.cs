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

using log4net;
using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace Artivity.Api.Plugin
{
    public abstract class PluginChecker
    {
        #region Members

        List<PluginManifest> _manifests = new List<PluginManifest>();

        List<string> _errorManifests = new List<string>();

        public List<SoftwareAgentPlugin> Plugins { get; private set;}

        private static ILog _logger;

        protected static ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    Type type = MethodBase.GetCurrentMethod().DeclaringType;

                    _logger = LogManager.GetLogger(type);
                }

                return _logger;
            }
        }

        private IModelProvider _modelProvider;

        private DirectoryInfo _pluginDir;

        #endregion

        #region Constructors

        public PluginChecker(IModelProvider modelProvider, DirectoryInfo pluginDir)
        {
            _modelProvider = modelProvider;
            _pluginDir = pluginDir;

            Plugins = new List<SoftwareAgentPlugin>();

            LoadManifests();
            LoadLoggingStatus();
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

        private void LoadLoggingStatus()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?agentUri ?loggingEnabled
                WHERE
                {
                    ?agentUri a art:SoftwareAgent .
                    ?agentUri art:isCaptureEnabled ?loggingEnabled .
                }
            ");

            List<BindingSet> bindings = _modelProvider.GetAgents().GetBindings(query).ToList();

            Dictionary<Uri, bool> agents = new Dictionary<Uri, bool>();

            foreach(BindingSet b in bindings)
            {
                Uri uri = new Uri(b["agentUri"].ToString());
                bool enabled = (bool)b["loggingEnabled"];

                agents[uri] = enabled;
            }

            foreach(SoftwareAgentPlugin plugin in Plugins)
            {
                if(agents.ContainsKey(plugin.AgentUri))
                {
                    plugin.IsPluginEnabled = agents[plugin.AgentUri];
                }
            }
        }

        public void CheckPlugins(bool installPlugins = false)
        {
            foreach (SoftwareAgentPlugin plugin in Plugins)
            {
                plugin.IsSoftwareInstalled = GetApplicationLocation(plugin.Manifest) != null;

                if (plugin.IsSoftwareInstalled)
                {
                    plugin.IsPluginInstalled = IsPluginInstalled(plugin);

                    if (!plugin.IsPluginInstalled && installPlugins)
                    {
                        plugin.IsPluginInstalled = InstallPlugin(plugin.Manifest);
                        plugin.IsPluginEnabled = plugin.IsPluginInstalled;
                    }
                }
                else
                {
                    plugin.IsPluginInstalled = false;
                    plugin.IsPluginEnabled = false;
                }
            }
        }

        public bool IsPluginInstalled(PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation(manifest);

            if (location != null && location.Exists)
            {
                FileSystemInfo info;

                if (!string.IsNullOrEmpty(manifest.ExecPath))
                {
                    info = new FileInfo(Path.Combine(location.FullName, manifest.ExecPath));
                }
                else
                {
                    info = location;
                }

                var version = GetApplicationVersion(info);

                if (!version.StartsWith(manifest.HostVersion))
                {
                    return false;
                }

                bool installed = true;

                foreach (var pluginFile in manifest.PluginFile)
                {
                    var pluginPath = Path.Combine(location.FullName, manifest.TargetPath, pluginFile.GetName());

                    installed = installed && (File.Exists(pluginPath) || Directory.Exists(pluginPath));
                }

#if WIN
                bool is64Bit = Environment.Is64BitOperatingSystem;

                foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
                {
                    if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                    {
                        continue;
                    }

                    if(!HasRegistryKey(key))
                    {
                        return false;
                    }
                }
#endif

                return installed;
            }

            return false;
        }

        public bool IsPluginInstalled(SoftwareAgentPlugin plugin)
        {
            return IsPluginInstalled(plugin.Manifest);
        }

        public bool InstallPlugin(PluginManifest manifest)
        {
            try
            {
                DirectoryInfo location = GetApplicationLocation(manifest);

                if (location != null && location.Exists)
                {
                    foreach (PluginManifestPluginFile file in manifest.PluginFile)
                    {
                        var path = Path.Combine(location.FullName, manifest.TargetPath, file.GetName());

                        string source = file.GetPluginSource(manifest);

                        if (File.Exists(source) || Directory.Exists(source))
                        {
                            if (file.Link)
                            {
                                CreateLink(path, source);
                            }
                            else
                            {
                                File.Copy(source, path);
                            }
                        }
                    }

#if WIN
                    bool is64Bit = Environment.Is64BitOperatingSystem;

                    foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
                    {
                        if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                        {
                            continue;
                        }

                        if(!CreateRegistryKey(key))
                        {
                            return false;
                        }
                    }
#endif

                    return true;
                }
            }
            catch(Exception e)
            {
                Logger.ErrorFormat("Failed to install plugin {0}: {1}", manifest.Uri, e);

                throw e;
            }

            return false;
        }

        public bool InstallPlugin(Uri uri)
        {
            SoftwareAgentPlugin p = Plugins.First((x) => x.AgentUri == uri);

            return p != null ? InstallPlugin(p.Manifest) : false;
        }

        public void InstallAgent(IModel model, SoftwareAgentPlugin plugin, bool pluginEnabled = false)
        {
            if (!model.ContainsResource(plugin.AgentUri))
            {
                Logger.InfoFormat("Installing agent {0}", plugin.AgentUri);

                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(plugin.AgentUri);
                agent.Name = plugin.AgentName;
                agent.ColourCode = plugin.AgentColor;
                agent.IsCaptureEnabled = pluginEnabled;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(plugin.AgentUri);

                if (agent.Name != plugin.AgentName)
                {
                    agent.Name = plugin.AgentName;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = plugin.AgentColor;

                    modified = true;
                }

                if (modified)
                {
                    Logger.InfoFormat("Updating agent {0}", plugin.AgentUri);

                    agent.Commit();
                }
            }

            if (!model.ContainsResource(plugin.AssociationUri))
            {
                SoftwareAssociation association = model.CreateResource<SoftwareAssociation>(plugin.AssociationUri);
                association.Agent = new SoftwareAgent(plugin.AgentUri);
                association.Role = new Role(new UriRef(ART.SOFTWARE));
                association.ExecutableVersion = plugin.ExecutableVersion;
                association.ExecutablePath = plugin.ExecutablePath;
                association.Commit();
            }
        }

        public bool UninstallPlugin(Uri uri)
        {
            SoftwareAgentPlugin p = Plugins.First((x) => x.AgentUri == uri);

            return p != null ? UninstallPlugin(p.Manifest) : false;
        }

        public bool UninstallPlugin(PluginManifest manifest)
        {
            try
            {
                DirectoryInfo location = GetApplicationLocation(manifest);

                if (location != null && location.Exists)
                {
                    foreach (PluginManifestPluginFile file in manifest.PluginFile)
                    {
                        var path = Path.Combine(location.FullName, manifest.TargetPath, file.GetName());

                        string source = file.GetPluginSource(manifest);

                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }

#if WIN
                    bool is64Bit = Environment.Is64BitOperatingSystem;

                    foreach (PluginManifestRegistryKey key in manifest.RegistryKeys)
                    {
                        if (is64Bit && key.Platform == "Win32" || !is64Bit && key.Platform == "Win64")
                        {
                            continue;
                        }

                        DeleteRegistryKey(key);
                    }
#endif

                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to uninstall plugin {0}: {1}", manifest.Uri, e);

                throw e;
            }

            return false;
        }

        public void UninstallAgent(IModel model, SoftwareAgentPlugin plugin)
        {
            if (model.ContainsResource(plugin.AssociationUri))
            {
                Logger.InfoFormat("Uninstalling agent association {0}", plugin.AgentUri);

                model.DeleteResource(plugin.AssociationUri);
            }

            if (model.ContainsResource(plugin.AgentUri))
            {
                Logger.InfoFormat("Uninstalling agent {0}", plugin.AgentUri);

                model.DeleteResource(plugin.AgentUri);
            }
        }

        protected abstract DirectoryInfo GetApplicationLocation(PluginManifest manifest);

        protected abstract string GetApplicationVersion(FileSystemInfo app);

        protected abstract void CreateLink(string target, string source);

        protected virtual bool HasRegistryKey(PluginManifestRegistryKey key)
        {
            return false;
        }

        protected virtual bool CreateRegistryKey(PluginManifestRegistryKey key)
        {
            return false;
        }

        protected virtual bool DeleteRegistryKey(PluginManifestRegistryKey key)
        {
            return false;
        }

        #endregion
    }
}
