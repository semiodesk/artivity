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
using Nancy;
using Nancy.ModelBinding;
using System.Text;
using System.IO;
using Nancy.IO;
using VDS.RDF;
using Semiodesk.Trinity.Store;

namespace Artivity.Api.Http
{
	public class ModelModule : ModuleBase
	{
		#region Constructors

        public ModelModule()
		{
            Post["/artivity/1.0/activities/"] = parameters => 
            {
				HttpStatusCode result = HttpStatusCode.OK;

				IModel model = GetModel("http://localhost:8890/artivity/1.0/activities/");

				if(model == null)
				{
					Console.WriteLine("ERROR: Could not establish connection to model <{0}>", model.Uri);

					return result;
				}

				string body = ToString(this.Request.Body);

				AddResources(model, ToStream(body));

				LogRequest("/artivity/1.0/activities/", "POST", body, result);

				return result;
            };
		}

		#endregion

		#region Methods

		private void AddResources(IModel model, Stream stream)
		{
			string connectionString = "Server=localhost:1111;uid=dba;pwd=dba;Charset=utf-8";

			using (TextReader reader = new StreamReader(stream))
			{
				try
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
				catch(Exception e)
				{
					Console.WriteLine(e);
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

		#endregion
	}
}

