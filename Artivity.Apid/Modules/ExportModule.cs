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
using Artivity.Apid.Parameters;
using Artivity.Api.Plugin;
using Artivity.Api.Extensions;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Nancy;
using VDS.RDF;
using VDS.RDF.Storage;
using Newtonsoft.Json;

namespace Artivity.Apid.Modules
{
    public class ExportModule : ModuleBase
    {
        #region Members

        private VirtuosoManager _virtuoso;

        #endregion

        #region Constructors

        public ExportModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/export", modelProvider, platform)
        {
            string connectionString = ModelProvider.NativeConnectionString;

            _virtuoso = new VirtuosoManager(connectionString);
                
            Get["/"] = parameters =>
            {
                string entityUri = Request.Query.entityUri;
                string targetUrl = Request.Query.targetUrl;

                if (!IsUri(entityUri) || !IsFileUrl(targetUrl))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return Export(new UriRef(entityUri), new Uri(targetUrl));
            };
        }

        #endregion

        #region Methods

        protected Response Export(UriRef entityUri, Uri targetUrl)
        {
            try
            {
                DirectoryInfo appFolder = new DirectoryInfo(PlatformProvider.ArtivityDataFolder);
                DirectoryInfo exportFolder = CreateExportDirectory(entityUri);

                ExportData(entityUri, appFolder, exportFolder);
                ExportRenderings(entityUri, appFolder, exportFolder);
                ExportAvatars(entityUri, appFolder, exportFolder);

                WriteManifest(entityUri, exportFolder);
                WriteZipArchive(entityUri, exportFolder);

                DeleteExportDirectory(exportFolder);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        private void ExportData(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder)
        {
            string dataExport = PlatformProvider.DatabaseFolder;

            dataExport = dataExport.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(dataExport))
            {
                Directory.CreateDirectory(dataExport);
            }

            ExportAgents(entityUri, dataExport);
            ExportActivities(entityUri, dataExport);
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
        }

        private void ExportActivities(UriRef entityUri, string targetDir)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    @entity
                    ?file
                    ?activity
                    ?influence
                    ?undo
                    ?redo
                    ?bounds
                    ?change
                    ?render
                WHERE
                {
                  ?activity prov:generated | prov:used @entity .

                  @entity nie:isStoredAs ?file .

                  ?influence prov:activity | prov:hadActivity ?activity .

                  OPTIONAL { ?influence art:hadViewport ?viewport . }
                  OPTIONAL { ?influence art:hadBoundaries ?bounds . }
                  OPTIONAL { ?influence art:hadChange ?change . }
                  OPTIONAL { ?influence art:renderedAs ?render . }

                  OPTIONAL { ?undo art:reverted ?influence . }
                  OPTIONAL { ?redo art:restored ?influence . }
                }");

            query.Bind("@entity", entityUri);

            WriteTurtle(query, targetDir, "activities.ttl");
        }

        private void ExportRenderings(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder)
        {
            string renderingsEntity = Path.Combine(PlatformProvider.RenderingsFolder, entityUri.ToFileName());

            if (Directory.Exists(renderingsEntity))
            {
                string renderingsApp = PlatformProvider.RenderingsFolder;
                string renderingsExport = renderingsApp.Replace(appFolder.FullName, exportFolder.FullName);

                if (!Directory.Exists(renderingsExport))
                {
                    Directory.CreateDirectory(renderingsExport);
                }

                // Copy all the files in the renderings folder to the export directory.
                foreach (string file in Directory.GetFiles(renderingsEntity, "*.png"))
                {
                    File.Copy(file, file.Replace(renderingsEntity, renderingsExport), true);
                }
            }
        }

        private void ExportAvatars(UriRef entityUri, DirectoryInfo appFolder, DirectoryInfo exportFolder)
        {
            string avatarsApp = PlatformProvider.AvatarsFolder;
            string avatarsExport = PlatformProvider.AvatarsFolder;

            avatarsExport = avatarsExport.Replace(appFolder.FullName, exportFolder.FullName);

            if (!Directory.Exists(avatarsExport))
            {
                Directory.CreateDirectory(avatarsExport);
            }

            // Export the user avatar picture.
            string file = Path.Combine(avatarsApp, PlatformProvider.Config.GetUserId() + ".jpg");

            if (File.Exists(file))
            {
                File.Copy(file, file.Replace(avatarsApp, avatarsExport), true);
            }
        }

        private void WriteTurtle(ISparqlQuery query, string targetDir, string fileName)
        {
            IGraph graph = _virtuoso.Query(query.ToString()) as IGraph;

            if (graph != null && !graph.IsEmpty)
            {
                var syntax = VDS.RDF.Parsing.TurtleSyntax.W3C;
                var writer = new VDS.RDF.Writing.CompressingTurtleWriter(syntax);

                string file = Path.Combine(targetDir, fileName);

                graph.SaveToFile(file, writer);
            }
        }

        private void WriteManifest(UriRef entityUri, DirectoryInfo exportFolder)
        {
            Dictionary<string, string> manifest = new Dictionary<string, string>();
            manifest["Format"] = "1.0";
            manifest["Entities"] = entityUri.ToString();
            manifest["Date"] = DateTime.UtcNow.ToString();

            string manifestFile = Path.Combine(exportFolder.FullName, "Manifest.json");
            string json = JsonConvert.SerializeObject(manifest);

            File.WriteAllText(manifestFile, json);
        }

        private void WriteZipArchive(UriRef entityUri, DirectoryInfo exportFolder)
        {
            string targetFile = Path.Combine(PlatformProvider.ExportFolder, entityUri.ToFileName() + ".artz");

            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            ZipFile.CreateFromDirectory(exportFolder.FullName, targetFile);
        }

        private DirectoryInfo CreateExportDirectory(UriRef entityUri)
        {
            string export = Path.Combine(PlatformProvider.ExportFolder, entityUri.ToFileName());

            // Empty any existing export directories..
            if (Directory.Exists(export))
            {
                Directory.Delete(export, true);
            }

            Directory.CreateDirectory(export);

            return new DirectoryInfo(export);
        }

        private void DeleteExportDirectory(DirectoryInfo exportFolder)
        {
            Directory.Delete(exportFolder.FullName, true);
        }

        #endregion
    }
}
