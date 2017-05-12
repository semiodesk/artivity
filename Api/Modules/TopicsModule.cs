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
    public class TopicsModule : ModuleBase
    {
        #region Constructors

        public TopicsModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/topics", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                InitializeRequest();

                if(IsUri(Request.Query.entityUri))
                {
                    UriRef entityUri = new UriRef(Request.Query.entityUri);

                    return GetTopicsFromEntity(entityUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Post["/"] = parameters =>
            {
                InitializeRequest();

                TopicParameter topic = this.Bind<TopicParameter>();

                if (!string.IsNullOrEmpty(topic.title))
                {
                    return PostTopic(topic);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Delete["/"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.topicUri))
                {
                    UriRef topicUri = new UriRef(Request.Query.topicUri);

                    return DeleteTopic(topicUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };
        }

        #endregion

        #region Methods

        private Response GetTopicsFromEntity(UriRef entityUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri
                    ?agent
                    ?time
                    ?title
                WHERE
                {
                    ?activity prov:generated ?uri ;
                        prov:wasStartedBy ?agent .

                    ?uri a art:Topic ;
                        prov:hadPrimarySource @entity ;
                        nie:created ?time ;
                        art:deleted @undefined ;
                        dces:title ?title .
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            var bindings = model.GetBindings(query, true).ToList();

            return Response.AsJson(bindings);
        }

        private Response PostTopic(TopicParameter parameter)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            if (!Uri.IsWellFormedUriString(parameter.primarySource, UriKind.Absolute))
            {
                PlatformProvider.Logger.LogError("Invalid URI for parameter 'entity': {0}", parameter.primarySource);

                return HttpStatusCode.BadRequest;
            }

            Agent agent = new Agent(new UriRef(parameter.agent));
            Entity primarySource = new Entity(new UriRef(parameter.primarySource));

            if (model.ContainsResource(primarySource))
            {
                Topic topic = model.CreateResource<Topic>(ModelProvider.CreateUri<Topic>());
                topic.CreationTimeUtc = parameter.endTime;
                topic.PrimarySource = primarySource; // TODO: Correct this in the data provided by the plugins.
                topic.Title = parameter.title;
                topic.IsSynchronizable = true;
                topic.Commit();

                CreateEntity activity = model.CreateResource<CreateEntity>(ModelProvider.CreateUri<CreateEntity>());
                activity.StartedBy = agent;
                activity.StartTimeUtc = parameter.startTime;
                activity.EndTimeUtc = parameter.endTime;
                activity.GeneratedEntities.Add(topic); // Associate the comment with the activity.
                activity.UsedEntities.Add(primarySource); // TODO: Correct this in the data provided by the plugins.

                activity.Commit();

                // Return the URI to the frontend.
                var data = new { uri = topic.Uri };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", primarySource);

                return HttpStatusCode.NotFound;
            }
        }

        private Response DeleteTopic(UriRef topicUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            if(model.ContainsResource(topicUri))
            {
                Topic topic = model.GetResource<Topic>(topicUri);
                topic.DeletionTimeUtc = DateTime.UtcNow;
                topic.Commit();

                DeleteEntity activity = model.CreateResource<DeleteEntity>(ModelProvider.CreateUri<DeleteEntity>());
                //activity.StartedBy = agent;
                activity.StartTimeUtc = DateTime.UtcNow;
                activity.EndTimeUtc = DateTime.UtcNow.AddSeconds(1);
                activity.InvalidatedEntities.Add(topic);

                if (topic.PrimarySource != null)
                {
                    activity.UsedEntities.Add(topic.PrimarySource);
                }

                activity.Commit();

                var data = new { success = true };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", topicUri);

                return HttpStatusCode.NotFound;
            }
        }

        #endregion
    }
}
