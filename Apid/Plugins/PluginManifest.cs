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

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Artivity.Apid.Plugins
{
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.artivity.io/plugins/manifest/")]
    [XmlRootAttribute(Namespace = "http://www.artivity.io/plugins/manifest/", IsNullable = false)]
    public class PluginManifest
    {
        #region Members

        [JsonIgnore]
        [XmlIgnore]
        public FileInfo ManifestFile { get; set; }

        public string AgentUri { get; set; }

        public string DefaultColor { get; set; }

        public string DisplayName { get; set; }

        [JsonIgnore]
        public string ProcessName { get; set; }

        [JsonIgnore]
        public string ExecutablePath { get; set; }

        [JsonIgnore]
        public string ExecutableVersion { get; set; }

        [JsonIgnore]
        public string MinExecutableVersion { get; set; }

        [JsonIgnore]
        public string MaxExecutableVersion { get; set; }

        private List<string> _defaultPaths = new List<string>();

        [JsonIgnore]
        [XmlElement("DefaultPath")]
        public List<string> DefaultPaths
        {
            get { return _defaultPaths; }
            set { _defaultPaths = value; }
        }

        [JsonIgnore]
        public string SampleFile { get; set; }

        [JsonIgnore]
        public string SampleResultFilter { get; set; }

        public byte PluginArch { get; set; }

        public string PluginVersion { get; set; }

        [JsonIgnore]
        public string PluginInstallPath { get; set; }

        [JsonIgnore]
        [XmlElement("PluginInstallRelativeToBinary")]
        public bool PluginInstallRelativeToBinary { get; set; }

        private List<PluginManifestPluginFile> _pluginFile = new List<PluginManifestPluginFile>();

        [JsonIgnore]
        [XmlElement("PluginFile")]
        public List<PluginManifestPluginFile> PluginFile
        {
            get { return _pluginFile; }
            set { _pluginFile = value; }
        }

        [JsonIgnore]
        [XmlElement("Registry")]
        public PluginManifestRegistryInfo RegistryInfo { get; set; }

        public bool AutoInstall { get; set; }

        #endregion

        #region Methods

        public string GetVersion()
        {
            if (!string.IsNullOrEmpty(ExecutableVersion))
            {
                return ExecutableVersion;
            }
            else if (!string.IsNullOrEmpty(MinExecutableVersion))
            {
                return MinExecutableVersion;
            }
            else if (!string.IsNullOrEmpty(MaxExecutableVersion))
            {
                return MaxExecutableVersion;
            }
            else
            {
                return "";
            }
        }

        public DirectoryInfo GetPluginTargetDirectory(DirectoryInfo location)
        {
            string targetFolder = Environment.ExpandEnvironmentVariables(PluginInstallPath);

            if (!Path.IsPathRooted(targetFolder))
            {
                if (PluginInstallRelativeToBinary)
                {
                    // Some applications, such as Adobe Illustrator CC 2015, set a default registry entry (on Windows) which 
                    // contains a wrong application install path. Therefore, we search the given location for the binary and 
                    // install the plugins relative to the binary location as a fallback option.
                    string appFolder = Directory.EnumerateFiles(location.FullName, ProcessName, SearchOption.AllDirectories).FirstOrDefault();

                    if (!string.IsNullOrEmpty(appFolder))
                    {
                        targetFolder = Path.Combine(Path.GetDirectoryName(appFolder), targetFolder);
                    }
                }
                else
                {
                    targetFolder = Path.Combine(location.FullName, targetFolder);
                }
            }

            DirectoryInfo directory = new DirectoryInfo(targetFolder);

            if (!directory.Exists)
            {
                // Note: We intentionally do not implicitly try to create the plugin target directory here, because
                // it would mess up the file system in the most likly event the target directory has been computed wrong.
                //Logger.LogError("Plugin target directory does not exist: {0}", targetFolder);
            }

            return directory;
        }

        /// <summary>
        /// Indicates if a given (Major, Minor, Release) version string matches the version information which is specified in the manifest.
        /// </summary>
        /// <param name="versionString">A version string.</param>
        /// <returns><c>true</c> if the version was matched, <c>false</c> otherwise.</returns>
        public bool IsMatch(string versionString)
        {
            Version v = Version.Parse(versionString);

            if (!string.IsNullOrEmpty(ExecutableVersion))
            {
                Version version = Version.Parse(ExecutableVersion);

                return v == version;
            }
            else if (!string.IsNullOrEmpty(MinExecutableVersion))
            {
                Version min = Version.Parse(MinExecutableVersion);

                if (min <= v)
                {
                    if (!string.IsNullOrEmpty(MaxExecutableVersion))
                    {
                        Version max = Version.Parse(MaxExecutableVersion);

                        return v <= max;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(MaxExecutableVersion))
            {
                Version max = Version.Parse(MaxExecutableVersion);

                return v.Major <= max.Major && v.Minor <= max.Minor && v.Revision <= max.Revision;
            }

            return false;
        }

        #endregion
    }
}
