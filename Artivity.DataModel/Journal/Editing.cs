using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel.Journal
{
    public class Editing
    {
        public static int GetSessionCount(IModel model, Uri fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                select count(distinct ?activity) as ?activities where
                {
                    ?activity prov:used ?file .

                    ?file nfo:fileUrl """ + fileUrl.AbsoluteUri + @""" .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["activities"] : 0;
        }

        public static int GetStepCount(IModel model, Uri fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                select count(distinct ?version) as ?steps where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl.AbsoluteUri + @""" .

                    ?version prov:qualifiedGeneration ?generation .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["steps"] : 0;
        }

        public static int GetUndoCount(IModel model, Uri fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                select count(distinct ?version) as ?undos where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl.AbsoluteUri + @""" .

                    ?version prov:qualifiedGeneration ?generation .

                    ?generation a art:Undo .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["undos"] : 0;
        }

        public static int GetRedoCount(IModel model, Uri fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                select count(distinct ?version) as ?redos where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl.AbsoluteUri + @""" .

                    ?version prov:qualifiedGeneration ?generation .

                    ?generation a art:Redo .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["redos"] : 0;
        }

    }
}
