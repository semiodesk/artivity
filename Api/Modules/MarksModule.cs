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
    public class MarksModule : ModuleBase
    {
        #region Constructors

        public MarksModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/marks", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                InitializeRequest();

                string uri = Request.Query.entityUri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetMarksFromEntity(new UriRef(uri));
            };

            Post["/"] = parameters =>
            {
                InitializeRequest();

                MarkParameter mark = this.Bind<MarkParameter>();

                return PostMark(mark);
            };

            Put["/"] = parameters =>
            {
                InitializeRequest();

                MarkParameter mark = this.Bind<MarkParameter>();

                return PutMark(mark);
            };

            Delete["/"] = parameters =>
            {
                InitializeRequest();

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
                    ?geometryType
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
                    art:region ?region .

                    ?region a ?geometryType ;
                        art:x ?x ;
                        art:y ?y .

                    OPTIONAL
                    {
                        ?region a art:Rectangle ;
                            art:width ?w ;
                            art:height ?h .
                    }
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@undefined", DateTime.MinValue);

            var result = model.GetBindings(query).ToList();

            return Response.AsJson(result);
        }

        private Response PostMark(MarkParameter parameter)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            Agent agent = new Agent(new UriRef(parameter.agent));
            Entity entity = new Entity(new UriRef(parameter.entity)); // TODO: This influence needs to be an entity which was derived from the original.

            if (model.ContainsResource(entity))
            {
                Geometry region = TryCreateMarkGeometry(model, parameter);

                if(region != null)
                {
                    Mark mark = model.CreateResource<Mark>(ModelProvider.CreateUri<Mark>());
                    mark.IsSynchronizable = true;
                    mark.CreationTimeUtc = parameter.endTime;
                    mark.PrimarySource = entity;
                    mark.Region = region;
                    mark.Commit();

                    // Associate the comment with the activity.
                    CreateEntity activity = model.CreateResource<CreateEntity>(ModelProvider.CreateUri<CreateEntity>());
                    activity.StartedBy = agent;
                    activity.StartTimeUtc = parameter.startTime;
                    activity.EndTimeUtc = parameter.endTime;
                    activity.GeneratedEntities.Add(mark);
                    activity.UsedEntities.Add(entity); // TODO: Correct this in the data provided by the plugins.
                    activity.Commit();

                    // Return the URI to the frontend.
                    var data = new { uri = mark.Uri };

                    return Response.AsJsonSync(data, HttpStatusCode.OK);
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            }
            else
            {
                PlatformProvider.Logger.LogError("Model does not contain entity {0}", entity);

                return HttpStatusCode.BadRequest;
            }
        }

        private Geometry TryCreateMarkGeometry(IModel model, MarkParameter parameter)
        {
            switch(parameter.geometryType)
            {
                case ART.Point:
                {
                    Point point = model.CreateResource<Point>(ModelProvider.CreateUri<Point>());
                    point.X = parameter.geometry.x;
                    point.Y = parameter.geometry.y;
                    point.Commit();

                    return point;
                }
                case ART.Rectangle:
                {
                    Rectangle rectangle = model.CreateResource<Rectangle>(ModelProvider.CreateUri<Rectangle>());
                    rectangle.X = parameter.geometry.x;
                    rectangle.Y = parameter.geometry.y;
                    rectangle.Width = parameter.geometry.width;
                    rectangle.Height = parameter.geometry.height;
                    rectangle.Commit();

                    return rectangle;
                }
            }

            return null;
        }

        private Response PutMark(MarkParameter parameter)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            UriRef uri = new UriRef(parameter.uri);

            if (model.ContainsResource(uri))
            {
                Mark mark = model.GetResource<Mark>(uri);

                if(TryUpdateMarkGeometry(model, parameter, mark.Region.Uri))
                {
                    // Update the mark timestamps.
                    mark.Commit();

                    EditEntity activity = model.CreateResource<EditEntity>(ModelProvider.CreateUri<EditEntity>());
                    activity.StartedBy = new Agent(new UriRef(parameter.agent));
                    activity.StartTimeUtc = DateTime.UtcNow;
                    activity.EndTimeUtc = DateTime.UtcNow;
                    activity.UsedEntities.Add(mark);
                    activity.Commit();

                    return HttpStatusCode.OK;
                }
            }

            return HttpStatusCode.NotModified;
        }

        private bool TryUpdateMarkGeometry(IModel model, MarkParameter parameter, UriRef uri)
        {
            switch (parameter.geometryType)
            {
                case ART.Point:
                {
                    Point point = model.GetResource<Point>(uri);
                    point.X = parameter.geometry.x;
                    point.Y = parameter.geometry.y;
                    point.Commit();

                    return true;
                }
                case ART.Rectangle:
                {
                    Rectangle rectangle = model.GetResource<Rectangle>(uri);
                    rectangle.X = parameter.geometry.x;
                    rectangle.Y = parameter.geometry.y;
                    rectangle.Width = parameter.geometry.width;
                    rectangle.Height = parameter.geometry.height;
                    rectangle.Commit();

                    return true;
                }
            }

            return false;
        }

        private Response DeleteMark(UriRef uri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            if(model.ContainsResource(uri))
            {
                Mark mark = model.GetResource<Mark>(uri);
                mark.DeletionTimeUtc = DateTime.UtcNow;
                mark.Commit();

                DeleteEntity activity = model.CreateResource<DeleteEntity>(ModelProvider.CreateUri<DeleteEntity>());
                activity.StartTimeUtc = DateTime.UtcNow;
                activity.EndTimeUtc = DateTime.UtcNow;
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
