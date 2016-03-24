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
        public IModelProvider ModelProvider { get; set; }

        public ApiModule(IModelProvider provider)
            : base("/artivity/1.0/api")
        {
            ModelProvider = provider;

            Get["/list"] = parameters =>
            {
                return Response.AsJson(ListJournalFiles());
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

            Get["/activity/list"] = parameters =>
            {
                string f = ((string)Request.Query["file"]).Trim('"');
                int qty = Request.Query["qty"];
                int idx = Request.Query["idx"];
                return Response.AsJson(ListActivity(f));
            };


        }

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


        public IEnumerable<JournalFile> ListJournalFiles()
        {
            return Journal.GetItems(ModelProvider.ActivitiesModel);
        }

        public IEnumerable<Artivity.DataModel.Journal.Activity> ListActivity(string fileUrl)
        {
            var res = ActivityList.GetActivities(ModelProvider.GetAllActivities(), fileUrl).ToList();
            return res;
        }



    }
}
