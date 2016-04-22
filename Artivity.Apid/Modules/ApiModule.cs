using Artivity.DataModel;
using Artivity.DataModel.Journal;
using Nancy;
using Nancy.Responses;
using Nancy.ModelBinding;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Artivity.Apid.Parameters;
using Newtonsoft.Json;

namespace Artivity.Apid.Modules
{
    public class ApiModule : ModuleBase
    {
        #region Constructors

        public ApiModule(IModelProvider provider)
            : base("/artivity/1.0/api", provider)
        {
            ModelProvider = provider;

            Get["/agents"] = paramters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if(string.IsNullOrEmpty(fileUrl))
                {
                    return GetAgents();
                }
                else
                {
                    return GetAgent(fileUrl);
                }
            };

            Get["/agents/user"] = parameters =>
            {
                return GetUserAgent();
            };

            Post["/agents/user"] = parameters =>
            {
                Person user = Bind<Person>(ModelProvider.Store, Request.Body);
                user.Commit();

                return HttpStatusCode.OK;
            };

            Get["/agents/user/photo"] = parameters =>
            {
                return GetUserAgentPhoto();
            };

            Get["/files/recent"] = parameters =>
            {
                return GetRecentlyUsedFiles();
            };

            Get["/activities"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                return GetActivities(fileUrl);
            };

            Get["/activities/test"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                return GetActivitiesTest(fileUrl);
            };

            Get["/influences"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                return GetInfluences(fileUrl);
            };
        }

        #endregion

        #region Methods

        public Response GetAgents()
        {
            IModel model = ModelProvider.GetAgents();

            IEnumerable<SoftwareAgent> agents = model.GetResources<SoftwareAgent>();

            return Response.AsJson(agents);
        }

        public Response GetAgent(string fileUrl)
        {
            IModel model = ModelProvider.GetAllActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?s ?p ?o WHERE 
                {
                    ?s ?p ?o .

                    {
                        SELECT ?s WHERE
                        {
                            ?file nfo:fileUrl @fileUrl .

                            ?activity prov:used ?file;
                                prov:qualifiedAssociation ?association .

                            ?association prov:agent ?s .

                            ?s a prov:SoftwareAgent .
                        }
                        LIMIT 1
                    }
                }");

            query.Bind("@fileUrl", fileUrl);

            return Response.AsJson(model.GetResources(query).FirstOrDefault());
        }

        public Response GetUserAgentPhoto()
        {
            Person result = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

            if(result != null && File.Exists(result.Photo))
            {
                FileStream file = new FileStream(result.Photo, FileMode.Open);

                StreamResponse response = new StreamResponse(() => file, MimeTypes.GetMimeType(result.Photo));
                  
                return response.AsAttachment(result.Photo);
            }
            else
            {
                return null;
            }
        }

        public Response GetUserAgent()
        {
            Person result = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

            if (result != null)
            {
                return Response.AsJson(result);
            }
            else
            {
                return null;
            }
        }

        public Response GetRecentlyUsedFiles()
        {
            IModel model = ModelProvider.ActivitiesModel;

            ISparqlQuery query = new SparqlQuery(@"
                SELECT MAX(?startTime) as ?time ?uri ?url ?agent WHERE
                {
                    ?activity prov:used ?uri ;
                        prov:startedAtTime ?startTime ;
                        prov:qualifiedAssociation ?association .

                    ?association prov:agent ?agent .

                    ?uri nfo:fileUrl ?url .
                }
                ORDER BY DESC(?time) LIMIT 25");

            return Response.AsJson(model.GetBindings(query));
        }

        public Response GetActivities(string fileUrl)
        {
            IModel model = ModelProvider.GetAll();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?startTime ?endTime ?color ?name WHERE
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file ;
                        prov:qualifiedAssociation ?association ;
                        prov:startedAtTime ?startTime ;
                        prov:endedAtTime ?endTime .

                    ?association prov:agent ?agent .

                    ?agent foaf:name ?name ;
                        art:hasColourCode ?color .
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@fileUrl", fileUrl);

            return Response.AsJson(model.GetBindings(query));
        }

        public Response GetActivitiesTest(string fileUrl)
        {
            IModel model = ModelProvider.GetAll();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?duration ?color ?name WHERE
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file ;
                        prov:qualifiedAssociation ?association ;
                        prov:startedAtTime ?startTime ;
                        prov:endedAtTime ?endTime .

                    BIND(?endTime - ?startTime as ?duration)

                    ?association prov:agent ?agent .

                    ?agent foaf:name ?name ;
                        art:hasColourCode ?color .
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@fileUrl", fileUrl);

            ISparqlQueryResult result = model.ExecuteQuery(query);

            return Response.AsJson(result.GetBindings());
        }

        public Response GetInfluences(string fileUrl)
        {
            IModel model = ModelProvider.GetAllActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?time ?uri ?type ?color ?description ?bounds WHERE 
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file;
                        prov:qualifiedAssociation ?association ;
                        prov:generated ?entity .

                    ?association prov:agent ?agent .

                    ?agent art:hasColourCode ?color .

                    ?entity prov:qualifiedGeneration ?generation .

                    ?generation a ?type ;
                        prov:atTime ?time .

                    OPTIONAL { ?generation dces:description ?description . }
                    OPTIONAL { ?generation art:hadBoundaries ?bounds . }
                }
                ORDER BY (?time)");

            query.Bind("@fileUrl", fileUrl);

            return Response.AsJson(model.GetBindings(query));
        }

        #endregion
    }
}
