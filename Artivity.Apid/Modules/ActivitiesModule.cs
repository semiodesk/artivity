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
// Copyright (c) Semiodesk GmbH 2015

using Artivity.DataModel;
using Artivity.Apid.Parameters;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Nancy;
using Nancy.IO;
using Nancy.ModelBinding;
using VDS.RDF;
using System.Threading.Tasks;
using Artivity.Apid.Platforms;

namespace Artivity.Apid
{
	public class ActivitiesModule : ModuleBase
	{
        #region Members

        private static readonly Dictionary<string, Browse> _activities = new Dictionary<string, Browse>();

        private static object _modelLock = new object();

        #endregion

		#region Constructors

        public ActivitiesModule(IModelProvider model, IPlatformProvider platform) : base("/artivity/1.0/activities", model, platform)
        {
            Post["/web/"] = parameters => { return PostActivity(this.Bind<ActivityParameters>()); };
        }

		#endregion

		#region Methods

        private HttpStatusCode PostActivity(ActivityParameters p)
        {
            try
            {
                if (string.IsNullOrEmpty(p.tab))
                {
                    return Logger.LogRequest(HttpStatusCode.NotModified, Request.Url, "POST", p.tab);
                }

                if (string.IsNullOrEmpty(p.agent))
                {
                    return Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameters {0}", p);
                }

                if (!IsCaptureEnabled(p))
                {
                    return Logger.LogRequest(HttpStatusCode.Locked, Request.Url, "POST", "");
                }

                IModel model = ModelProvider.GetWebActivities();

                Browse activity;

                if (!_activities.ContainsKey(p.tab))
                {
                    Association association = model.CreateResource<Association>();
                    association.Agent = new SoftwareAgent(new UriRef(p.agent));
                    association.Commit();

                    activity = model.CreateResource<Browse>();
                    activity.Associations.Add(association);
                    activity.StartTime = p.startTime != null ? (DateTime)p.startTime : DateTime.Now;
                    activity.Commit();

                    _activities[p.tab] = activity;
                }
                else
                {
                    activity = _activities[p.tab];
                }

                if (!string.IsNullOrEmpty(p.url) && Uri.IsWellFormedUriString(p.url, UriKind.Absolute))
                {
                    UriRef url = new UriRef(p.url);

                    WebDataObject website;

                    if (!model.ContainsResource(url))
                    {
                        website = model.CreateResource<WebDataObject>(url);
                        website.Title = p.title;
                        website.Commit();
                    }
                    else
                    {
                        website = model.GetResource<WebDataObject>(url);
                    }

                    DateTime time = p.time != null ? (DateTime)p.time : DateTime.Now;

                    View view = model.CreateResource<View>();
                    view.Entity = website;
                    view.Time = time;
                    view.Commit();

                    activity.Usages.Add(view);
                    activity.Commit();
                }
                else if (p.endTime != null)
                {
                    activity.EndTime = (DateTime)p.endTime;
                    activity.Commit();

                    _activities.Remove(p.tab);
                }

                return Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", "");
            }
            catch (Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, "{0}: {1}", Request.Url, e.Message);
            }
        }

        private bool IsCaptureEnabled(ActivityParameters p)
        {
            IModel model = ModelProvider.GetAgents();

            SoftwareAgent agent = null;
            Uri agentUri = new Uri(p.agent);

            if (model.ContainsResource(agentUri))
            {
                agent = model.GetResource<SoftwareAgent>(agentUri);
            }

            return agent.IsCaptureEnabled;
        }

		#endregion
	}
}

