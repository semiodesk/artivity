using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;
using Artivity.Model;
using Eto.Forms;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesLog : GridView
    {
        #region Constructors

        public ActivitiesLog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            ShowHeader = true;

//            Columns.AddTextColumn(TimeField, "Time", Alignment.Start, false, true);
//            Columns.AddTextColumn(TypeField, "Event", Alignment.Start, false, true);
//            Columns.AddTextColumn(DescriptionField, "Description", Alignment.Start, false, true);
//            Columns.AddTextColumn(DataField, "Data", Alignment.Start, false, true);
        }

        public void LoadInfluences(string fileUrl)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX dces: <http://purl.org/dc/elements/1.1/>

                SELECT ?agent ?influenceTime ?influenceType ?entity ?entityType ?description ?value WHERE 
                {
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:agent ?agent .

                    {
                        ?activity prov:used ?file ;
                            prov:generated ?entity .

                        ?file nfo:fileUrl """ + fileUrl + @""" .

                        ?entity a ?entityType ;
                            prov:qualifiedGeneration ?generation .

                        ?generation a ?influenceType ;
                            prov:atTime ?influenceTime .

                        OPTIONAL { ?generation dces:description ?description . }
                        OPTIONAL { ?generation prov:value ?value . }
                    }
                    UNION
                    {
                        ?editing prov:used ?file;
                                    prov:startedAtTime ?startTime ;
                                    prov:endedAtTime ?endTime .

                        ?file nfo:fileUrl """ + fileUrl + @""" .

                        ?activity prov:startedAtTime ?time ;
                            prov:qualifiedUsage ?usage .

                        ?usage a ?influenceType ;
                            prov:entity ?entity ;
                            prov:atTime ?influenceTime .

                        ?entity a ?entityType .

                        FILTER(?startTime <= ?time && ?time <= ?endTime) .
                    }
                }
                ORDER BY DESC(?influenceTime)";

            IModel model = Models.GetAllActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            CreateRows(result);
        }

        private void CreateRows(ISparqlQueryResult result)
        {
//            foreach (BindingSet binding in result.GetBindings())
//            {
//                int row = Store.AddRow();
//
//                Store.SetValue(row, AgentField, binding["agent"].ToString());
//
//                DateTime time = (DateTime)binding["influenceTime"];
//
//                // Set the formatted date time.
//                Store.SetValue(row, TimeField, time.ToString("HH:mm:ss")); 
//
//                if (!(binding["influenceType"] is DBNull))
//                {
//                    Store.SetValue(row, TypeField, ToDisplayString(binding["influenceType"].ToString()));
//                }
//                    
//                if (!(binding["description"] is DBNull))
//                {
//                    Store.SetValue(row, DescriptionField, binding["description"].ToString());
//                }
//
//                UriRef entityType = new UriRef(binding["entityType"].ToString());
//
//                if (entityType == nfo.FileDataObject.Uri)
//                {
//                    string value = binding["value"].ToString();
//
//                    Store.SetValue(row, DataField, value);
//                }
//                else
//                {
//                    UriRef entityUri = new UriRef(binding["entity"].ToString());
//
//                    Store.SetValue(row, DataField, entityUri.Host);
//                }
//            }
        }
            
        private string ToDisplayString(string uri)
        {
            if (uri.Contains('#'))
            {
                uri = uri.Substring(uri.LastIndexOf('#') + 1);
            }
            else if(uri.Contains('/'))
            {
                uri = uri.Substring(uri.LastIndexOf('/') + 1);
            }

            return uri.TrimEnd('>');
        }

        #endregion
    }
}
