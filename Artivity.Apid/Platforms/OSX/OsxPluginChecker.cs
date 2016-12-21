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

#if OSX
using Artivity.DataModel;
using Artivity.Apid;
using Artivity.Apid.Platforms;
using System;
using System.IO;
using System.Linq;
using Mono.Unix;
using MonoDevelop.MacInterop;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Xml;

namespace Artivity.Apid.Plugin.OSX
{
    public class OsxPluginChecker : PluginChecker
    {
        #region Constructors

        public OsxPluginChecker(IPlatformProvider platformProvider, IModelProvider modelProvider, DirectoryInfo folder)
            : base(platformProvider, modelProvider, folder)
        {
        }

        #endregion

        #region Methods

        protected override IEnumerable<DirectoryInfo> GetApplicationLocations(PluginManifest manifest)
        {
            if (string.IsNullOrEmpty(manifest.SampleFile))
            {
                throw new Exception("No sample file set in manifest.");
            }

            string sample = Path.Combine(manifest.ManifestFile.Directory.FullName, manifest.SampleFile);

            if (!File.Exists(sample))
            {
                throw new Exception("Sample file does not exist: " + sample);
            }

            List<string> aassociatedApps = new List<string>();

            foreach (string appBundle in CoreFoundation.GetApplicationUrls(sample, CoreFoundation.LSRolesMask.All))
            {
                if (appBundle.Contains(manifest.SampleResultFilter))
                {
                    aassociatedApps.Add(appBundle);
                }
            }

            if (PlatformProvider != null && PlatformProvider.Config != null)
            {
                aassociatedApps.InsertRange(0, PlatformProvider.Config.SoftwarePaths);
            }

            foreach(string appBundle in aassociatedApps)
            {
                if (Directory.Exists(appBundle))
                {
                    string name;
                    string version;

                    if (GetApplicationNameAndVersion(appBundle, out name, out version))
                    {
                        if (name.Contains(manifest.SampleResultFilter) && manifest.IsMatch(version))
                        {
                            yield return new DirectoryInfo(appBundle);
                        }
                    }
                }
            }
        }

        protected bool GetApplicationNameAndVersion(string app, out string name, out string version)
        {
            name = null;
            version = null;

            if (Directory.Exists(app))
            {
                var infoPlist = Path.Combine(app, "Contents", "Info.plist");

                if (File.Exists(infoPlist))
                {
                    try
                    {
                        using (TextReader textReader = File.OpenText(infoPlist))
                        {
                            // See: http://todotnet.com/archive/2006/07/27/8248.aspx
                            // Prevent the XPathDocument from issueing a HTTP-Request to
                            // resolve the document DTD. This causes exceptions if there is no
                            // network connection.
                            XmlReaderSettings settings = new XmlReaderSettings();
                            settings.XmlResolver = null;
                            settings.DtdProcessing = DtdProcessing.Ignore;

                            XmlReader reader = XmlReader.Create(textReader, settings);

                            XPathDocument document = new XPathDocument(reader);

                            XPathNavigator root = document.CreateNavigator();

                            XPathNavigator value = root.SelectSingleNode("/plist/dict/key[text()='CFBundleShortVersionString']");
                            value.MoveToNext();

                            version = value.InnerXml;

                            value = root.SelectSingleNode("/plist/dict/key[text()='CFBundleExecutable']");
                            value.MoveToNext();

                            name = value.InnerXml;

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Artivity.Apid.Logger.LogError(ex);
                    }
                }
            }
            return false;
        }

        protected override string GetApplicationVersion(FileSystemInfo app)
        {
            if (app is DirectoryInfo)
            {
                var appBundle = app as DirectoryInfo;

                var infoPlist = Path.Combine(appBundle.FullName, "Contents", "Info.plist");

                if (File.Exists(infoPlist))
                {
                    try
                    {
                        using (TextReader textReader = File.OpenText(infoPlist))
                        {
                            // See: http://todotnet.com/archive/2006/07/27/8248.aspx
                            // Prevent the XPathDocument from issueing a HTTP-Request to
                            // resolve the document DTD. This causes exceptions if there is no
                            // network connection.
                            XmlReaderSettings settings = new XmlReaderSettings();
                            settings.XmlResolver = null;
                            settings.DtdProcessing = DtdProcessing.Ignore;

                            XmlReader reader = XmlReader.Create(textReader, settings);

                            XPathDocument document = new XPathDocument(reader);

                            XPathNavigator root = document.CreateNavigator();

                            XPathNavigator value = root.SelectSingleNode("/plist/dict/key[text()='CFBundleShortVersionString']");

                            value.MoveToNext();

                            return value.InnerXml;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);

                        Logger.Debug("Failed to parse Info.plist. May be the file is in binary format?");
                    }
                }
            }

            return null;
        }

        public override bool IsPluginInstalled(PluginManifest manifest)
        {
            foreach (DirectoryInfo location in GetApplicationLocations(manifest).Where(l => l.Exists))
            {
                foreach (PluginManifestPluginFile file in manifest.PluginFile)
                {
                    DirectoryInfo targetFolder = TryGetPluginTargetDirectory(location, manifest);

                    if (!targetFolder.Exists)
                    {
                        return false;
                    }

                    // This may be a file or app bundle / directory.
                    var targetPath = Path.Combine(targetFolder.FullName, file.GetName());

                    if (file.Link)
                    {
                        UnixSymbolicLinkInfo link = new UnixSymbolicLinkInfo(targetPath);

                        if (!link.Exists)
                        {
                            return false;
                        }
                    }
                    else if (!File.Exists(targetPath) && !Directory.Exists(targetPath))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected override bool CreateLink(string source, string target)
        {
            UnixFileInfo file = new UnixFileInfo(source);

            file.CreateSymbolicLink(target);

            UnixSymbolicLinkInfo link = new UnixSymbolicLinkInfo(target);

            return link.Exists;
        }

        protected override void DeleteLink(string target)
        {
            UnixFileInfo link = new UnixFileInfo(target);

            link.Delete();
        }

        #endregion
    }
}

#endif