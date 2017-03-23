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
using Artivity.DataModel.Tasks;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy.ModelBinding;

namespace Artivity.Api.Modules
{
    public class TasksModule : ModuleBase
    {
        #region Constructors

        public TasksModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/tasks", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                string uri = Request.Query.entityUri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetTasksFromEntity(new UriRef(uri));
            };

            Post["/"] = parameters =>
            {
                TaskParameter task = this.Bind<TaskParameter>();

                if (!string.IsNullOrEmpty(task.name))
                {
                    return PostTask(task);
                }

                return HttpStatusCode.BadRequest;
            };

            Put["/"] = parameters =>
            {
                TaskParameter task = this.Bind<TaskParameter>();

                if (!string.IsNullOrEmpty(task.name))
                {
                    return PutTask(task);
                }

                return HttpStatusCode.BadRequest;
            };
        }

        #endregion

        #region Methods

        private Response GetTasksFromEntity(UriRef entityUri)
        {
            LoadCurrentUser();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri
                    ?agent
                    ?time
                    ?name
                    ?completed
                WHERE
                {
                    ?activity prov:generated | prov:used ?uri ;
                        prov:wasStartedBy ?agent .

                    ?uri a tmo:Task ;
                        prov:hadPrimarySource @entity ;
                        nie:created ?time ;
                        art:deleted @undefined ;
                        tmo:taskName ?name ;
                        tmo:taskState ?state .

                    BIND(EXISTS { ?uri tmo:taskState tmo:TMO_Instance_TaskState_Completed } AS ?completed)
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            var bindings = UserModel.GetBindings(query, true).ToList();

            return Response.AsJson(bindings);
        }

        private Response PostTask(TaskParameter parameter)
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
                Task task = UserModel.CreateResource<Task>();
                task.CreationTimeUtc = parameter.endTime;
                task.PrimarySource = entity; // TODO: Correct this in the data provided by the plugins.
                task.Name = parameter.name;
                task.IsCompleted = parameter.completed;
                task.Commit();

                CreateEntity activity = UserModel.CreateResource<CreateEntity>();
                activity.StartedBy = agent;
                activity.StartTime = parameter.startTime;
                activity.EndTime = parameter.endTime;
                activity.GeneratedEntities.Add(task); // Associate the comment with the activity.
                activity.UsedEntities.Add(entity); // TODO: Correct this in the data provided by the plugins.

                activity.Commit();

                // Return the URI to the frontend.
                var data = new { uri = task.Uri };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", entity);

                return HttpStatusCode.BadRequest;
            }
        }

        private Response PutTask(TaskParameter parameter)
        {
            LoadCurrentUser();

            UriRef uri = new UriRef(parameter.uri);

            if (UserModel.ContainsResource(uri))
            {
                Task task = UserModel.GetResource<Task>(uri);
                task.IsCompleted = parameter.completed;
                task.Name = parameter.name;
                task.Commit();

                EditEntity activity = UserModel.CreateResource<EditEntity>();
                activity.StartedBy = new Agent(new UriRef(parameter.agent));
                activity.StartTime = DateTime.UtcNow;
                activity.EndTime = DateTime.UtcNow;
                activity.UsedEntities.Add(task);
                activity.Commit();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        #endregion
    }
}
