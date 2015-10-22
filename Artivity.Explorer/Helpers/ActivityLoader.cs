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

        public static DateTime GetFirstEventTime(IModel model, string file)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?time WHERE 
                {
                    ?activity prov:used ?file .
                    ?file nfo:fileUrl """ + file + @""" .

                    ?activity prov:generated ?entity .
                    ?entity prov:qualifiedGeneration ?generation .

                    ?activity prov:startedAtTime ?time .
                }
                ORDER BY ASC(?time) LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
        }

        public static DateTime GetLastEventTime(IModel model, string file)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?time WHERE 
                {
                    ?activity prov:used ?file .
                    ?file nfo:fileUrl """ + file + @""" .

                    ?activity prov:generated ?entity .
                    ?entity prov:qualifiedGeneration ?generation .

                    ?activity prov:endedAtTime ?time .
                }
                ORDER BY DESC(?time) LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
        }

        #endregion
    }
}

