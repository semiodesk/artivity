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

using Artivity.Model.ObjectModel;
using Artivity.Api.Http.Parameters;
using Semiodesk.Trinity;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.ModelBinding;

namespace Artivity.Api.Http
{
	public class BrowserModule : ModuleBase
	{
		#region Members
		
        private static Dictionary<string, bool> _agents = new Dictionary<string, bool>();

		#endregion

		#region Constructors

		public BrowserModule()
		{
			try
			{
	            Post["/artivity/1.0/activities/web/"] = parameters => 
	            {
	                return AddActivity(this.Bind<ActivityParameters>()); 
	            };

				Post["/artivity/1.0/activities/web/status/"] = parameters =>
				{
					return SetStatus(this.Bind<AgentParameters>());
				};

				Get["/artivity/1.0/activities/web/status/"] = parameters =>
	            {
					return GetStatus(this.Bind<AgentParameters>());
	            };
			}
			catch(Exception e)
			{
				LogError(e);
			}
		}

		#endregion

		#region Methods

        protected HttpStatusCode AddActivity(ActivityParameters p)
        {
            if (string.IsNullOrEmpty(p.agent)
                || string.IsNullOrEmpty(p.title)
                || string.IsNullOrEmpty(p.url)
                || !Uri.IsWellFormedUriString(p.url, UriKind.Absolute))
            {
                return HttpStatusCode.BadRequest;
            }

			if(!_agents.ContainsKey(p.agent) || !_agents[p.agent])
            {
                return HttpStatusCode.Locked;
            }

			IModel model = GetModel("http://localhost:8890/artivity/1.0/activities/web/");

			WebDataObject page = model.CreateResource<WebDataObject>(p.url);
			page.Title = p.title;
			page.Commit();

			DateTime now = DateTime.Now;

			View view = model.CreateResource<View>();
			//view.Associations.Add(GetUserAssociation(model));
            view.Associations.Add(GetSoftwareAssociation(model, p.agent));
			view.UsedEntities.Add(page);
            view.StartTime = now;
            view.EndTime = now;
            view.Commit();

			LogRequest("/artivity/1.0/activities/web/", "POST", "", HttpStatusCode.OK);

            return HttpStatusCode.OK;
        }

        protected Response GetStatus(AgentParameters p)
        {
			AgentParameters result = new AgentParameters() { agent = p.agent, enabled = false };

            if(p.agent != null && _agents.ContainsKey(p.agent))
            {
                result.enabled = _agents[p.agent];
            }

			Response response = Response.AsJson(result);

			LogRequest("/artivity/1.0/activities/web/status/", "GET", "", response.StatusCode);

            return response;
        }

		protected Response SetStatus(AgentParameters p)
        {
			if (p.agent == null) return HttpStatusCode.BadRequest;

            if (p.enabled != null)
            {
				_agents[p.agent] = Convert.ToBoolean(p.enabled);
            }
			else if(_agents.ContainsKey(p.agent))
            {
				p.enabled = _agents[p.agent];
            }

            // We return the request so that the plugin can set the server's enabled status.
			Response response = Response.AsJson(p);

			LogRequest("/artivity/1.0/activities/web/status/", "GET", "", response.StatusCode);

			return response;
        }

        private SoftwareAssociation GetSoftwareAssociation(IModel model, string agentId)
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

			SoftwareAssociation association = model.CreateResource<SoftwareAssociation>();
			association.Agent = agent;

			return association;
        }

		#endregion
	}
}

