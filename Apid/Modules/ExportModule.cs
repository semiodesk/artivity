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

using Artivity.Api;
using Artivity.Api.IO;
using Artivity.Api.Helpers;
using Artivity.Api.Modules;
using Artivity.Api.Platform;
using Artivity.Apid.IO;
using Artivity.Apid.Plugins;
using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using Nancy;

namespace Artivity.Apid.Modules
{
    public class ExportModule : ModuleBase
    {
        #region Members

        /// <summary>
        /// Maps a session id to a pending authentication request for querying the status.
        /// </summary>
        private static readonly Dictionary<string, TaskProgressInfo> _tasks = new Dictionary<string, TaskProgressInfo>();

        #endregion

        #region Constructors

        public ExportModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/export", modelProvider, platform)
        {
            Get["/"] = parameters =>
            {
                string entityUri = Request.Query.entityUri;
                string fileName = Request.Query.fileName;

                if (!IsUri(entityUri) || string.IsNullOrEmpty(fileName))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                string minStartTime = Request.Query.minStartTime;

                if(!string.IsNullOrEmpty(minStartTime))
                {
                    DateTimeOffset timestamp;

                    if (DateTimeOffset.TryParse(minStartTime.Replace(' ', '+'), out timestamp))
                    {
                        return Export(fileName, new UriRef(entityUri), timestamp.UtcDateTime);
                    }
                    else
                    {
                        return HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    return Export(fileName, new UriRef(entityUri), DateTime.MinValue);
                }
            };

            Get["/backup"] = parameters =>
            {
                string fileName = Request.Query.fileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return Backup(fileName);
            };

            Get["/backup/status"] = parameters =>
            {
                string taskId = Request.Query.taskId;

                if (string.IsNullOrEmpty(taskId))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetBackupStatus(taskId);
            };
        }

        #endregion

        #region Methods

        protected Response Export(string fileName, UriRef entityUri, DateTime minTime)
        {
            try
            {
                string targetFile = Path.GetFileNameWithoutExtension(fileName) + ".arta";
                string targetFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string targetPath = Path.Combine(targetFolder, targetFile);

                ArchiveWriter writer = new ArchiveWriter(PlatformProvider, ModelProvider);
                writer.Write(entityUri, targetPath, minTime);
            }
            catch (Exception ex)
            {
                PlatformProvider.Logger.LogError(ex.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        protected Response Backup(string fileName)
        {
            try
            {
                string targetFile = Path.GetFileNameWithoutExtension(fileName) + ".artb";
                string targetFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string targetPath = Path.Combine(targetFolder, targetFile);

                TaskProgressInfo progress = new TaskProgressInfo();

                _tasks[progress.Id.ToString()] = progress;

                BackupWriter writer = new BackupWriter(PlatformProvider, ModelProvider);

                try
                {
                    PlatformProvider.Logger.LogInfo("Started backup task with id: {0}", progress.Id);

                    writer.WriteAsync(targetPath, progress);
                }
                catch(Exception ex)
                {
                    progress.Error = ex;

                    throw;
                }

                return Response.AsJsonSync(progress);
            }
            catch (Exception e)
            {
                PlatformProvider.Logger.LogError(e.Message);

                return HttpStatusCode.InternalServerError;
            }
        }

        protected Response GetBackupStatus(string taskId)
        {
            if(_tasks.ContainsKey(taskId))
            {
                return Response.AsJsonSync(_tasks[taskId]);
            }
            else
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.NotFound, "Task with id {0} not found.", taskId);
            }
        }

        #endregion
    }
}
