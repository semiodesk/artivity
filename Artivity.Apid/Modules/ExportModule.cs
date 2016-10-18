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

using Artivity.DataModel;
using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
using Artivity.Apid.IO;
using Semiodesk.Trinity;
using System;
using Nancy;
using System.IO;

namespace Artivity.Apid.Modules
{
    public class ExportModule : ModuleBase
    {
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
                        return Export(new UriRef(entityUri), fileName, timestamp.UtcDateTime);
                    }
                    else
                    {
                        return HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    return Export(new UriRef(entityUri), fileName, DateTime.MinValue);
                }
            };
        }

        #endregion

        #region Methods

        protected Response Export(UriRef entityUri, string fileName, DateTime minTime)
        {
            try
            {
                string targetFile = Path.GetFileNameWithoutExtension(fileName) + ".arta";
                string targetFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                ArchiveWriter exporter = new ArchiveWriter(PlatformProvider, ModelProvider);
                exporter.Write(entityUri, Path.Combine(targetFolder, targetFile), minTime);
            }
            catch (Exception e)
            {
                PlatformProvider.Logger.LogError(e.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        #endregion
    }
}
