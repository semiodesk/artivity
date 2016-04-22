using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artivity.DataModel.Journal
{
    public class Journal
    {
        public static IEnumerable<JournalFile> GetItems(IModel model)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX dces: <http://purl.org/dc/elements/1.1/>

                SELECT ?agent ?startTime ?endTime ?fileUrl WHERE
                {
                       ?activity prov:used ?entity .
                       ?activity prov:startedAtTime ?startTime .
                       ?activity prov:endedAtTime ?endTime .
                       ?activity prov:qualifiedAssociation ?association .

                       ?association prov:agent ?agent .

                       ?entity nfo:fileUrl ?fileUrl .
                }
                ORDER BY DESC(?startTime)";
            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            return LoadBindings(result.GetBindings());
        }

        private static IEnumerable<JournalFile> LoadBindings(IEnumerable<BindingSet> bindings)
        {
            Dictionary<string, JournalFile> items = new Dictionary<string, JournalFile>();

            foreach (BindingSet binding in bindings)
            {
                string url = binding["fileUrl"].ToString();

                // Skip any malformed URIs.
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    continue;
                }

                // Do not list files which do not exist.
                string path = new Uri(url).LocalPath;

                if (!File.Exists(path))
                {
                    continue;
                }

                UriRef agent = new UriRef(binding["agent"].ToString());
                DateTime startTime = (DateTime)binding["startTime"];
                DateTime endTime = (DateTime)binding["endTime"];
                TimeSpan editingTime = endTime - startTime;

                JournalFile item = new JournalFile()
                {
                    Agent = agent,
                    FileUrl = url,
                    FilePath = path,
                    LastEditingDate = startTime,
                    TotalEditingTime = editingTime
                };

                if (items.ContainsKey(path))
                {
                    items[path].TotalEditingTime += item.TotalEditingTime;
                }
                else
                {
                    items[path] = item;
                }
            }

            return items.Values;
        }

    }
}
