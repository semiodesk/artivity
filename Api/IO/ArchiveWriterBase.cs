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

using Artivity.Api.Platform;
using Artivity.Api.Helpers;
using Artivity.DataModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Storage;
using System.Collections.Generic;

namespace Artivity.Api.IO
{
    public abstract class ArchiveWriterBase : INotifyProgressChanged
    {
        #region Members

        protected readonly VirtuosoManager VirtuosoManager;

        protected readonly IPlatformProvider PlatformProvider;

        protected readonly IModelProvider ModelProvider;

        public TaskProgressInfo Progress { get; private set; }

        #endregion

        #region Constructors

        public ArchiveWriterBase(IPlatformProvider platformProvider, IModelProvider modelProvider)
        {
            PlatformProvider = platformProvider;
            ModelProvider = modelProvider;

            VirtuosoManager = new VirtuosoManager(ModelProvider.NativeConnectionString);

            Progress = new TaskProgressInfo(artf.ExportArchive.Uri);
        }

        #endregion

        #region Methods

        public async Task WriteAsync(Uri user, Uri uri, string targetPath, DateTime minTime)
        {
            await Task.Run(() => Write(user, uri, targetPath, minTime));
        }

        public void Write(Uri user, Uri uri, string targetPath, DateTime minTime)
        {
            FileInfo targetFile = new FileInfo(targetPath);

            if (!targetFile.Directory.Exists)
            {
                throw new DirectoryNotFoundException("The target directory does not exist.");
            }

            // We track the completion of 8 different methods + the number of compied renderings.
            Progress.Total = 9 + GetRenderings(uri, minTime).Count();

            RaiseProgressChanged();

            DirectoryInfo appFolder = new DirectoryInfo(PlatformProvider.ArtivityDataFolder);
            DirectoryInfo exportFolder = CreateExportFolder(uri);

            ExportData(uri, appFolder, exportFolder, minTime);
            ExportRenderings(uri, appFolder, exportFolder, minTime);
            ExportAvatars(uri, appFolder, exportFolder, user);

            WriteManifest(uri, exportFolder);

            FileInfo archiveFile = CompressExportFolder(uri, exportFolder);

            DeleteExportFolder(exportFolder);

            File.Move(archiveFile.FullName, targetFile.FullName);
        }

        protected abstract ISparqlQuery GetAgentsQuery(Uri uri, DateTime minTime);

        protected abstract ISparqlQuery GetActivitiesQuery(Uri uri, DateTime minTime);

        protected abstract IEnumerable<EntityRenderingInfo> GetRenderings(Uri uri, DateTime minTime);

        private void ExportData(Uri uri, DirectoryInfo appFolder, DirectoryInfo exportFolder, DateTime minTime)
        {
            string dataExport = PlatformProvider.DatabaseFolder;

            dataExport = dataExport.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(dataExport))
            {
                Directory.CreateDirectory(dataExport);
            }

            using(FileStream fs = new FileStream(Path.Combine(dataExport, "agents.ttl"), FileMode.CreateNew))
            {
                ExportAgents(uri, fs, minTime);
            }
            Progress.Completed++;
            RaiseProgressChanged();

            using (FileStream fs = new FileStream(Path.Combine(dataExport, "activities.ttl"), FileMode.CreateNew))
            {
                ExportActivities(uri, fs, minTime);
            }
            
            Progress.Completed++;
            RaiseProgressChanged();
        }

        private void ExportAgents(Uri uri, Stream stream, DateTime minTime)
        {
            ISparqlQuery query = GetAgentsQuery(uri, minTime);

            WriteTurtle(query, stream);
        }

        private void ExportActivities(Uri uri, Stream stream, DateTime minTime)
        {
            ISparqlQuery query = GetActivitiesQuery(uri, minTime);

            WriteTurtle(query, stream);
        }

        protected virtual void ExportRenderings(Uri uri, DirectoryInfo appFolder, DirectoryInfo exportFolder, DateTime minTime)
        {
            foreach(EntityRenderingInfo info in GetRenderings(uri, minTime))
            {
                string entityFolder = FileNameEncoder.Encode(info.EntityUri);
                string sourceFolder = Path.Combine(PlatformProvider.RenderingsFolder, entityFolder);

                if (Directory.Exists(sourceFolder))
                {
                    string sourceFile = Path.Combine(sourceFolder, info.Rendering.Name);

                    string[] path = new string[] { exportFolder.FullName, Path.GetFileName(PlatformProvider.RenderingsFolder), entityFolder };

                    string targetFolder = string.Join(Path.DirectorySeparatorChar.ToString(), path);

                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    ExportThumbnail(uri, sourceFolder, targetFolder);

                    if (File.Exists(sourceFile))
                    {
                        File.Copy(sourceFile, sourceFile.Replace(sourceFolder, targetFolder), true);

                        Progress.Completed++;

                        RaiseProgressChanged();
                    }
                }
            }
        }

        private void ExportThumbnail(Uri uri, string sourceFolder, string targetFolder)
        {
            string thumbnailName = "thumbnail.png";

            string sourceFile = Path.Combine(sourceFolder, thumbnailName);
            string targetFile = Path.Combine(targetFolder, thumbnailName);

            if(File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                File.Copy(sourceFile, targetFile);
            }
        }

        private void ExportAvatars(Uri uri, DirectoryInfo appFolder, DirectoryInfo exportFolder, Uri user)
        {
            string sourceFolder = PlatformProvider.AvatarsFolder;
            string targetFolder = PlatformProvider.AvatarsFolder.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            // Export the user avatar picture.
            string file = Path.Combine(sourceFolder, FileNameEncoder.Encode(user.AbsoluteUri) + ".jpg");

            if (File.Exists(file))
            {
                File.Copy(file, file.Replace(sourceFolder, targetFolder), true);
            }

            Progress.Completed++;

            RaiseProgressChanged();
        }

        private void WriteTurtle(ISparqlQuery query, Stream output)
        {
            IGraph graph = VirtuosoManager.Query(query.ToString()) as IGraph;

            if (graph != null && !graph.IsEmpty)
            {
                var syntax = VDS.RDF.Parsing.TurtleSyntax.W3C;

                var writer = new VDS.RDF.Writing.CompressingTurtleWriter(syntax);
                writer.DefaultNamespaces.AddNamespace("art", ART.Namespace);
                writer.DefaultNamespaces.AddNamespace("dc", DCES.Namespace);
                writer.DefaultNamespaces.AddNamespace("nie", NIE.Namespace);
                writer.DefaultNamespaces.AddNamespace("nfo", NFO.Namespace);
                writer.DefaultNamespaces.AddNamespace("prov", PROV.Namespace);

                using (StreamWriter wr = new StreamWriter(output) )
                {
                    graph.SaveToStream(wr, writer);
                }
            }

            Progress.Completed++;

            RaiseProgressChanged();
        }

        private void WriteTurtle(ISparqlQuery query, string targetDir, string fileName)
        {
            IGraph graph = VirtuosoManager.Query(query.ToString()) as IGraph;

            if (graph != null && !graph.IsEmpty)
            {
                var syntax = VDS.RDF.Parsing.TurtleSyntax.W3C;

                var writer = new VDS.RDF.Writing.CompressingTurtleWriter(syntax);
                writer.DefaultNamespaces.AddNamespace("art", ART.Namespace);
                writer.DefaultNamespaces.AddNamespace("dc", DCES.Namespace);
                writer.DefaultNamespaces.AddNamespace("nie", NIE.Namespace);
                writer.DefaultNamespaces.AddNamespace("nfo", NFO.Namespace);
                writer.DefaultNamespaces.AddNamespace("prov", PROV.Namespace);

                string file = Path.Combine(targetDir, fileName);

                graph.SaveToFile(file, writer);
            }

            Progress.Completed++;

            RaiseProgressChanged();
        }

        private void WriteManifest(Uri entityUri, DirectoryInfo exportFolder)
        {
            ArchiveManifest manifest = new ArchiveManifest();
            manifest.FileFormat = "1.1";
            manifest.ExportDate = DateTime.UtcNow;
            manifest.ExportedEntites.Add(entityUri);

            string manifestFile = Path.Combine(exportFolder.FullName, "Manifest.json");
            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                
            File.WriteAllText(manifestFile, json);

            Progress.Completed++;

            RaiseProgressChanged();
        }

        private FileInfo CompressExportFolder(Uri entityUri, DirectoryInfo exportFolder)
        {
            string exportArchive = Path.Combine(PlatformProvider.ExportFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri) + ".arta");

            if (File.Exists(exportArchive))
            {
                File.Delete(exportArchive);
            }

            ZipFile.CreateFromDirectory(exportFolder.FullName, exportArchive);

            Progress.Completed++;

            RaiseProgressChanged();

            return new FileInfo(exportArchive);
        }

        private DirectoryInfo CreateExportFolder(Uri entityUri)
        {
            string exportFolder = Path.Combine(PlatformProvider.ExportFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri));

            // Empty any existing export directories..
            if (Directory.Exists(exportFolder))
            {
                Directory.Delete(exportFolder, true);
            }

            Directory.CreateDirectory(exportFolder);

            Progress.Completed++;

            RaiseProgressChanged();

            return new DirectoryInfo(exportFolder);
        }

        private void DeleteExportFolder(DirectoryInfo exportFolder)
        {
            Directory.Delete(exportFolder.FullName, true);

            Progress.Completed++;

            RaiseProgressChanged();
        }

        #endregion

        #region Events

        public event ProgressChangedEventHandler ProgressChanged;

        protected void RaiseProgressChanged()
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, Progress);
            }
        }

        #endregion

        #region Classes

        protected class EntityRenderingInfo
        {
            #region Members

            public readonly string EntityUri;

            public readonly FileInfo Rendering;

            #endregion

            #region Constructors

            public EntityRenderingInfo(string entityUri, string fileName)
            {
                EntityUri = entityUri;
                Rendering = new FileInfo(fileName);
            }

            #endregion
        }

        #endregion
    }
}

