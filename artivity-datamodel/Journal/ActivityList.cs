using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artivity.DataModel.Extensions;

namespace Artivity.DataModel.Journal
{
    public class ActivityList
    {
        #region Methods
        public static IEnumerable<Activity> GetActivities(IModel model, string fileUrl) //TODO add paging
        {
            List<Activity> res = new List<Activity>();
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?agent ?startTime ?endTime WHERE
                {
                  ?activity prov:qualifiedAssociation ?association .
                  ?activity prov:startedAtTime ?startTime .
                  ?activity prov:endedAtTime ?endTime .
                  ?activity prov:used ?entity .

                  ?association prov:agent ?agent .

                  ?entity nfo:fileUrl """ + Uri.EscapeUriString(fileUrl) + @""" .
                }
                ORDER BY DESC(?startTime)";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query, true);

            foreach (BindingSet binding in result.GetBindings())
            {
                res.Add( new Activity()
                {
                    AgentUri = new Uri(binding["agent"].ToString()),
                    StartTime = ((DateTime)binding["startTime"]).RoundToMinute(),
                    EndTime = ((DateTime)binding["endTime"]).RoundToMinute()
                });
            }
            return res;
        }
        #endregion
    }
}
