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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artivity.Api.Plugin
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.artivity.io/plugins/manifest/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.artivity.io/plugins/manifest/", IsNullable = false)]
    public partial class PluginManifest
    {
        [XmlIgnore]
        public FileInfo ManifestFile { get; set; }

        private string displayNameField;

        private string processNameField;

        private string idField;

        private string targetPathField;

        private string targetExampleFile;

        private string targetFilterName;

        private decimal versionField;

        private string hostVersionField;

        private byte archField;

        private PluginManifestPluginFile pluginFileField;

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public string ProcessName
        {
            get
            {
                return this.processNameField;
            }
            set
            {
                this.processNameField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string TargetPath
        {
            get
            {
                return this.targetPathField;
            }
            set
            {
                this.targetPathField = value;
            }
        }

        /// <remarks/>
        public string ExampleFile
        {
            get
            {
                return this.targetExampleFile;
            }
            set
            {
                this.targetExampleFile = value;
            }
        }

        /// <remarks/>
        public string FilterName
        {
            get
            {
                return this.targetFilterName;
            }
            set
            {
                this.targetFilterName = value;
            }
        }
            
        /// <remarks/>
        public decimal Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public string HostVersion
        {
            get
            {
                return this.hostVersionField;
            }
            set
            {
                this.hostVersionField = value;
            }
        }

        /// <remarks/>
        public byte Arch
        {
            get
            {
                return this.archField;
            }
            set
            {
                this.archField = value;
            }
        }

        /// <remarks/>
        public PluginManifestPluginFile PluginFile
        {
            get
            {
                return this.pluginFileField;
            }
            set
            {
                this.pluginFileField = value;
            }
        }

        public string GetPluginSource()
        {
            return Path.Combine(ManifestFile.Directory.FullName, PluginFile.Value);
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.artivity.io/plugins/manifest/")]
    public partial class PluginManifestPluginFile
    {

        private bool linkField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        public string GetName()
        {
            #if WIN
            if (Link)
                return string.Format("{0}.lnk", Value);
            else
            #endif
                return Value;
        }
    }

    public class PluginManifestReader
    {
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

        public static PluginManifest ReadManifest(FileInfo manifestFile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PluginManifest));

                StreamReader reader = new StreamReader(manifestFile.FullName);
                PluginManifest manifest = (PluginManifest)serializer.Deserialize(reader);
                reader.Close();
                manifest.ManifestFile = manifestFile;
                return manifest;
            }catch(Exception e)
            {
                Logger.ErrorFormat("Manifest {0} could not be read. {1}", manifestFile, e);
                return null;
            }
        }

        public static PluginManifest ReadManifest(DirectoryInfo puginFolder)
        {
            var x = puginFolder.GetFiles("manifest.xml");
            if (x.Length > 0)
            {
                return ReadManifest(x[0]);
            }
            return null;
        }
    }
}
