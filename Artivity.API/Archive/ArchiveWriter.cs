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

using Artivity.Apid.IO;
using Artivity.Apid.Platforms;
using Artivity.Apid.Helpers;
using Artivity.DataModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Storage;

namespace Artivity.Apid.IO
{
    public class ArchiveWriter : INotifyProgressChanged
    {
        #region Members

        private readonly IPlatformProvider _platformProvider;

        private readonly IModelProvider _modelProvider;

        private VirtuosoManager _virtuosoManager;

        public TaskProgressInfo Progress { get; private set; }

        #endregion

        #region Constructors

        public ArchiveWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
        {
            _platformProvider = platformProvider;
            _modelProvider = modelProvider;
            _virtuosoManager = new VirtuosoManager(_modelProvider.NativeConnectionString);

            Progress = new TaskProgressInfo(artf.ExportArchive.Uri);
        }

        #endregion

        #region Methods

        public async Task WriteAsync(UriRef entityUri, string targetPath, DateTime minTime)
        {
            await Task.Run(() => Write(entityUri, targetPath, minTime));
        }

        public void Write(UriRef entityUri, string targetPath, DateTime minTime)
        {
            FileInfo targetFile = new FileInfo(targetPath);

            if (!targetFile.Directory.Exists)
            {
                throw new DirectoryNotFoundException("The target directory does not exist.");
            }

            // We track the completion of 8 different methods + the number of compied renderings.
            Progress.Total = 9 + GetRenderingsCount(entityUri);
            RaiseProgressChanged();

            DirectoryInfo appFolder = new DirectoryInfo(_platformProvider.ArtivityDataFolder);
            DirectoryInfo exportFolder = CreateExportFolder(entityUri);

            ExportData(entityUri, appFolder, exportFolder, minTime);
            ExportRenderings(entityUri, appFolder, exportFolder, minTime);
            ExportAvatars(entityUri, appFolder, exportFolder);

            WriteManifest(entityUri, exportFolder);

            FileInfo archiveFile = CompressExportFolder(entityUri, exportFolder);

            DeleteExportFolder(exportFolder);

            File.Move(archiveFile.FullName, targetFile.FullName);
        }

        private void ExportData(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder, DateTime minTime)
        {
            string dataExport = _platformProvider.DatabaseFolder;

            dataExport = dataExport.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(dataExport))
            {
                Directory.CreateDirectory(dataExport);
            }

            ExportAgents(entityUri, dataExport);
            ExportActivities(entityUri, dataExport, minTime);
        }

        private void ExportAgents(UriRef entityUri, string targetDir)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?agent
                    ?association
                WHERE
                {
                  ?activity prov:generated | prov:used @entity .
                  ?activity prov:qualifiedAssociation ?association .

                  ?association prov:agent ?agent .
                }");

            query.Bind("@entity", entityUri);

            WriteTurtle(query, targetDir, "agents.ttl");

            Progress.Completed++;
            RaiseProgressChanged();
        }

        private void ExportActivities(UriRef entityUri, string targetDir, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    @entity
                    ?file
                    ?activity
                    ?influence
                    ?entity
                    ?undo
                    ?redo
                    ?bounds
                    ?change
                    ?render
                    ?renderRegion
                WHERE
                {
                  ?activity prov:generated | prov:used @entity .
                  ?activity prov:startedAtTime ?startTime .

                  FILTER(@minTime <= ?startTime) .

                  @entity nie:isStoredAs ?file .

                  ?influence prov:activity | prov:hadActivity ?activity .

                  OPTIONAL
                  {
                     ?influence art:renderedAs ?render .

                     OPTIONAL { ?render art:region ?renderRegion . }
                  }

                  OPTIONAL { ?influence art:hadViewport ?viewport . }
                  OPTIONAL { ?influence art:hadBoundaries ?bounds . }
                  OPTIONAL
                  {
                     ?influence art:hadChange ?change .

                     OPTIONAL { ?change art:entity ?entity . }
                  }

                  OPTIONAL { ?undo art:reverted ?influence . }
                  OPTIONAL { ?redo art:restored ?influence . }
                }");

            query.Bind("@entity", entityUri);
            query.Bind("@minTime", minTime);

            WriteTurtle(query, targetDir, "activities.ttl");

            Progress.Completed++;
            RaiseProgressChanged();
        }

        private long GetRenderingsCount(UriRef entityUri)
        {
            string renderingsEntity = Path.Combine(_platformProvider.RenderingsFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri));

            if (Directory.Exists(renderingsEntity))
            {
                return Directory.GetFiles(renderingsEntity, "*.png", SearchOption.TopDirectoryOnly).Length;
            }

            return 0;
        }

        private void ExportRenderings(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder, DateTime minTime)
        {
            string renderingsEntity = Path.Combine(_platformProvider.RenderingsFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri));

            if (Directory.Exists(renderingsEntity))
            {
                string renderingsApp = _platformProvider.RenderingsFolder;
                string renderingsExport = renderingsApp.Replace(appFolder.FullName, exportFolder.FullName);

                if (!Directory.Exists(renderingsExport))
                {
                    Directory.CreateDirectory(renderingsExport);
                }

                // Copy all the files in the renderings folder to the export directory.
                foreach (string fileName in Directory.GetFiles(renderingsEntity, "*.png", SearchOption.TopDirectoryOnly))
                {
                    FileInfo file = new FileInfo(fileName);

                    if (minTime < file.CreationTimeUtc)
                    {
                        File.Copy(file.FullName, file.FullName.Replace(renderingsEntity, renderingsExport), true);
                    }

                    Progress.Completed++;
                    RaiseProgressChanged();
                }
            }
        }

        private void ExportAvatars(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder)
        {
            string avatarsApp = _platformProvider.AvatarsFolder;
            string avatarsExport = _platformProvider.AvatarsFolder;

            avatarsExport = avatarsExport.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(avatarsExport))
            {
                Directory.CreateDirectory(avatarsExport);
            }

            // Export the user avatar picture.
            string file = Path.Combine(avatarsApp, _platformProvider.Config.GetUserId() + ".jpg");

            if (File.Exists(file))
            {
                File.Copy(file, file.Replace(avatarsApp, avatarsExport), true);
            }

            Progress.Completed++;
            RaiseProgressChanged();
        }

        private void WriteTurtle(ISparqlQuery query, string targetDir, string fileName)
        {
            IGraph graph = _virtuosoManager.Query(query.ToString()) as IGraph;

            if (graph != null && !graph.IsEmpty)
            {
                var syntax = VDS.RDF.Parsing.TurtleSyntax.W3C;
                var writer = new VDS.RDF.Writing.CompressingTurtleWriter(syntax);

                string file = Path.Combine(targetDir, fileName);

                graph.SaveToFile(file, writer);
            }

            Progress.Completed++;
            RaiseProgressChanged();
        }

        private void WriteManifest(UriRef entityUri, DirectoryInfo exportFolder)
        {
            ArchiveManifest manifest = new ArchiveManifest();
            manifest.FileFormat = "1.0";
            manifest.ExportDate = DateTime.UtcNow;
            manifest.ExportedEntites.Add(entityUri);

            string manifestFile = Path.Combine(exportFolder.FullName, "Manifest.json");
            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                
            File.WriteAllText(manifestFile, json);

            Progress.Completed++;
            RaiseProgressChanged();
        }

        private FileInfo CompressExportFolder(UriRef entityUri, DirectoryInfo exportFolder)
        {
            string exportArchive = Path.Combine(_platformProvider.ExportFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri) + ".arta");

            if (File.Exists(exportArchive))
            {
                File.Delete(exportArchive);
            }

            ZipFile.CreateFromDirectory(exportFolder.FullName, exportArchive);

            Progress.Completed++;
            RaiseProgressChanged();

            return new FileInfo(exportArchive);
        }

        private DirectoryInfo CreateExportFolder(UriRef entityUri)
        {
            string exportFolder = Path.Combine(_platformProvider.ExportFolder, FileNameEncoder.Encode(entityUri.AbsoluteUri));

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

        private void RaiseProgressChanged()
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, Progress);
            }
        }

        #endregion
    }
}

