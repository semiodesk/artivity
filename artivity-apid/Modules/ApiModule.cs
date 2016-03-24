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

            Get["/activity/list"] = parameters =>
            {
                string f = Request.Query["file"];
                int qty = Request.Query["qty"];
                int idx = Request.Query["idx"];
                var l = ListActivity(f);
                return Response.AsJson(l);
            };


        }



        public IEnumerable<JournalFile> ListJournalFiles()
        {
            return Journal.GetItems(ModelProvider.ActivitiesModel);
        }

        public IEnumerable<Artivity.DataModel.Journal.Activity> ListActivity(string fileUrl)
        {
            return ActivityList.GetActivities(ModelProvider.GetAllActivities(), fileUrl);
        }

    }
}
