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

using Artivity.DataModel;
using Artivity.Apid.Platforms;
using System;
using System.IO;
using System.Linq;
using Mono.Unix;
using MonoDevelop.MacInterop;
using System.Xml.XPath;
using System.Collections.Generic;

namespace Artivity.Api.Plugin.OSX
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

        protected override DirectoryInfo GetApplicationLocation(PluginManifest manifest)
        {
            if (string.IsNullOrEmpty(manifest.ExampleFile))
            {
                throw new Exception("No value set for ExampleFile in manifest.");
            }

            string sample = Path.Combine(manifest.ManifestFile.Directory.FullName, manifest.ExampleFile);

            if (!File.Exists(sample))
            {
                throw new Exception("Sample file does not exist: " + sample);
            }

            List<string> list = CoreFoundation.GetApplicationUrls(sample, CoreFoundation.LSRolesMask.All).ToList();

            if (PlatformProvider != null && PlatformProvider.Config != null)
            {
                list.InsertRange(0, PlatformProvider.Config.SoftwarePaths);
            }

            if (list.Any())
            {
                string location = (from app in list where app.Contains(manifest.FilterName) select app).FirstOrDefault();

                return string.IsNullOrEmpty(location) ? null : new DirectoryInfo(location);
            }
            else
            {
                return null;
            }
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
                        XPathDocument document = new XPathDocument(infoPlist);

                        XPathNavigator root = document.CreateNavigator();

                        XPathNavigator value = root.SelectSingleNode("/plist/dict/key[text()='CFBundleShortVersionString']");

                        value.MoveToNext();

                        return value.InnerXml;
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
            DirectoryInfo location = GetApplicationLocation(manifest);

            if (location == null || !location.Exists)
            {
                return false;
            }

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

