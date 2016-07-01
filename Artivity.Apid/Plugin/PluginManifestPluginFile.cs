using Artivity.Api.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Artivity.Api.Plugin
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.artivity.io/plugins/manifest/")]
    public partial class PluginManifestPluginFile
    {
        private bool linkField;

        private string valueField;

        /// <remarks/>
        [XmlAttributeAttribute()]
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
        [XmlTextAttribute()]
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

        public string GetPluginSource(PluginManifest manifest)
        {
            return Path.Combine(manifest.ManifestFile.Directory.FullName, Value);
        }
    }
}
