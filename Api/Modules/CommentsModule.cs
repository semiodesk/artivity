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

using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;

namespace Artivity.Api.Modules
{
    public class CommentsModule : ModuleBase
    {
        #region Constructors

        public CommentsModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/comments", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                string uri = Request.Query.entityUri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetCommentsFromEntity(new UriRef(uri));
            };

            Post["/"] = parameters =>
            {
                CommentParameter comment = this.Bind<CommentParameter>();

                if (!string.IsNullOrEmpty(comment.text))
                {
                    return PostComment(comment);
                }

                return HttpStatusCode.BadRequest;
            };

            #if DEBUG
            Get["/clean"] = parameters =>
            {
                return CleanComments();
            };
            #endif
        }

        #endregion

        #region Methods

        private Response GetCommentsFromEntity(UriRef entityUri)
        {
            LoadCurrentUser();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri
                    ?agent
                    ?time
                    ?message
                WHERE
                {
                    ?activity prov:generated ?uri ;
                        prov:wasStartedBy ?agent .

                    ?uri a art:Comment ;
                        prov:hadPrimarySource @entity ;
                        nie:created ?time ;
                        art:deleted @undefined ;
                        sioc:content ?message .
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            var bindings = UserModel.GetBindings(query, true).ToList();

            return Response.AsJson(bindings);
        }

        private Response PostComment(CommentParameter parameter)
        {
            LoadCurrentUser();

            if (!Uri.IsWellFormedUriString(parameter.entity, UriKind.Absolute))
            {
                PlatformProvider.Logger.LogError("Invalid URI for parameter 'entity': {0}", parameter.entity);

                return HttpStatusCode.BadRequest;
            }

            Agent agent = new Agent(new UriRef(parameter.agent));
            Entity entity = new Entity(new UriRef(parameter.entity));

            if (UserModel.ContainsResource(entity))
            {
                Comment comment = UserModel.CreateResource<Comment>();
                comment.CreationTimeUtc = parameter.endTime;
                comment.PrimarySource = entity; // TODO: Correct this in the data provided by the plugins.
                comment.Message = parameter.text;
                comment.Commit();

                CreateEntity activity = UserModel.CreateResource<CreateEntity>();
                activity.StartedBy = agent;
                activity.StartTime = parameter.startTime;
                activity.EndTime = parameter.endTime;
                activity.GeneratedEntities.Add(comment); // Associate the comment with the activity.
                activity.UsedEntities.Add(entity); // TODO: Correct this in the data provided by the plugins.

                if (parameter.marks != null)
                {
                    foreach (var mark in parameter.marks)
                    {
                        // The comment could not have been created without using the marker.
                        activity.UsedEntities.Add(new Mark(new UriRef(mark)));
                    }
                }

                activity.Commit();

                // Return the URI to the frontend.
                var data = new { uri = comment.Uri };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", entity);

                return HttpStatusCode.BadRequest;
            }
        }

        private Response CleanComments()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?comment
                WHERE
                {
                    ?comment a art:Comment .

                    {
                        FILTER NOT EXISTS { ?comment sioc:content ?value . }
                    }
                    UNION
                    {
                        FILTER NOT EXISTS { ?comment prov:hadActivity / prov:startedBy ?agent . }
                    }
                }");

            IModel model = ModelProvider.GetActivities();

            List<BindingSet> bindings = model.GetBindings(query).ToList();

            foreach (BindingSet b in bindings)
            {
                Uri uri = new Uri(b["comment"].ToString());

                model.DeleteResource(uri);
            }

            return HttpStatusCode.OK;
        }

        #endregion

        #region Types

        private class CommentParameter
        {
            #region Members

            public string agent { get; set; }

            public string entity { get; set; }

            public DateTime startTime { get; set; }

            public DateTime endTime { get; set; }

            public string text { get; set; }

            public string[] marks { get; set; }

            #endregion
        }

        #endregion
    }
}
