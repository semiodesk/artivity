using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artivity.DataModel.Journal;

namespace Artivity.Api.Http.Modules
{
    public class ApiModule : NancyModule
    {
        #region Members

        public IModelProvider ModelProvider { get; set; }

        #endregion

        #region Constructors

        public ApiModule(IModelProvider provider)
            : base("/artivity/1.0/api")
        {
            ModelProvider = provider;

            Get["/agents/user"] = parameters =>
            {
                return GetUserAgent();
            };

            Get["/agent/list"] = paramters =>
            {
                return Response.AsJson(ListAgents());
            };

            Get["/agent"] = paramters =>
            {
                string f = ((string)Request.Query["uri"]).Trim('"');
                return Response.AsJson(GetAgent(f));
            };

            Get["/files/recent"] = parameters =>
            {
                return GetRecentlyUsedFiles();
            };

            Get["/activities"] = parameters =>
            {
                return GetActivities();
            };
        }

        #endregion

        #region Methods

        public IEnumerable<SoftwareAgent> ListAgents()
        {
            IModel model = ModelProvider.GetAgents();
            return model.GetResources<SoftwareAgent>();
        }

        public SoftwareAgent GetAgent(string f)
        {
            IModel model = ModelProvider.GetAgents();

            return model.GetResource<SoftwareAgent>(new Uri(f));
        }

        public Response GetUserAgent()
        {
            var result = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

            if(result != null)
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
            var result = Journal.GetItems(ModelProvider.ActivitiesModel);

            return Response.AsJson(result);
        }

        public Response GetActivities()
        {
            string fileUrl = Request.Query["fileUrl"];

            var result = ActivityList.GetActivities(ModelProvider.GetAllActivities(), fileUrl);

            return Response.AsJson(result);
        }

        #endregion
    }
}
