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

using log4net;
using Artivity.DataModel;
using Artivity.Apid.Platforms;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Artivity.Apid.Plugin
{
    public abstract class PluginChecker
    {
        #region Members

        protected IPlatformProvider PlatformProvider;

        protected IModelProvider ModelProvider;

        private List<PluginManifest> _manifests = new List<PluginManifest>();

        private List<string> _manifestErrors = new List<string>();

        public List<SoftwareAgentPlugin> Plugins { get; private set;}

        protected DirectoryInfo PluginDirectory;

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

        #endregion

        #region Constructors

        public PluginChecker(IPlatformProvider platformProvider, IModelProvider modelProvider, DirectoryInfo pluginDir)
        {
            PlatformProvider = platformProvider;
            ModelProvider = modelProvider;

            Plugins = new List<SoftwareAgentPlugin>();
            PluginDirectory = pluginDir;

            IModel agents = ModelProvider.GetAgents();

            LoadManifests(agents);
            LoadLoggingStatus(agents);
        }

        #endregion

        #region Methods

        private void LoadManifests(IModel model)
        {
            if (PluginDirectory == null || !PluginDirectory.Exists)
            {
                Logger.ErrorFormat("Could not find plugin directory \"{0}\"", PluginDirectory);

                return;
            }

            foreach (var plugin in PluginDirectory.EnumerateDirectories())
            {
                PluginManifest manifest = PluginManifestReader.ReadManifest(plugin);

                if (manifest != null)
                {
                    string msg = string.Format("Registered plugin for agent <{0}>", manifest.AgentUri);

                    if (!string.IsNullOrEmpty(manifest.ExecutableVersion))
                    {
                        msg += " ; version " + manifest.ExecutableVersion;
                    }

                    if (!string.IsNullOrEmpty(manifest.MinExecutableVersion))
                    {
                        msg += " ; min version " + manifest.MinExecutableVersion;
                    }

                    if (!string.IsNullOrEmpty(manifest.MaxExecutableVersion))
                    {
                        msg += " ; max version " + manifest.MaxExecutableVersion;
                    }

                    Logger.Info(msg);

                    _manifests.Add(manifest);

                    SoftwareAgentPlugin p = new SoftwareAgentPlugin(manifest);

                    Plugins.Add(p);
                }
                else
                {
                    _manifestErrors.Add(plugin.FullName);

                    Logger.ErrorFormat("Directory {0} did not contain valid manifest.", plugin.FullName);
                }
            }
        }

        private void LoadLoggingStatus(IModel model)
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

            List<BindingSet> bindings = model.GetBindings(query).ToList();

            Dictionary<Uri, bool> agents = new Dictionary<Uri, bool>();

            foreach(BindingSet b in bindings)
            {
                Uri uri = new Uri(b["agentUri"].ToString());
                bool enabled = (bool)b["loggingEnabled"];

                agents[uri] = enabled;
            }

            foreach(SoftwareAgentPlugin plugin in Plugins)
            {
                UriRef agentUri = new UriRef(plugin.Manifest.AgentUri);

                if(agents.ContainsKey(agentUri))
                {
                    plugin.IsPluginEnabled = agents[agentUri];
                }
            }
        }

        public void CheckPlugins(bool installPlugins = false)
        {
            IModel model = ModelProvider.GetAgents();

            foreach (SoftwareAgentPlugin plugin in Plugins)
            {
                try
                {
                    plugin.DetectedVersions = TryGetInstalledSoftwareVersions(plugin.Manifest).ToArray();
                    plugin.IsSoftwareInstalled = plugin.DetectedVersions.Any();

                    if (plugin.IsSoftwareInstalled)
                    {
                        plugin.IsPluginInstalled = IsPluginInstalled(plugin.Manifest);

                        if (plugin.IsPluginInstalled)
                        {
                            foreach (UriRef uri in plugin.GetAssociationUris())
                            {
                                Logger.InfoFormat("Software and plugin installed: <{0}>", uri);
                            }
                        }
                        else
                        {
                            foreach (UriRef uri in plugin.GetAssociationUris())
                            {
                                Logger.InfoFormat("Software installed: <{0}>", uri);
                            }
                        }

                        if (!plugin.IsPluginInstalled && installPlugins)
                        {
                            plugin.IsPluginInstalled = InstallPlugin(plugin);
                            plugin.IsPluginEnabled = plugin.IsPluginInstalled;
                        }

                        if (!HasAgentAssociations(model, plugin))
                        {
                            InstallAgentAssociations(model, plugin);
                        }
                    }
                    else
                    {
                        foreach (UriRef uri in plugin.GetAssociationUris())
                        {
                            Logger.InfoFormat("Software not installed: <{0}>", uri);
                        }

                        plugin.IsPluginInstalled = false;
                        plugin.IsPluginEnabled = false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        protected abstract IEnumerable<string> TryGetInstalledSoftwareVersions(PluginManifest manifest);

        public bool IsSoftwareInstalled(PluginManifest manifest)
        {
            return TryGetInstalledSoftwareVersions(manifest).Any();
        }

        public abstract bool IsPluginInstalled(PluginManifest manifest);

        protected abstract string GetApplicationVersion(FileSystemInfo app);

        protected abstract IEnumerable<DirectoryInfo> GetApplicationLocations(PluginManifest manifest);

        public bool InstallPlugin(Uri uri)
        {
            SoftwareAgentPlugin plugin = Plugins.First((p) => p.Manifest.AgentUri == uri.AbsoluteUri && IsSoftwareInstalled(p.Manifest));

            return plugin != null ? InstallPlugin(plugin) : false;
        }

        public virtual bool InstallPlugin(SoftwareAgentPlugin plugin)
        {
            PluginManifest manifest = plugin.Manifest;

            try
            {
                foreach (DirectoryInfo location in GetApplicationLocations(manifest).Where(l => l.Exists))
                {
                    foreach (PluginManifestPluginFile pluginFile in manifest.PluginFile)
                    {
                        // This may also be an app bundle / directory on macOS.
                        string sourcePath = pluginFile.GetPluginSource(manifest);

                        if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
                        {
                            throw new FileNotFoundException(sourcePath);
                        }

                        DirectoryInfo targetFolder = manifest.GetPluginTargetDirectory(location);

                        if (!targetFolder.Exists)
                        {
                            return false;
                        }

                        var targetPath = Path.Combine(targetFolder.FullName, pluginFile.GetName());

                        if (pluginFile.Link)
                        {
                            Logger.InfoFormat("Linking plugin: {0}", targetPath);

                            CreateLink(sourcePath, targetPath);
                        }
                        else
                        {
                            Logger.InfoFormat("Copying plugin: {0}", targetPath);

                            if (File.Exists(sourcePath))
                            {
                                File.Copy(sourcePath, targetPath);
                            }
                            else if (Directory.Exists(sourcePath))
                            {
                                CopyDirectory(sourcePath, targetPath, true);
                            }
                        }
                    }

                    IModel model = ModelProvider.GetAgents();

                    if (!HasAgentAssociations(model, plugin))
                    {
                        InstallAgent(model, plugin);
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Logger.ErrorFormat("Failed to install plugin {0}: {1}", manifest.AgentUri, e);

                throw e;
            }
        }

        private bool HasAgentAssociations(IModel model, SoftwareAgentPlugin plugin)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("ASK WHERE {");

            foreach (UriRef uri in plugin.GetAssociationUris())
            {
                queryBuilder.AppendFormat(" <{0}> prov:agent <{1}> .", uri, plugin.Manifest.AgentUri);
            }

            queryBuilder.Append(" }");

            ISparqlQuery query = new SparqlQuery(queryBuilder.ToString());
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return result.GetAnwser();
        }

        private void InstallAgent(IModel model, SoftwareAgentPlugin plugin, bool pluginEnabled = false)
        {
            UriRef agentUri = new UriRef(plugin.Manifest.AgentUri);

            if (!model.ContainsResource(agentUri))
            {
                Logger.InfoFormat("Installing agent {0}", agentUri);

                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(agentUri);
                agent.Name = plugin.Manifest.DisplayName;
                agent.ColourCode = plugin.Manifest.DefaultColor;
                agent.IsCaptureEnabled = pluginEnabled;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(agentUri);

                if (agent.Name != plugin.Manifest.DisplayName)
                {
                    agent.Name = plugin.Manifest.DisplayName;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = plugin.Manifest.DefaultColor;

                    modified = true;
                }

                if (modified)
                {
                    Logger.InfoFormat("Updating agent {0}", plugin.Manifest.AgentUri);

                    agent.Commit();
                }
            }
        }

        private void InstallAgentAssociations(IModel model, SoftwareAgentPlugin plugin)
        {
            foreach (UriRef uri in plugin.GetAssociationUris())
            {
                if (!model.ContainsResource(uri))
                {
                    Logger.InfoFormat("Installing agent association <{0}>", uri);

                    SoftwareAssociation association = model.CreateResource<SoftwareAssociation>(uri);
                    association.Agent = new SoftwareAgent(new UriRef(plugin.Manifest.AgentUri));
                    association.Role = new Role(new UriRef(ART.SOFTWARE));
                    association.ExecutableVersion = plugin.Manifest.ExecutableVersion;
                    association.ExecutablePath = plugin.Manifest.ExecutablePath;
                    association.Commit();
                }
            }
        }

        public bool UninstallPlugin(Uri uri)
        {
            SoftwareAgentPlugin plugin = Plugins.First((p) => p.Manifest.AgentUri == uri.AbsoluteUri && IsSoftwareInstalled(p.Manifest));

            return plugin != null ? UninstallPlugin(plugin) : false;
        }

        public virtual bool UninstallPlugin(SoftwareAgentPlugin plugin)
        {
            PluginManifest manifest = plugin.Manifest;

            try
            {
                foreach (DirectoryInfo location in GetApplicationLocations(manifest).Where(l => l.Exists))
                {
                    foreach (PluginManifestPluginFile pluginFile in manifest.PluginFile)
                    {
                        DirectoryInfo targetFolder = manifest.GetPluginTargetDirectory(location);

                        if (!targetFolder.Exists)
                        {
                            return false;
                        }

                        var targetPath = Path.Combine(targetFolder.FullName, pluginFile.GetName());

                        if (pluginFile.Link)
                        {
                            DeleteLink(targetPath);
                        }
                        else if (File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                        }
                        else if (Directory.Exists(targetPath))
                        {
                            Directory.Delete(targetPath);
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to uninstall plugin {0}: {1}", manifest.AgentUri, e);

                throw e;
            }
        }

        public void UninstallAgent(IModel model, SoftwareAgentPlugin plugin)
        {
            UriRef agentUri = new UriRef(plugin.Manifest.AgentUri);

            if (model.ContainsResource(agentUri))
            {
                Logger.InfoFormat("Uninstalling agent {0}", plugin.Manifest.AgentUri);

                model.DeleteResource(agentUri);
            }

            foreach(UriRef uri in plugin.GetAssociationUris())
            {
                if (model.ContainsResource(uri))
                {
                    Logger.InfoFormat("Uninstalling agent association {0}", plugin.Manifest.AgentUri);

                    model.DeleteResource(uri);
                }
            }
        }

        protected void CopyDirectory(string sourceDirName, string destDirName, bool recursive)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);

                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (recursive)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    CopyDirectory(subdir.FullName, temppath, recursive);
                }
            }
        }
                                
        protected abstract bool CreateLink(string source, string target);

        protected abstract void DeleteLink(string target);

        #endregion
    }
}
