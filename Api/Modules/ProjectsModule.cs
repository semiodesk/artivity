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
using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Nancy.ModelBinding;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artivity.Api.Modules
{
    public class ProjectsModule : ModuleBase
    {
        public ProjectsModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/projects", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                Response response = InitializeRequest();

                if (response != null)
                {
                    return response;
                }

                IModel model = ModelProvider.GetActivities();

                if (model == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetEntities<Project>(model, art.Project.Uri);
            };
            

            // Create a new resource
            Get["/new"] = parameters =>
            {
                Response response = InitializeRequest();

                if (response != null)
                {
                    return response;
                }

                IModel model = ModelProvider.GetActivities();

                if (model == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                Uri uri = modelProvider.CreateUri<Project>();

                Project entity = model.CreateResource<Project>(uri);

                return Response.AsJsonSync(entity);
            };
            

            Put["/"] = parameters =>
            {
                Response response = InitializeRequest();

                if (response != null)
                {
                    return response;
                }

                IModel model = ModelProvider.GetActivities();

                if (model == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                Uri uri = new Uri(Request.Query["uri"]);

                ProjectParameter param = this.Bind<ProjectParameter>();

                PutProject(uri, param);

                return Response.AsJsonSync(new { success = true });
            };
            
            Delete["/"] = parameters =>
            {
                Response response = InitializeRequest();

                if (response != null)
                {
                    return response;
                }

                Uri uri = new Uri(Request.Query["uri"]);

                return DeleteProject(uri);
            };
            
            Get["/agents"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);

                    if (string.IsNullOrEmpty(Request.Query.q))
                    {
                        return GetProjectMembers(projectUri);
                    }
                    else
                    {
                        return GetProjectMembersFromQuery(projectUri, Request.Query.q);
                    }
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Post["/agents"] = parameters =>
            {
                InitializeRequest();

                UserRoles role = UserRoles.ProjectMemberRole;

                if (IsUri(Request.Query.projectUri) && IsUri(Request.Query.agentUri) && Enum.TryParse(Request.Query.role, out role))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return PostProjectMemberWithRole(projectUri, agentUri, role);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Delete["/agents"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri) && IsUri(Request.Query.agentUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return DeleteProjectMember(projectUri, agentUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Get["/files"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);

                    return GetProjectFiles(projectUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Post["/files"] = parameters =>
            {
                InitializeRequest();

                if(IsUri(Request.Query.projectUri) && IsUri(Request.Query.fileUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef fileUri = new UriRef(Request.Query.fileUri);

                    return PostProjectFile(projectUri, fileUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Delete["/files"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri) && IsUri(Request.Query.fileUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef fileUri = new UriRef(Request.Query.fileUri);

                    return DeleteProjetFile(projectUri, fileUri);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Get["/folders"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);

                    return GetProjectFolders(projectUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Post["/folders"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri) && IsUri(Request.Query.folderUrl))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef folderUrl = new UriRef(Request.Query.folderUrl);

                    return PostProjectFolder(projectUri, folderUrl);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Delete["/folders"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.projectUri) && IsUri(Request.Query.folderUrl))
                {
                    UriRef projectUri = new UriRef(Request.Query.projectUri);
                    UriRef folderUrl = new UriRef(Request.Query.folderUrl);

                    return DeleteProjectFolder(projectUri, folderUrl);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };
        }

        #region Methods


        private Response PutProject(Uri uri, ProjectParameter parameter)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            if (uri.AbsoluteUri != parameter.uri)
            {
                return HttpStatusCode.BadRequest;
            }

            Project project;

            if (model.ContainsResource(uri))
            {
                project = model.GetResource<Project>(uri);
            }
            else
            {
                project = model.CreateResource<Project>(uri);
            }

            project.Title = parameter.title;
            project.ColorCode = parameter.colorCode;
            project.Description = parameter.description;
            project.StartTimeUtc = DateTime.UtcNow;
            project.Commit();

            return HttpStatusCode.OK;
        }

        protected Response GetProjectMembers(UriRef projectUri)
        {
            IModel all = ModelProvider.GetAll();

            if(all.ContainsResource(projectUri))
            {
                ISparqlQuery query = new SparqlQuery(@"
                    SELECT ?s ?p ?o
                    WHERE
                    {
                        @project art:qualifiedMembership ?s .

                        ?s a art:ProjectMembership ; ?p ?o .
                    }");

                query.Bind("@project", projectUri);

                List<ProjectMembership> members = all.GetResources<ProjectMembership>(query).ToList();

                return Response.AsJsonSync(members);
            }
            else
            {
                return HttpStatusCode.NoContent;
            }
        }

        protected Response GetProjectMembersFromQuery(UriRef projectUri, string q)
        {
            IModel all = ModelProvider.GetAll();

            if (all.ContainsResource(projectUri))
            {
                ISparqlQuery query = new SparqlQuery(@"
                    SELECT ?s ?p ?o
                    WHERE
                    {
                        @project art:qualifiedMembership ?s .

                        ?s a art:ProjectMembership ; ?p ?o .

                        ?s art:agent / foaf:name ?name .

                        FILTER(STRSTARTS(LCASE(?name), @q))
                    }");

                query.Bind("@project", projectUri);
                query.Bind("@q", q.ToLowerInvariant());

                List<ProjectMembership> members = all.GetResources<ProjectMembership>(query).ToList();

                return Response.AsJsonSync(members);
            }
            else
            {
                return HttpStatusCode.NoContent;
            }
        }

        protected Response PostProjectMemberWithRole(UriRef projectUri, UriRef agentUri, UserRoles userRole)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"ASK WHERE { @project art:qualifiedMembership / art:agent @agent }");
            query.Bind("@project", projectUri);
            query.Bind("@agent", agentUri);

            ISparqlQueryResult result = activities.ExecuteQuery(query);

            if (!result.GetAnwser())
            {
                Resource role;

                switch (userRole)
                {
                    case UserRoles.ProjectAdministratorRole: role = art.ProjectAdministratorRole; break;
                    case UserRoles.ProjectMemberRole: role = art.ProjectMemberRole; break;
                    default: throw new ArgumentException("userRole");
                }

                ProjectMembership membership = activities.CreateResource<ProjectMembership>(ModelProvider.CreateUri<ProjectMembership>());
                membership.Agent = new Person(agentUri);
                membership.Role = new Role(role.Uri);
                membership.Commit();

                Project project = activities.GetResource<Project>(projectUri);
                project.Memberships.Add(membership);
                project.Commit();

                return Response.AsJsonSync(new { success = true });
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        protected Response DeleteProjectMember(UriRef projectUri, UriRef agentUri)
        {
            IModel activities = ModelProvider.GetActivities();

            if (activities.ContainsResource(projectUri))
            {
                Project project = activities.GetResource<Project>(projectUri);

                if (project.Memberships.Any(m => m.Agent.Uri == agentUri))
                {
                    project.Memberships.RemoveAll(m => m.Agent.Uri == agentUri);
                    project.Commit();

                    return HttpStatusCode.OK;
                }

                return HttpStatusCode.NotModified;
            }
            else
            {
                return HttpStatusCode.NoContent;
            }
        }

        private Response GetProjectFiles(UriRef projectUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
	                ?t1 AS ?time
                    ?entityUri
	                ?file AS ?uri
	                ?label
                    SAMPLE(?p) AS ?thumbnail 
                    COALESCE(?agentColor, '#cecece') AS ?agentColor
                WHERE
                {
                    ?a1 prov:generated | prov:used ?entity ;
                        prov:endedAtTime ?t1 .

	                ?entity nie:isStoredAs ?file .

                    @project prov:qualifiedUsage / prov:entity ?file .

                    ?file a nfo:FileDataObject .
                    ?file rdfs:label ?label .

                    BIND(STRBEFORE(STR(?entity), '#') AS ?e).
                    BIND(IF(?e != '', ?e, STR(?entity)) AS ?entityUri).
                    " + ModelProvider.GetFilesQueryModifier + @"
	
	                OPTIONAL
	                {
                        ?a2 prov:generated | prov:used ?entity ;
                            prov:endedAtTime ?t2 .
		
		                FILTER(?t1 > ?t2)
	                }
                    FILTER NOT EXISTS
                    {
						?older prov:qualifiedRevision / prov:entity ?entity.
                    }

	                FILTER(!BOUND(?t2))

                    OPTIONAL {
                        ?a1 prov:qualifiedAssociation / prov:agent ?agent .

                        ?agent a prov:SoftwareAgent.
                        ?agent art:hasColourCode ?agentColor .
                    }
                }
                ORDER BY DESC(?t1) LIMIT 100");

            query.Bind("@project", projectUri);

            var bindings = ModelProvider.GetAll().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        protected Response PostProjectFile(UriRef projectUri, UriRef fileUri)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"ASK WHERE { @project prov:qualifiedUsage / prov:entity @file }");
            query.Bind("@project", projectUri);
            query.Bind("@file", fileUri);

            ISparqlQueryResult result = activities.ExecuteQuery(query);

            if(!result.GetAnwser())
            {
                Usage usage = activities.CreateResource<Usage>(ModelProvider.CreateUri<Usage>());
                usage.Entity = new Entity(fileUri);
                usage.Time = DateTime.UtcNow;
                usage.Commit();

                Project project = activities.GetResource<Project>(projectUri);
                project.Usages.Add(usage);
                project.Commit();

                return Response.AsJsonSync(new { success = true });
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        protected Response DeleteProjetFile(UriRef projectUri, UriRef fileUri)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"ASK WHERE { @project prov:qualifiedUsage / prov:entity @file }");
            query.Bind("@project", projectUri);
            query.Bind("@file", fileUri);

            ISparqlQueryResult result = activities.ExecuteQuery(query);

            if (result.GetAnwser())
            {
                Project project = activities.GetResource<Project>(projectUri);
                project.Usages.RemoveAll(u => u.Entity.Uri == fileUri);
                project.Commit();

                return Response.AsJsonSync(new { success = true });
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        protected Response GetProjectFolders(UriRef projectUri)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT ?s ?p ?o
                WHERE
                {
                    @project prov:qualifiedUsage / prov:entity ?s .

                    ?s a nfo:Folder ; ?p ?o .
                }");

            query.Bind("@project", projectUri);

            List<Folder> folders = activities.GetResources<Folder>(query).ToList();

            return Response.AsJsonSync(folders);
        }

        protected Response PostProjectFolder(UriRef projectUri, UriRef folderUrl)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"SELECT ?folder WHERE { ?folder a nfo:Folder ; nie:url @url . }");
            query.Bind("@url", folderUrl);

            IEnumerable<BindingSet> bindings = activities.GetBindings(query);

            Folder folder;

            if(bindings.Any())
            {
                folder = new Folder(new Uri(bindings.First()["folder"].ToString()));
            }
            else
            {
                folder = activities.CreateResource<Folder>(ModelProvider.CreateUri<Folder>());
                folder.Url = new Resource(folderUrl);
                folder.Commit();
            }

            Usage usage = activities.CreateResource<Usage>(ModelProvider.CreateUri<Usage>());
            usage.Entity = folder;
            usage.Time = DateTime.UtcNow;
            usage.Commit();

            Project project = activities.GetResource<Project>(projectUri);
            project.Usages.Add(usage);
            project.Commit();

            return HttpStatusCode.OK;
        }

        protected Response DeleteProjectFolder(UriRef projectUri, UriRef folderUrl)
        {
            IModel activities = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"ASK WHERE { @project prov:qualifiedUsage / prov:entity [ nie:url @url ] }");
            query.Bind("@project", projectUri);
            query.Bind("@url", folderUrl);

            ISparqlQueryResult result = activities.ExecuteQuery(query);

            if (result.GetAnwser())
            {
                Project project = activities.GetResource<Project>(projectUri);
                project.Usages.RemoveAll(u => u.Entity is Folder && (u.Entity as Folder).Url.Uri == folderUrl);
                project.Commit();

                return Response.AsJsonSync(new { success = true });
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        private Response DeleteProject(Uri uri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            SparqlUpdate update = new SparqlUpdate(@"
                        WITH @model
                        DELETE { @subject art:deleted ?deletionTime . }
                        WHERE 
                        {
                            @subject art:deleted ?deletionTime . 
                            @subject a art:Project .
                        }
                        INSERT { @subject art:deleted @deletionTime . }
                        DELETE { ?state arts:lastRemoteRevision ?revision . }
                        INSERT { ?state arts:lastRemoteRevision @undefined . }
                        WHERE
                        {
                            @subject arts:synchronizationState ?state .
                            ?state arts:lastRemoteRevision ?revision .
                            @subject a art:Project .
                        }
                    ");

            update.Bind("@model", model.Uri);
            update.Bind("@subject", uri);
            update.Bind("@deletionTime", DateTime.UtcNow);
            update.Bind("@undefined", -1);

            model.ExecuteUpdate(update);

            return Response.AsJsonSync(new { success = true });
        }
        #endregion
    }
}
