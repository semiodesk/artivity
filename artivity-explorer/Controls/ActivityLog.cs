using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;
using Artivity.Model;
using Eto.Forms;

namespace Artivity.Explorer.Controls
{
    public class ActivityLog : GridView
    {
        #region Members

        private readonly List<ActivityLogItem> _items = new List<ActivityLogItem>();

        #endregion

        #region Constructors

        public ActivityLog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            ShowHeader = true;

            Columns.Add(new GridColumn() { HeaderText = "Time", DataCell = new TextBoxCell("Time") });
            Columns.Add(new GridColumn() { HeaderText = "Influence", DataCell = new TextBoxCell("InfluenceType") });
            Columns.Add(new GridColumn() { HeaderText = "Description", DataCell = new TextBoxCell("Description") });
            Columns.Add(new GridColumn() { HeaderText = "Data", DataCell = new TextBoxCell("Data") });
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
            _items.Clear();

            foreach (BindingSet binding in result.GetBindings())
            {
                ActivityLogItem item = new ActivityLogItem();

                item.Agent = binding["agent"].ToString();

                DateTime time = (DateTime)binding["influenceTime"];

                // Set the formatted date time.
                item.Time = time.ToString("HH:mm:ss");

                if (!(binding["influenceType"] is DBNull))
                {
                    item.InfluenceType = ToDisplayString(binding["influenceType"].ToString());
                }
                    
                if (!(binding["description"] is DBNull))
                {
                    item.Description = binding["description"].ToString();
                }

                UriRef entityType = new UriRef(binding["entityType"].ToString());

                if (entityType == nfo.FileDataObject.Uri)
                {
                    string value = binding["value"].ToString();

                    item.Data = value;
                }
                else
                {
                    UriRef entityUri = new UriRef(binding["entity"].ToString());

                    item.Data = entityUri.Host;
                }

                _items.Add(item);
            }

            DataStore = _items;
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
