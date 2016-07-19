﻿// LICENSE:
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

using System;
using System.IO;
using System.IO.Compression;
using Artivity.Api.Helpers;
using Artivity.Api.Plugin;
using Artivity.Apid;
using Artivity.Apid.Platforms;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using VDS.RDF.Storage;

namespace Artivity.Api.IO
{
    public class ArchiveReader
    {
        #region Members

        private readonly IPlatformProvider _platformProvider;

        private readonly IModelProvider _modelProvider;

        #endregion

        #region Constructors

        public ArchiveReader(IPlatformProvider platformProvider, IModelProvider modelProvider)
        {
            _platformProvider = platformProvider;
            _modelProvider = modelProvider;
        }

        #endregion

        #region Methods

        public void Read(Uri fileUrl)
        {
            DirectoryInfo appFolder = new DirectoryInfo(_platformProvider.ArtivityDataFolder);
            DirectoryInfo importFolder = CreateImportFolder(fileUrl);

            Decompress(importFolder, fileUrl);

            ArchiveManifest manifest = ReadManifest(importFolder);

            foreach (Uri entityUri in manifest.ExportedEntites)
            {
                ImportData(appFolder, importFolder, entityUri);
                ImportRenderings(appFolder, importFolder, entityUri);
            }

            ImportAvatars(appFolder, importFolder);

            DeleteImportFolder(importFolder);
        }

        private void ImportData(DirectoryInfo appFolder, DirectoryInfo importFolder, Uri entityUri)
        {
            ImportAgents(appFolder, importFolder, entityUri);
            ImportActivities(appFolder, importFolder, entityUri);
        }

        private void ImportAgents(DirectoryInfo appFolder, DirectoryInfo importFolder, Uri entityUri)
        {
            string dataApp = _platformProvider.DatabaseFolder;
            string dataImport = _platformProvider.DatabaseFolder;

            dataImport = dataImport.Replace(appFolder.FullName, importFolder.FullName);

            FileInfo file = new FileInfo(Path.Combine(dataImport, "agents.ttl"));

            if (file.Exists)
            {
                _modelProvider.GetAgents().Read(file.ToUriRef(), RdfSerializationFormat.Turtle, true);
            }
            else
            {
                throw new FileNotFoundException(file.FullName);
            }
        }

        private void ImportActivities(DirectoryInfo appFolder, DirectoryInfo importFolder, Uri entityUri)
        {
            string dataApp = _platformProvider.DatabaseFolder;
            string dataImport = _platformProvider.DatabaseFolder;

            dataImport = dataImport.Replace(appFolder.FullName, importFolder.FullName);

            FileInfo file = new FileInfo(Path.Combine(dataImport, "activities.ttl"));

            if (file.Exists)
            {
                _modelProvider.GetActivities().Read(file.ToUriRef(), RdfSerializationFormat.Turtle, true);
            }
            else
            {
                throw new FileNotFoundException(file.FullName);
            }
        }

        private void ImportAvatars(DirectoryInfo appFolder, DirectoryInfo importFolder)
        {
            string avatarsApp = _platformProvider.AvatarsFolder;
            string avatarsImport = _platformProvider.AvatarsFolder;

            avatarsImport = avatarsImport.Replace(appFolder.FullName, importFolder.FullName);

            if (!Directory.Exists(avatarsImport))
            {
                return;
            }

            // Copy all the files in the renderings folder to the export directory.
            foreach (string file in Directory.GetFiles(avatarsImport, "*.jpg"))
            {
                FileInfo source = new FileInfo(file);
                FileInfo target = new FileInfo(Path.Combine(avatarsApp, Path.GetFileName(file)));

                if (target.Exists && (target.Length == source.Length || target.CreationTime >= source.CreationTime))
                {
                    continue;
                }

                File.Copy(source.FullName, target.FullName, true);
            }
        }

        private void ImportRenderings(DirectoryInfo appFolder, DirectoryInfo importFolder, Uri entityUri)
        {
            string renderingsApp = _platformProvider.RenderingsFolder;
            string renderingsSource = _platformProvider.RenderingsFolder;

            renderingsSource = renderingsSource.Replace(appFolder.FullName, importFolder.FullName);

            string renderingsTarget = Path.Combine(renderingsApp, FileNameEncoder.Encode(entityUri.AbsoluteUri));

            if (!Directory.Exists(renderingsTarget))
            {
                Directory.CreateDirectory(renderingsTarget);
            }

            // Copy all the files in the renderings folder to the export directory.
            foreach (string file in Directory.GetFiles(renderingsSource, "*.png"))
            {
                FileInfo source = new FileInfo(file);
                FileInfo target = new FileInfo(Path.Combine(renderingsTarget, Path.GetFileName(file)));

                if (target.Exists && (target.Length == source.Length || target.CreationTime >= source.CreationTime))
                {
                    continue;
                }

                File.Copy(source.FullName, target.FullName, true);
            }
        }

        private DirectoryInfo CreateImportFolder(Uri fileUrl)
        {
            string fileName = FileNameEncoder.Encode(Path.GetFileNameWithoutExtension(fileUrl.LocalPath));

            string import = Path.Combine(_platformProvider.ImportFolder, fileName);

            // Empty any existing import directories..
            if (Directory.Exists(import))
            {
                Directory.Delete(import, true);
            }

            Directory.CreateDirectory(import);

            return new DirectoryInfo(import);
        }

        private void DeleteImportFolder(DirectoryInfo importFolder)
        {
            Directory.Delete(importFolder.FullName, true);
        }

        private ArchiveManifest ReadManifest(DirectoryInfo importFolder)
        {
            FileInfo file = new FileInfo(Path.Combine(importFolder.FullName, "Manifest.json"));

            if (file.Exists)
            {
                // Note: JsonConvert.Deserialize threw comment parsing errors on a comment-less document.
                using (TextReader reader = File.OpenText(file.FullName))
                {
                    return new JsonSerializer().Deserialize<ArchiveManifest>(new JsonTextReader(reader));
                }
            }
            else
            {
                throw new FileNotFoundException(file.FullName);
            }
        }

        private void Decompress(DirectoryInfo importFolder, Uri fileUrl)
        {
            ZipFile.ExtractToDirectory(fileUrl.LocalPath, importFolder.FullName);
        }

        #endregion
    }
}
