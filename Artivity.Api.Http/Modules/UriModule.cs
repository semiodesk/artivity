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
	public class UriModule : ModuleBase
	{
		#region Constructors

        public UriModule()
		{
			try
			{
				Get["/artivity/1.0/uri"] = parameters => 
				{
                    if(Request.Query.file)
                    {
                        return GetFileUri();
                    }
                    else if(Request.Query.canvas)
                    {
                        return GetCanvasUri();
                    }
                    else if(Request.Query.latestVersion)
                    {
                        return GetLatestVersionUri();
                    }

                    return Logger.LogError(HttpStatusCode.BadRequest, Request.Url, "");
				};
			}
			catch(Exception e)
			{
				Logger.LogError(HttpStatusCode.InternalServerError, e);
			}
		}

		#endregion

		#region Methods

        private Response GetFileUri()
		{
            Logger.LogRequest(HttpStatusCode.OK, Request);

            string url = GetUri(Request.Query.file);

            string queryString = @"
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT DISTINCT ?uri WHERE { ?v prov:specializationOf ?uri . ?uri nfo:fileUrl """ + url + "\" . } LIMIT 1";

            IModel model = Models.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            if (result.GetBindings().Any())
            {
                BindingSet binding = result.GetBindings().First();

                string uri = binding["uri"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    return Response.AsJson(uri);
                }
            }
                
            return "";
		}

        private Response GetCanvasUri()
        {
            Logger.LogRequest(HttpStatusCode.OK, Request);

            string url = GetUri(Request.Query.canvas);

            string queryString = @"
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                SELECT DISTINCT ?uri WHERE { ?v prov:specializationOf ?f . ?f nfo:fileUrl """ + url + "\" . ?f art:canvas ?uri . } LIMIT 1";

            IModel model = Models.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            if (result.GetBindings().Any())
            {
                BindingSet binding = result.GetBindings().First();

                string uri = binding["uri"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    return Response.AsJson(uri);
                }
            }
                
            return "";
        }

        private Response GetLatestVersionUri()
        {
            Logger.LogRequest(HttpStatusCode.OK, Request);

            string url = GetUri(Request.Query.latestVersion);

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT DISTINCT ?uri WHERE { ?uri prov:specializationOf ?f . ?f nfo:fileUrl """ + url + "\" . ?uri prov:qualifiedGeneration ?g . ?g prov:atTime ?time . } ORDER BY DESC(?time) LIMIT 1";

            IModel model = Models.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            if (result.GetBindings().Any())
            {
                BindingSet binding = result.GetBindings().First();

                string uri = binding["uri"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    return Response.AsJson(uri);
                }
            }

            return "";
        }

        private string GetUri(string path)
        {
            return path.StartsWith("file://") ? path : "file://" + path;
        }

		#endregion
	}
}

