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
    public class MarkModule : ModuleBase
    {
        #region Constructors

        public MarkModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/marks", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                string uri = Request.Query.entityUri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetMarksFromEntity(new UriRef(uri));
            };

            Post["/"] = parameters =>
            {
                MarkParameter mark = this.Bind<MarkParameter>();

                return PostMark(mark);
            };

            Put["/"] = parameters =>
            {
                MarkParameter mark = this.Bind<MarkParameter>();

                return PutMark(mark);
            };

            Delete["/"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return DeleteMark(new UriRef(uri));
            };
        }

        #endregion

        #region Methods

        private Response GetMarksFromEntity(UriRef entityUri)
        {
            LoadCurrentUser();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri
                    ?agent
                    ?time
                    ?x
                    ?y
                    ?w
                    ?h
                WHERE
                {
                  ?activity prov:generated ?uri ;
                        prov:wasStartedBy ?agent .

                  ?uri a art:Mark ;
                    prov:hadPrimarySource @entity ;
                    nie:created ?time ;
                    art:deleted @undefined ;
                    art:region [
                        art:x ?x;
                        art:y ?y;
                        art:width ?w;
                        art:height ?h
                    ].
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            var result = UserModel.GetBindings(query).ToList();

            return Response.AsJson(result);
        }

        private Response PostMark(MarkParameter parameter)
        {
            LoadCurrentUser();

            Agent agent = new Agent(new UriRef(parameter.agent));
            Entity entity = new Entity(new UriRef(parameter.entity)); // TODO: This influence needs to be an entity which was derived from the original.

            if (UserModel.ContainsResource(entity))
            {
                // TODO: Move the creation of the entity into API for markers.
                Rectangle rectangle = UserModel.CreateResource<Rectangle>();
                rectangle.x = parameter.x;
                rectangle.y = parameter.y;
                rectangle.Width = parameter.width;
                rectangle.Height = parameter.height;
                rectangle.Commit();

                Mark mark = UserModel.CreateResource<Mark>();
                mark.CreationTimeUtc = parameter.endTime;
                mark.PrimarySource = entity; // TODO: Correct this in the data provided by the plugins.
                mark.Region = rectangle;
                mark.Commit();

                // Associate the comment with the activity.
                CreateEntity activity = UserModel.CreateResource<CreateEntity>();
                activity.StartedBy = agent;
                activity.StartTime = parameter.startTime;
                activity.EndTime = parameter.endTime;
                activity.GeneratedEntities.Add(mark);
                activity.UsedEntities.Add(entity); // TODO: Correct this in the data provided by the plugins.
                activity.Commit();

                // Return the URI to the frontend.
                var data = new { uri = mark.Uri };

                return Response.AsJsonSync(data, HttpStatusCode.OK);
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", entity);

                return HttpStatusCode.BadRequest;
            }
        }

        private Response PutMark(MarkParameter parameter)
        {
            LoadCurrentUser();

            UriRef uri = new UriRef(parameter.uri);

            if (UserModel.ContainsResource(uri))
            {
                Mark mark = UserModel.GetResource<Mark>(uri);
                mark.Commit();

                Rectangle rectangle = mark.Region;
                rectangle.x = parameter.x;
                rectangle.y = parameter.y;
                rectangle.Width = parameter.width;
                rectangle.Height = parameter.height;
                rectangle.Commit();

                EditEntity activity = UserModel.CreateResource<EditEntity>();
                activity.StartedBy = new Agent(new UriRef(parameter.agent));
                activity.StartTime = DateTime.UtcNow;
                activity.EndTime = DateTime.UtcNow;
                activity.UsedEntities.Add(mark);
                activity.Commit();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        private Response DeleteMark(UriRef uri)
        {
            LoadCurrentUser();

            if(UserModel.ContainsResource(uri))
            {
                Mark mark = UserModel.GetResource<Mark>(uri);
                mark.DeletionTimeUtc = DateTime.UtcNow;
                mark.Commit();

                DeleteEntity activity = UserModel.CreateResource<DeleteEntity>();
                activity.StartTime = DateTime.UtcNow;
                activity.EndTime = DateTime.UtcNow;
                activity.InvalidatedEntities.Add(mark);
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
