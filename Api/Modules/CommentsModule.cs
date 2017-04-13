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
using Nancy.Security;
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
                InitializeRequest();

                if(IsUri(Request.Query.entityUri))
                {
                    UriRef entityUri = new UriRef(Request.Query.entityUri);

                    return GetCommentsFromPrimarySource(entityUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Post["/"] = parameters =>
            {
                InitializeRequest();

                CommentParameter comment = this.Bind<CommentParameter>();

                if (comment.Validate() && IsUri(comment.primarySource))
                {
                    return PostComment(comment);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Post["/requests"] = parameters =>
            {
                InitializeRequest();

                CommentParameter request = this.Bind<CommentParameter>();

                if (request.Validate() && IsUri(request.primarySource) && request.associations.Any())
                {
                    if (request.type == CommentTypes.ApprovalRequest)
                    {
                        return PostRequest<ApprovalRequest>(request);
                    }
                    else
                    {
                        return PostRequest<FeedbackRequest>(request);
                    }
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Post["/responses"] = parameters =>
            {
                InitializeRequest();

                CommentParameter response = this.Bind<CommentParameter>();

                if (response.Validate() && IsUri(response.primarySource))
                {
                    if (response.type == CommentTypes.ApprovalResponse)
                    {
                        return PostResponse<ApprovalResponse>(response);
                    }
                    else
                    {
                        return PostResponse<FeedbackResponse>(response);
                    }
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
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

        private Response GetCommentsFromPrimarySource(UriRef entityUri)
        {
            IModel model = ModelProvider.GetAll();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
	                ?type
	                ?uri
                    ?agentId
	                ?message
	                ?primarySource
	                ?startTime
	                ?endTime
	                CONCAT('[', STR(GROUP_CONCAT(DISTINCT ?association_; SEPARATOR=',')),']') AS ?associations
                WHERE
                {
	                ?activity prov:generated ?uri ;
		                prov:wasStartedBy ?agent ;
		                prov:startedAtTime ?startTime ;
		                prov:endedAtTime ?endTime .

                    ?agent dces:identifier ?agentId .

	                ?uri a ?t ;
		                prov:hadPrimarySource @entity;
		                nie:created ?time ;
		                art:deleted @undefined;
		                sioc:content ?message .
	
	                BIND(@entity AS ?primarySource)
	                BIND(STRAFTER(STR(?t) , STR(art:)) AS ?type)
	
	                OPTIONAL
	                {
		                ?activity prov:qualifiedAssociation [ prov:agent ?a ; prov:hadRole ?r ] .
	
		                BIND(CONCAT('{{agent: \'', STR(?a),'\', role: \'', STR(?r), '\'}}') AS ?association_)
	                }
                }
                GROUP BY ?type ?uri ?agentId ?message ?primarySource ?startTime ?endTime
                ORDER BY DESC (?endTime)
            ");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            List<BindingSet> result = model.GetBindings(query).ToList();

            foreach(BindingSet b in result)
            {
                string json = b["associations"].ToString();

                b["associations"] = JsonConvert.DeserializeObject(json);
            }

            return Response.AsJsonSync(result);
        }

        private Response PostComment(CommentParameter parameter)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            Entity primarySource = new Entity(new UriRef(parameter.primarySource));

            if (model.ContainsResource(primarySource))
            {
                Comment comment = model.CreateResource<Comment>(ModelProvider.CreateUri<Comment>());
                comment.CreationTimeUtc = parameter.endTime;
                comment.PrimarySource = primarySource;
                comment.Message = parameter.message;
                comment.IsSynchronizable = true;
                comment.Commit();

                CreateEntity activity = model.CreateResource<CreateEntity>(ModelProvider.CreateUri<CreateEntity>());
                activity.StartedBy = new Agent(new UriRef(parameter.agent));
                activity.StartTimeUtc = parameter.startTime;
                activity.EndTimeUtc = parameter.endTime;
                activity.GeneratedEntities.Add(comment);
                activity.UsedEntities.Add(primarySource);

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
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", primarySource);

                return HttpStatusCode.BadRequest;
            }
        }

        private Response PostRequest<T>(CommentParameter parameter) where T : FeedbackRequest
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            Entity primarySource = new Entity(new UriRef(parameter.primarySource));

            if (model.ContainsResource(primarySource))
            {
                var request = model.CreateResource<T>(ModelProvider.CreateUri<T>());

                request.CreationTimeUtc = parameter.endTime;
                request.PrimarySource = primarySource;
                request.Message = parameter.message;
                request.IsSynchronizable = true;
                request.Commit();

                CreateEntity activity = model.CreateResource<CreateEntity>(ModelProvider.CreateUri<CreateEntity>());
                activity.StartedBy = new Agent(new UriRef(parameter.agent));
                activity.StartTimeUtc = parameter.startTime;
                activity.EndTimeUtc = parameter.endTime;
                activity.GeneratedEntities.Add(request);
                activity.UsedEntities.Add(primarySource);

                if (parameter.marks != null)
                {
                    foreach (var mark in parameter.marks)
                    {
                        activity.UsedEntities.Add(new Mark(new UriRef(mark)));
                    }
                }

                if (parameter.associations != null)
                {
                    foreach (AssociationParameter a in parameter.associations)
                    {
                        Association association = model.CreateResource<Association>(ModelProvider.CreateUri<Association>());
                        association.Agent = new Agent(new UriRef(a.agent));
                        association.Role = new Role(new UriRef(a.role));
                        association.Commit();

                        activity.Associations.Add(association);
                    }
                }

                activity.Commit();

                // Return the URI to the frontend.
                var data = new { uri = request.Uri };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", primarySource);

                return HttpStatusCode.BadRequest;
            }
        }

        private Response PostResponse<T>(CommentParameter parameter) where T : FeedbackResponse
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            var agent = new Agent(new UriRef(parameter.agent));
            var primarySource = new Entity(new UriRef(parameter.primarySource));

            if (model.ContainsResource(primarySource))
            {
                // Check if there is already a response from the agent.
                ISparqlQuery query = new SparqlQuery(@"
                    ASK WHERE { ?activity prov:hadAgent @agent ; prov:generated / prov:hadPrimarySource @request . }
                ");

                query.Bind("@agent", agent);
                query.Bind("@request", primarySource);

                ISparqlQueryResult result = model.ExecuteQuery(query);

                if (!result.GetAnwser())
                {
                    var request = model.CreateResource<T>(ModelProvider.CreateUri<T>());

                    request.CreationTimeUtc = parameter.endTime;
                    request.PrimarySource = primarySource;
                    request.Message = parameter.message;
                    request.IsSynchronizable = true;
                    request.Commit();

                    CreateEntity activity = model.CreateResource<CreateEntity>(ModelProvider.CreateUri<CreateEntity>());
                    activity.StartedBy = new Agent(new UriRef(parameter.agent));
                    activity.StartTimeUtc = parameter.startTime;
                    activity.EndTimeUtc = parameter.endTime;
                    activity.GeneratedEntities.Add(request);
                    activity.UsedEntities.Add(primarySource);

                    if (parameter.marks != null)
                    {
                        foreach (var mark in parameter.marks)
                        {
                            activity.UsedEntities.Add(new Mark(new UriRef(mark)));
                        }
                    }

                    if (parameter.associations != null)
                    {
                        foreach (AssociationParameter a in parameter.associations)
                        {
                            Association association = model.CreateResource<Association>(ModelProvider.CreateUri<Association>());
                            association.Agent = new Agent(new UriRef(a.agent));
                            association.Role = new Role(new UriRef(a.role));
                            association.Commit();

                            activity.Associations.Add(association);
                        }
                    }

                    activity.Commit();

                    // Return the URI to the frontend.
                    var data = new { uri = request.Uri };

                    return Response.AsJsonSync(data, HttpStatusCode.OK);
                }
                else
                {
                    return HttpStatusCode.NotModified;
                }
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", primarySource);

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

        private class CommentParameter : IValidatable
        {
            #region Members

            public CommentTypes type { get; set; }

            public string agent { get; set; }

            public string primarySource { get; set; }

            public DateTime startTime { get; set; }

            public DateTime endTime { get; set; }

            public string message { get; set; }

            public string[] marks { get; set; }

            public List<AssociationParameter> associations { get; set; }

            #endregion

            #region Constructors

            public CommentParameter()
            {
                associations = new List<AssociationParameter>();
            }

            #endregion

            #region Methods

            public bool Validate() {
                return !string.IsNullOrEmpty(message)
                    && !string.IsNullOrEmpty(agent)
                    && !string.IsNullOrEmpty(primarySource)
                    && DateTime.MinValue < startTime
                    && startTime <= endTime;
            }

            #endregion
        }

        private class AssociationParameter
        {
            #region Members

            public string agent { get; set; }

            public string role { get; set; }

            #endregion
        }

        private enum CommentTypes
        {
            Comment,
            FeedbackRequest,
            FeedbackResponse,
            ApprovalRequest,
            ApprovalResponse
        };

        #endregion
    }
}
