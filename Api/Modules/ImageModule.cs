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
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Nancy.Responses;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;

namespace Artivity.Api.Modules
{
    public class ImageModule : EntityModuleBase<Image>
    {
        public ImageModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/images", modelProvider, platformProvider)
        {
            Get["/rendering"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                UriRef entityUri = new UriRef(uri);

                string file = Path.Combine(PlatformProvider.GetRenderOutputPath(entityUri), "thumbnail.png");

                if (File.Exists(file))
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open);

                    StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));
                    response.Headers["Allow-Control-Allow-Origin"] = "127.0.0.1";

                    return response.AsAttachment(file);
                }
                else
                {
                    return null;
                }
            };
        }
    }
}
