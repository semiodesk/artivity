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
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.ModelBinding;

namespace Artivity.Api.Http
{
	public class AgentsModule : ModuleBase
	{
		#region Constructors

		public AgentsModule()
		{
			try
			{
				Get["/artivity/1.0/agents/"] = parameters => { return GetAgents(); };
				Get["/artivity/1.0/agents/status/"] = parameters => { return GetStatus(this.Bind<AgentParameters>()); };
				Post["/artivity/1.0/agents/status/"] = parameters => { return SetStatus(this.Bind<AgentParameters>()); };
			}
			catch(Exception e)
			{
				LogError(HttpStatusCode.InternalServerError, e);
			}
		}

		#endregion

		#region Methods

		protected Response GetAgents()
		{
			IModel model = GetModel(Models.Agents);

			List<AgentParameters> agents = new List<AgentParameters>();

			foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>())
			{
				agents.Add(new AgentParameters() { agent = agent.Uri.AbsoluteUri, enabled = agent.IsCaptureEnabled });
			}

			LogRequest(HttpStatusCode.OK, "/artivity/1.0/agents/status/", "GET", "");

			return Response.AsJson(agents);
		}

        protected Response GetStatus(AgentParameters p)
        {
			if (p.agent == null)
			{
				LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

				// Return disabled state so that the browsers properly indicate disabled logging.
				p.enabled = false;

				return Response.AsJson(p);
			}

			IModel model = GetModel(Models.Agents);

			SoftwareAgent agent = null;
			Uri agentUri = new Uri(p.agent);

			if (model.ContainsResource(agentUri))
			{
				agent = model.GetResource<SoftwareAgent>(agentUri);
			}

			// Capturing agent data is disabled by default.
			p.enabled = agent != null ? agent.IsCaptureEnabled : false;

			LogRequest(HttpStatusCode.OK, "/artivity/1.0/agents/status/ " + p.agent, "GET", "");

			return Response.AsJson(p);
        }

		protected Response SetStatus(AgentParameters p)
        {
			if (p.agent == null)
			{
				LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

				// Return disabled state so that the browsers properly indicate disabled logging.
				return Response.AsJson(new AgentParameters() { agent = p.agent, enabled = false });
			}

			IModel model = GetModel(Models.Agents);

			SoftwareAgent agent;
			Uri agentUri = new Uri(p.agent);

			if (model.ContainsResource(agentUri))
			{
				agent = model.GetResource<SoftwareAgent>(agentUri);
			}
			else
			{
				agent = model.CreateResource<SoftwareAgent>(agentUri);
			}

			agent.IsCaptureEnabled = Convert.ToBoolean(p.enabled);
			agent.Commit();

			LogRequest(HttpStatusCode.OK, "/artivity/1.0/agents/status/ " + p.agent, "POST", "");

            // We return the request so that the plugin can set the server's enabled status.
			return Response.AsJson(p);
        }

		#endregion
	}
}

