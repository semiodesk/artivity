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
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using Nancy;
using Artivity.DataModel;
using Artivity.Apid.Platforms;

namespace Artivity.Apid
{
    public class MonitoringModule : ModuleBase
    {
        #region Constructors

        public MonitoringModule(IModelProvider model, IPlatformProvider platform)
            : base("/artivity/1.0/monitor", model, platform)
        {
            Get["/add"] = parameters => { return AddFile(); };
            Get["/remove"] = parameters => { return RemoveFile(); };
        }

        #endregion

        #region Methods

        private Response AddFile()
        {
            try
            {
                if (Request.Query.file)
                {
                    string path = Request.Query.file;

                    FileSystemMonitor.Instance.AddFile(path);

                    return Logger.LogRequest(HttpStatusCode.OK, Request);
                }

                return Logger.LogError(HttpStatusCode.BadRequest, Request.Url, "");
            }
            catch (Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        private Response RemoveFile()
        {
            try
            {
                if (Request.Query.file)
                {
                    string path = Request.Query.file;

                    FileSystemMonitor.Instance.RemoveFile(path);

                    return Logger.LogRequest(HttpStatusCode.OK, Request);
                }

                return Logger.LogError(HttpStatusCode.BadRequest, Request.Url, "");
            }
            catch (Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        #endregion
    }
}

