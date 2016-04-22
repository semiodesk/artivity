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
            
        #endregion

        #region Constructor
        public PluginChecker()
        {
            LoadManifests();
        }
        #endregion

        #region Methods

        private void LoadManifests()
        {
            string dirString = Path.Combine(Environment.CurrentDirectory, "Plugins");
            DirectoryInfo dir = new DirectoryInfo(dirString);
            if (dir == null || !dir.Exists)
            {
                Logger.ErrorFormat("Could not find plugin directory \"{0}\"", dirString);
                return;
            }

            foreach (var plugin in dir.EnumerateDirectories())
            {
                var manifest = PluginManifestReader.ReadManifest(plugin);
                if (manifest != null)
                    _manifests.Add(manifest);
                else
                    Logger.ErrorFormat("Directory {0} did not contain valid manifest.", plugin.FullName);
            }
        }

        public void Check()
        {
            foreach (var man in _manifests)
            {
                bool? res = IsPluginInstalled(man);
                if (res.HasValue && !res.Value)
                {
                    InstallPlugin(man);
                }
            }
        }

        protected abstract DirectoryInfo GetApplicationLocation (PluginManifest manifest);
        protected abstract void CreateLink (string target, string source);

        protected bool? IsPluginInstalled (PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation (manifest);
            if(location != null && location.Exists)
            {
                var pluginPath = Path.Combine(location.FullName, manifest.TargetPath, manifest.PluginFile.GetName());
                return File.Exists(pluginPath) || Directory.Exists(pluginPath);
            }
            return null;
        }


        protected void InstallPlugin (PluginManifest manifest)
        {
            DirectoryInfo location = GetApplicationLocation (manifest);
            if (location != null && location.Exists)
            {
                var pluginPath = Path.Combine(location.FullName, manifest.TargetPath, manifest.PluginFile.GetName());
                string source = manifest.GetPluginSource();
                if (File.Exists(source) || Directory.Exists(source))
                {
                    if (manifest.PluginFile.Link)
                    {
                        CreateLink(pluginPath, source);
                    }
                    else
                    {
                        File.Copy(source, pluginPath);
                    }
                }
            }
        }
        #endregion
    }

    public class PluginCheckerFactory
    {
        public static PluginChecker CreatePluginChecker()
        {
            #if WIN
            return new Artivity.Api.Plugin.Win.WinPluginChecker();
            #elif OSX
            return new Artivity.Api.Plugin.OSX.OsxPluginChecker();
            #endif
        }
    }
}
