using Artivity.Apid;
using Artivity.Apid.Platforms;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Modules
{
    public class ProjectModule : EntityBase<Project>
    {
        public ProjectModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/projects", modelProvider, platformProvider)
        {
            Get["/addFileToProject"] = parameters =>
            {
                string projectUri = Request.Query.projectUri;
                string fileUri = Request.Query.fileUri;

                if ((string.IsNullOrEmpty(fileUri) || !IsUri(fileUri)) && (string.IsNullOrEmpty(projectUri) || !IsUri(projectUri)))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return AddFileToProject(projectUri, fileUri);
            };
        }

        protected Response AddFileToProject(string projectUri, string fileUri)
        {
            IModel m = ModelProvider.GetActivities();
            Project proj = m.GetResource<Project>(new Uri(projectUri));
            Entity entity = m.GetResource<Entity>(new Uri(fileUri));
            if (!proj.Members_.Contains(entity))
            {
                proj.Members_.Add(entity);
                proj.Commit();
            }

            return Response.AsJson(new Dictionary<string, bool>{ {"success", true}});
        }
    }
}
