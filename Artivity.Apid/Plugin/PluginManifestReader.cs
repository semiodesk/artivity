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

using Artivity.Api.Plugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Artivity.Apid.Plugin
{
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

        public static PluginManifest ReadManifest(DirectoryInfo pluginFolder)
        {
            FileInfo[] manifestFiles = pluginFolder.GetFiles("manifest.xml");

            return manifestFiles.Any() ? ReadManifest(manifestFiles[0]) : null;
        }

        public static PluginManifest ReadManifest(FileInfo manifestFile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PluginManifest));

                using (StreamReader reader = new StreamReader(manifestFile.FullName))
                {
                    PluginManifest manifest = (PluginManifest)serializer.Deserialize(reader);

                    manifest.ManifestFile = manifestFile;

                    FileInfo[] iconFiles = manifestFile.Directory.GetFiles("icon.png");

                    if (iconFiles.Any())
                    {
                        manifest.IconPath = iconFiles[0].FullName;
                    }

                    return manifest;
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Manifest {0} could not be read. {1}", manifestFile, e);

                return null;
            }
        }
    }
}
