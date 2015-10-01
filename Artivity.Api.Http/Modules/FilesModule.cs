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

using Artivity.Model;
using Artivity.Model.ObjectModel;
using Artivity.Api.Http.Parameters;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Nancy;
using Nancy.IO;
using Nancy.ModelBinding;
using VDS.RDF;

namespace Artivity.Api.Http
{
	public class FilesModule : ModuleBase
	{
		#region Constructors

        public FilesModule()
		{
			try
			{
				Get["/artivity/1.0/files"] = parameters => 
				{
                    if(Request.Query.fileUrl)
                    {
                        return GetUriFromFileUrl(Request.Query.fileUrl);
                    }
                    else if(Request.Query.filePath)
                    {
                        return GetUriFromFilePath(Request.Query.filePath);
                    }

                    return Logger.LogError(HttpStatusCode.BadRequest, Request.Url, "");
				};
			}
			catch(Exception e)
			{
				Logger.LogError(HttpStatusCode.InternalServerError, e);
			}
		}

		#endregion

		#region Methods

        private Response GetUriFromFileUrl(string url)
		{
            if (!url.StartsWith("file://"))
            {
                return Logger.LogError(HttpStatusCode.BadRequest, "Parameter fileUrl must be absolute URL with schema.");
            }

            Logger.LogRequest(HttpStatusCode.OK, Request);

            Uri uri = FileSystemMonitor.GetFileUri(url);

            return Response.AsJson(uri.AbsoluteUri);
		}

        private Response GetUriFromFilePath(string path)
        {
            string url = path.StartsWith("file://") ? path : "file://" + path;

            return GetUriFromFileUrl(url);
        }

		#endregion
	}
}

