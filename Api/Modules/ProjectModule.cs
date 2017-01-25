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
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;

namespace Artivity.Api.Modules
{
    public class ProjectModule : EntityModuleBase<Project>
    {
        public ProjectModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/projects", modelProvider, platformProvider)
        {
            Get["/addFileToProject"] = parameters =>
            {
                string projectUri = Request.Query.projectUri;
                string fileUri = Request.Query.fileUri;

                if ((string.IsNullOrEmpty(fileUri) || !IsUri(fileUri)) && (string.IsNullOrEmpty(projectUri) || !IsUri(projectUri)))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return AddFileToProject(projectUri, fileUri);
            };
        }

        protected Response AddFileToProject(string projectUri, string fileUri)
        {
            IModel m = ModelProvider.GetActivities();
            Project proj = m.GetResource<Project>(new Uri(projectUri));
            Entity entity = m.GetResource<Entity>(new Uri(fileUri));
            if (!proj.Members_.Contains(entity))
            {
                proj.Members_.Add(entity);
                proj.Commit();
            }

            return Response.AsJson(new Dictionary<string, bool>{ {"success", true}});
        }
    }
}
