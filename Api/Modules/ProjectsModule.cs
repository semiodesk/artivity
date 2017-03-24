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

using Artivity.Api;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artivity.Api.Modules
{
    public class ProjectsModule : EntityModuleBase<Project>
    {
        public ProjectsModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/projects", modelProvider, platformProvider)
        {
            Get["/agents"] = parameters =>
            {
                return HttpStatusCode.NotImplemented;
            };

            Post["/agents"] = parameters =>
            {
                string projectUri = Request.Query.projectUri;
                string agentUri = Request.Query.agentUri;

                if (!IsUri(projectUri) || !IsUri(agentUri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PostAgent(projectUri, agentUri);
            };

            Get["/files"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetFilesFromProject(new UriRef(uri));
            };

            Post["/files"] = parameters =>
            {
                string projectUri = Request.Query.projectUri;
                string fileUri = Request.Query.fileUri;

                if(!IsUri(projectUri) || !IsUri(fileUri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PostFile(projectUri, fileUri);
            };

            Get["/folders"] = parameters =>
            {
                return HttpStatusCode.NotImplemented;
            };

            Post["/folders"] = parameters =>
            {
                string projectUri = Request.Query.projectUri;
                string folderUrl = Request.Query.folderUrl;

                if(!IsUri(projectUri) || !IsUri(folderUrl))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PostFolder(projectUri, folderUrl);
            };
        }

        #region Methods

        protected Response PostAgent(string projectUri, string agentUri)
        {
            /*
            IModel activities = ModelProvider.GetActivities();
            IModel agents = ModelProvider.GetAgents();

            Project project = activities.GetResource<Project>(new Uri(projectUri));
            Agent agent = agents.GetResource<Agent>(new Uri(agentUri));

            if (!project.Associations.Any(a => a.Agent == agent))
            {
                Association association = activities.CreateResource<Association>();
                association.Agent = agent;

                project.Associations.Add(association);
                project.Commit();
            }

            return Response.AsJsonSync(new Dictionary<string, bool> { { "success", true } });
            */

            return HttpStatusCode.NotImplemented;
        }

        protected Response PostFile(string projectUri, string fileUri)
        {
            /*
            IModel activities = ModelProvider.GetActivities();

            Project project = activities.GetResource<Project>(new Uri(projectUri));
            Entity entity = activities.GetResource<Entity>(new Uri(fileUri));

            if (!project.Usages.Contains(entity))
            {
                project.Usages.Add(entity);
                project.Commit();
            }

            return Response.AsJsonSync(new Dictionary<string, bool>{ {"success", true}});
            */

            return HttpStatusCode.NotImplemented;
        }

        protected Response PostFolder(string projectUri, string folderUrl)
        {
            /*
            IModel activities = ModelProvider.GetActivities();

            Project project = activities.GetResource<Project>(new Uri(projectUri));
            Folder folder = activities.GetResource<Folder>(new Uri(folderUrl));

            if (!project.Usages.Any(u => u.Entity is Folder))
            {
                project.Usages.Add(folder);
                project.Commit();
            }

            return Response.AsJsonSync(new Dictionary<string, bool> { { "success", true } });
            */

            return HttpStatusCode.NotImplemented;
        }

        private Response GetFilesFromProject(UriRef projectUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
	                ?t1 AS ?time
	                ?file AS ?uri
                    ?q as ?entityUri
                    SAMPLE(?p) as ?thumbnail 
	                ?label
                    SAMPLE(COALESCE(?agentColor, '#FF0000')) AS ?agentColor
                WHERE
                {
                    ?a1
                        prov:generated | prov:used ?entity ;
                        prov:endedAtTime ?t1 .

	                ?entity nie:isStoredAs ?file .
                    BIND( STRBEFORE( STR(?entity), '#' ) as ?e ).
                    BIND( if(?e != '', ?e, str(?entity)) as ?q).
                    " + PlatformProvider.GetFilesQueryModifier + @"
                    ?file rdfs:label ?label .

                    <" + projectUri.AbsoluteUri + @"> prov:hadMember ?file .
	
	                OPTIONAL
	                {
                        ?a2
                            prov:generated | prov:used ?entity ;
                            prov:endedAtTime ?t2 .
		
		                FILTER(?t1 > ?t2)
	                }
	
	                FILTER(!BOUND(?t2))

                    OPTIONAL
                    {
                        ?a1 prov:qualifiedAssociation / prov:agent / art:hasColourCode ?agentColor .
                    }
                }
                ORDER BY DESC(?t1) LIMIT 100");

            var bindings = ModelProvider.GetAll().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        #endregion
    }
}
