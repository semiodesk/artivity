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
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using System.Text;
using System.IO;
using Nancy.IO;
using VDS.RDF;
using Semiodesk.Trinity.Store;

namespace Artivity.Api.Http
{
	public class ActivitiesModule : ModuleBase
	{
		#region Constructors

		public ActivitiesModule()
		{
			try
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
			catch(Exception e)
			{
				LogError(HttpStatusCode.InternalServerError, e);
			}
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
			IModel model = GetModel(Models.Activities);

			if(model == null)
			{
				return LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
			}

			AddResources(model, ToStream(data));

			return LogRequest(HttpStatusCode.OK, "/artivity/1.0/activities/", "POST", data);
		}

		private HttpStatusCode AddActivity(ActivityParameters p)
		{
			if (string.IsNullOrEmpty(p.agent)
			    || string.IsNullOrEmpty(p.title)
			    || string.IsNullOrEmpty(p.url)
			    || !Uri.IsWellFormedUriString(p.url, UriKind.Absolute))
			{
				return LogError(HttpStatusCode.BadRequest, "Invalid value for parameters {0}", p);
			}

			if (!IsCaptureEnabled(p))
			{
				return LogRequest(HttpStatusCode.Locked, "/artivity/1.0/activities/web/", "POST", "");
			}

			IModel model = GetModel(Models.WebActivities);

			WebDataObject website = model.CreateResource<WebDataObject>(p.url);
			website.Title = p.title;
			website.Commit();

			DateTime now = DateTime.Now;

			View view = model.CreateResource<View>();
			//view.Associations.Add(GetUserAssociation(model));
			view.Associations.Add(GetSoftwareAssociation(model, p.agent));
			view.UsedEntities.Add(website);
			view.StartTime = now;
			view.EndTime = now;
			view.Commit();

			return LogRequest(HttpStatusCode.OK, "/artivity/1.0/activities/web/", "POST", "");
		}

		private void AddResources(IModel model, Stream stream)
		{
			string connectionString = "Server=localhost:1111;uid=dba;pwd=dba;Charset=utf-8";

			using (TextReader reader = new StreamReader(stream))
			{
				using (VDS.RDF.Storage.VirtuosoManager m = new VDS.RDF.Storage.VirtuosoManager(connectionString))
				{
					using (VDS.RDF.Graph graph = new VDS.RDF.Graph())
					{
						IRdfReader parser = dotNetRDFStore.GetReader(RdfSerializationFormat.N3);
						parser.Load(graph, reader);

						graph.BaseUri = model.Uri;

						m.UpdateGraph(model.Uri, graph.Triples, new List<Triple>());
					}
				}
			}
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
