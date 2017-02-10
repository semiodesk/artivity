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
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;

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

        public void Read(string fileName)
        {
            Read(new Uri("file://" + fileName));
        }

        public void Read(Uri fileUrl)
        {
            DirectoryInfo appFolder = new DirectoryInfo(_platformProvider.ArtivityDataFolder);
            DirectoryInfo importFolder = CreateImportFolder(fileUrl);

            Decompress(importFolder, fileUrl);

            ArchiveManifest manifest = ReadManifestFromDirectory(importFolder);

            foreach (Uri entityUri in manifest.ExportedEntites)
            {
                ImportData(appFolder, importFolder, entityUri);
                ImportRenderings(appFolder, importFolder, manifest);
            }

            ImportAvatars(appFolder, importFolder);

            DeleteImportFolder(importFolder);
        }

        public ArchiveManifest GetManifest(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            using (ZipArchive archive = ZipFile.OpenRead(fileName))
            {
                ZipArchiveEntry entry = archive.GetEntry("Manifest.json");

                // Note: JsonConvert.Deserialize threw comment parsing errors on a comment-less document.
                using (StreamReader reader = new StreamReader(entry.Open()))
                {
                    return new JsonSerializer().Deserialize<ArchiveManifest>(new JsonTextReader(reader));
                }
            }
        }

        private ArchiveManifest ReadManifestFromDirectory(DirectoryInfo importFolder)
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

            if (Directory.Exists(avatarsImport))
            {
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
        }

        private void ImportRenderings(DirectoryInfo appFolder, DirectoryInfo importFolder, ArchiveManifest manifest)
        {
            string renderingsFolder = _platformProvider.RenderingsFolder.Replace(appFolder.FullName, importFolder.FullName);

            if (!Directory.Exists(renderingsFolder))
            {
                return;
            }

            if(manifest.FileFormat == "1.0")
            {
                // Format 1.0 only supports the epxort of one entity per archive.
                if (manifest.ExportedEntites.Count != 1)
                {
                    return;
                }

                Uri entityUri = manifest.ExportedEntites.First();

                string targetFolder = Path.Combine(_platformProvider.RenderingsFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri));

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                // All renderings for this entity are contained in the 'Renderings' folder.
                CopyRenderingFiles(renderingsFolder, targetFolder);
            }
            else
            {
                // In newer versions of the format, all renderings are copied into a sub folder in 'Renderings'
                // which is named after the entity the renderings belong to.
                foreach (string sourceDirectory in Directory.GetDirectories(renderingsFolder))
                {
                    string renderingsTarget = Path.Combine(_platformProvider.RenderingsFolder, Path.GetFileName(sourceDirectory));

                    if (!Directory.Exists(renderingsTarget))
                    {
                        Directory.CreateDirectory(renderingsTarget);
                    }

                    CopyRenderingFiles(sourceDirectory, renderingsTarget);
                }
        }
        }

        private void CopyRenderingFiles(string sourceDirectory, string targetDirectory)
        {
            // Copy all the files in the renderings folder to the export directory.
            foreach (string file in Directory.GetFiles(sourceDirectory, "*.png"))
            {
                FileInfo source = new FileInfo(file);
                FileInfo target = new FileInfo(Path.Combine(targetDirectory, Path.GetFileName(file)));

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

        private void Decompress(DirectoryInfo importFolder, Uri fileUrl)
        {
            using (ZipArchive archive = ZipFile.OpenRead(fileUrl.LocalPath))
            {
                if (archive == null)
                {
                    throw new ArgumentNullException("fileUrl");
                }

                if (importFolder == null)
                {
                    throw new ArgumentNullException("importFolder");
                }

                if (!importFolder.Exists)
                {
                    importFolder.Create();
                }

                string targetFolder = importFolder.FullName;

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string target = GetEntryTargetFile(entry, targetFolder, true);

                    if (!Directory.Exists(target))
                    {
                        entry.ExtractToFile(target, false);
                    }
                }
            }
        }

        private string GetEntryTargetFile(ZipArchiveEntry entry, string targetFolder, bool createTargetFolder)
        {
            string result = targetFolder;

            string[] path;

            if (entry.FullName.Contains('\\'))
            {
                path = entry.FullName.Split('\\');
            }
            else
            {
                path = entry.FullName.Split('/');
            }

            if (path.Length > 1)
            {
                for (int i = 0; i < path.Length - 1; i++)
                {
                    result = Path.Combine(result, path[i]);

                    if (!Directory.Exists(result) && createTargetFolder)
                    {
                        Directory.CreateDirectory(result);
                    }
                }
            }

            if (path.Length > 0)
            {
                string fileName = path.Last();

                result = Path.GetFullPath(Path.Combine(result, path.Last()));
            }

            return result;
        }

        #endregion
    }
}

