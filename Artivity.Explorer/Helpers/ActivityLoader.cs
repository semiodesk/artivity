using System;
using System.Linq;
using System.Collections.Generic;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class ActivityLoader
    {
        #region Methods

        public static IEnumerable<Activity> GetActivities(IModel model, string file)
        {
            string fileUrl = file.StartsWith("file://") ? file : "file://" + file;

            DateTime firstEventTime = GetFirstEventTime(model, fileUrl);
            DateTime lastEventTime = GetLastEventTime(model, fileUrl);

            if (firstEventTime == DateTime.MinValue || lastEventTime == DateTime.MinValue)
            {
                yield break;
            }

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?s ?p ?o WHERE
                {
                    ?s ?p ?o .
                    ?s prov:startedAtTime ?time .
                    FILTER(?time >= " + SparqlSerializer.SerializeDateTime(firstEventTime) + @") .
                    FILTER(?time <= " + SparqlSerializer.SerializeDateTime(lastEventTime) + @") .
                }
                ORDER BY DESC(?time)";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            foreach (Activity a in result.GetResources<Activity>())
            {
                yield return a;
            }
        }

        private static DateTime GetFirstEventTime(IModel model, string file)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?time WHERE { ?a prov:startedAtTime ?time . ?a prov:used ?f . ?f nfo:fileUrl """ + file + "\" . } ORDER BY ASC(?time) LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
        }

        private static DateTime GetLastEventTime(IModel model, string file)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?time WHERE { ?a prov:startedAtTime ?time . ?a prov:used ?f . ?f nfo:fileUrl """ + file + "\" . } ORDER BY DESC(?time) LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
        }

        #endregion
    }
}

