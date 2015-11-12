using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Eto.Forms;
using OxyPlot;

namespace Artivity.Explorer.Controls
{
    public class ActivityLog : GridView
    {
        #region Members

        private Dictionary<Uri, SoftwareAgent> _agents = new Dictionary<Uri, SoftwareAgent>();

        private readonly List<ActivityLogItem> _items = new List<ActivityLogItem>();

        #endregion

        #region Constructors

        public ActivityLog()
        {
            InitializeComponent();
            InitializeAgents();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            ShowHeader = true;

            Columns.Add(new GridColumn() { HeaderText = "", DataCell = new ColourPickerCell("AgentColour") { MaxWidth = 2 }, Width = 5, AutoSize = false, Resizable = false });
            Columns.Add(new GridColumn() { HeaderText = "Time", DataCell = new TextBoxCell("FormattedTime") });
            Columns.Add(new GridColumn() { HeaderText = "Influence", DataCell = new TextBoxCell("InfluenceType") });
            Columns.Add(new GridColumn() { HeaderText = "Description", DataCell = new TextBoxCell("Description") });
            Columns.Add(new GridColumn() { HeaderText = "Data", DataCell = new TextBoxCell("Data") });
        }

        private void InitializeAgents()
        {
            IModel model = Models.GetAgents();

            foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>())
            {
                _agents[agent.Uri] = agent;
            }
        }

        public void LoadInfluences(string fileUrl)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX dces: <http://purl.org/dc/elements/1.1/>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                SELECT ?activity ?agent ?influenceTime ?influenceType ?entity ?entityType ?description ?value ?bounds WHERE 
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

                        OPTIONAL { ?generation art:hadBoundaries ?bounds . }
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

                item.Activity = new UriRef(binding["activity"].ToString());
                item.Agent = new UriRef(binding["agent"].ToString());

                if (_agents.ContainsKey(item.Agent))
                {
                    item.AgentColour = _agents[item.Agent].ColourCode;
                }

                item.Date = (DateTime)binding["influenceTime"];

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

                if (!(binding["bounds"] is DBNull))
                {
                    item.InfluencedRegion = binding["bounds"].ToString();
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
