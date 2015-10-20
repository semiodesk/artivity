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

using Artivity.Model;
using Artivity.Model.ObjectModel;
using Artivity.Api.Http.Parameters;
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

namespace Artivity.Api.Http
{
	public class ActivitiesModule : ModuleBase
	{
        #region Members

        private static readonly Dictionary<string, Browse> _activities = new Dictionary<string, Browse>();

        #endregion

		#region Constructors

		public ActivitiesModule()
		{
            Post["/artivity/1.0/activities/"] = parameters => 
            {
				return AddActivity(ToString(this.Request.Body));
            };

			Post["/artivity/1.0/activities/web/"] = parameters => 
			{
				return AddActivity(this.Bind<ActivityParameters>()); 
			};
		}

		#endregion

		#region Methods

		private Association GetSoftwareAssociation(IModel model, string agentId)
		{
			Uri agentUri = new Uri(agentId);

			SoftwareAgent agent;

			if (model.ContainsResource(agentUri))
			{
				agent = model.GetResource<SoftwareAgent>(agentUri);
			}
			else
			{
				agent = model.CreateResource<SoftwareAgent>(agentUri);
				agent.Commit();
			}

			Association association = model.CreateResource<Association>();
			association.Agent = agent;
			association.Commit();

			return association;
		}

		private HttpStatusCode AddActivity(string data)
		{
            try
            {
        		IModel model = GetModel(Models.Activities);

        		if(model == null)
        		{
        			return Logger.LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
        		}

        		AddResources(model, ToStream(data));

        		return Logger.LogRequest(HttpStatusCode.OK, "/artivity/1.0/activities/", "POST", data);
            }
            catch(Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, e);
            }
		}

		private HttpStatusCode AddActivity(ActivityParameters p)
		{
            try
            {
                if (string.IsNullOrEmpty(p.tab))
                {
                    return Logger.LogRequest(HttpStatusCode.NotModified, "/artivity/1.0/activities/web/", "POST", p.tab);
                }

                if (string.IsNullOrEmpty(p.agent))
    			{
    				return Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameters {0}", p);
    			}

    			if (!IsCaptureEnabled(p))
    			{
    				return Logger.LogRequest(HttpStatusCode.Locked, "/artivity/1.0/activities/web/", "POST", "");
    			}
                    
                IModel model = GetModel(Models.WebActivities);

                Browse activity;

                if (!_activities.ContainsKey(p.tab))
                {
                    activity = model.CreateResource<Browse>();
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

    			return Logger.LogRequest(HttpStatusCode.OK, "/artivity/1.0/activities/web/", "POST", "");
            }
            catch(Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, e);
            }
		}

		private void AddResources(IModel model, Stream stream)
		{
            try
            {
    			string connectionString = "Server=localhost:1111;uid=dba;pwd=dba;Charset=utf-8";

                using (StreamReader reader = new StreamReader(stream))
    			{
                    string data = BindUriVariables(reader.ReadToEnd());

    				using (VDS.RDF.Storage.VirtuosoManager m = new VDS.RDF.Storage.VirtuosoManager(connectionString))
    				{
    					using (VDS.RDF.Graph graph = new VDS.RDF.Graph())
    					{
    						IRdfReader parser = dotNetRDFStore.GetReader(RdfSerializationFormat.N3);
    						parser.Load(graph, new StringReader(data));

    						graph.BaseUri = model.Uri;

    						m.UpdateGraph(model.Uri, graph.Triples, new List<Triple>());
    					}
    				}
    			}
            }
            catch(Exception e)
            {
                Logger.LogError(HttpStatusCode.InternalServerError, e);
            }
		}

        public static string BindUriVariables(string data)
        {
            Regex expression = new Regex(@"<(?<var>\?\:(?<url>.+))>");

            MatchCollection matches = expression.Matches(data);

            if (matches.Count == 0) return data;

            foreach (Match match in matches)
            {
                string token = match.Groups["var"].Value;
                string url = match.Groups["url"].Value;

                Uri uri = FileSystemMonitor.GetFileUri(url);

                data = data.Replace(token, uri.OriginalString);
            }

            return data;
        }

		private Stream ToStream(string str)
		{
			MemoryStream stream = new MemoryStream();

			StreamWriter writer = new StreamWriter(stream);
			writer.Write(str);
			writer.Flush();
			stream.Position = 0;

			return stream;
		}

		private string ToString(RequestStream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		private bool IsCaptureEnabled(ActivityParameters p)
		{
			IModel model = GetModel(Models.Agents);

			SoftwareAgent agent = null;
			Uri agentUri = new Uri(p.agent);

			if (model.ContainsResource(agentUri))
			{
				agent = model.GetResource<SoftwareAgent>(agentUri);
			}

			return agent != null ? agent.IsCaptureEnabled : false;
		}

		#endregion
	}
}

