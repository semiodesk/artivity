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
    public class CommentModule : ModuleBase
    {
        #region Members

        #endregion

        public CommentModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/entity/derivations", modelProvider, platformProvider)
        {
            Get["/comments"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetInfluenceComments(new UriRef(uri));
            };

            Get["/comments/clean"] = parameters =>
            {
                return CleanComments();
            };

            Post["/comments"] = parameters =>
            {
                CommentCollection comment = this.Bind<CommentCollection>();

                if (comment.Comments.Length == 0)
                {
                    return HttpStatusCode.BadRequest;
                }
                return PostComment(comment);

            };
        }

        private Response GetInfluenceComments(UriRef uriRef)
        {
            LoadCurrentUser();

            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE ?comment
                WHERE
                {
                  
                  ?comment
                    a art:Comment ;
                    prov:atTime ?time ; 
                    art:refersTo @uri ;
                    prov:hadActivity ?act .

                  ?act prov:endedAtTime ?actTime .
                }
                ORDER BY DESC(?actTime) DESC(?time) ");

            query.Bind("@uri", uriRef);
            var res = UserModel.ExecuteQuery(query, true).GetResources<Comment>();
            var list = res.ToList();

            return Response.AsJsonSync(list);
        }


        private Response GetComments(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?time
                    ?activity
                    ?comment
                    ?agent
                    ?message
                WHERE
                {
                  ?activity prov:generated | prov:used @entity .

                  ?comment
                    a art:Comment ;
                    rdfs:comment ?message ;
                    prov:activity ?activity ;
                    prov:atTime ?time ;
                    nao:creator ?agent .
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.GetAll().GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        private Response CleanComments()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?comment
                WHERE
                {
                    ?comment a art:Comment ; prov:activity ?activity .

                    FILTER NOT EXISTS { ?comment rdfs:comment ?value . }
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

        private UriRef GetAgentUri() { return new UriRef(""); }

        private Response PostComment(CommentCollection parameter)
        {
            LoadCurrentUser();

            if (!Uri.IsWellFormedUriString(parameter.entity, UriKind.Absolute))
            {
                PlatformProvider.Logger.LogError("Invalid URI for parameter 'entity': {0}", parameter.entity);

                return HttpStatusCode.BadRequest;
            }

            if (!Uri.IsWellFormedUriString(parameter.influence, UriKind.Absolute))
            {
                PlatformProvider.Logger.LogError("Invalid URI for parameter 'influence': {0}", parameter.influence);

                return HttpStatusCode.BadRequest;
            }

            UriRef entityUri = new UriRef(parameter.entity);
            UriRef influenceUri = new UriRef(parameter.influence);

            //UriRef agentUri = GetAgentUri();

            if (UserModel.ContainsResource(entityUri) && UserModel.ContainsResource(influenceUri))
            {
                var influence = UserModel.GetResource<EntityInfluence>(influenceUri);

                LeaveComment activity = UserModel.CreateResource<LeaveComment>();
                activity.StartTime = parameter.startTime;
                activity.EndTime = parameter.endTime;
                activity.UsedEntities.Add(new Entity(entityUri));
                //activity.StartedBy = new Agent(agentUri);

                foreach (var c in parameter.Comments)
                {
                    Comment comment = UserModel.CreateResource<Comment>();
                    comment.Activity_ = activity;
                    comment.RefersTo_ = influence;
                    comment.Message = c.text;
                    comment.Time = c.time;

                    foreach (var marker in c.marker)
                    {
                        RectangleEntity rect = UserModel.CreateResource<RectangleEntity>();
                        rect.x = marker.x;
                        rect.y = marker.y;
                        rect.Width = marker.width;
                        rect.Height = marker.height;
                        rect.Commit();
                        comment.Regions.Add(rect);
                        activity.GeneratedEntities.Add(rect);
                    }
                    comment.Commit();
                    activity.GeneratedEntities.Add(comment);
                }


                activity.Commit();

                return HttpStatusCode.OK;
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", entityUri);

                return HttpStatusCode.BadRequest;
            }
        }
    }
}
