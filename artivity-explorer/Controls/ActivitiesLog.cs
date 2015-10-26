using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;
using Artivity.Model;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesLog : ListView
    {
		#region Members
		
        public ListStore Store { get; private set; }

		public IDataField<string> AgentField { get; private set; }
		public IDataField<string> TimeField { get; private set; }
		public IDataField<string> TypeField { get; private set; }
        public IDataField<string> DescriptionField { get; private set; }
		public IDataField<string> DataField { get; private set; }

		#endregion

        #region Constructors

        public ActivitiesLog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = 0;
            HeadersVisible = true;
            BorderVisible = false;
            ExpandHorizontal = true;
            SelectionMode = SelectionMode.Multiple;

			AgentField = new DataField<string>();

            TimeField = new DataField<string>();

			TextCellView timeView = new TextCellView();
			timeView.TextField = TimeField;

			ListViewColumn timeColumn = new ListViewColumn ("Time", timeView);
			timeColumn.Alignment = Alignment.End;
            timeColumn.CanResize = true;

			TypeField = new DataField<string>();

			TextCellView typeView = new TextCellView();
            typeView.TextField = TypeField;

			ListViewColumn typeColumn = new ListViewColumn ("Event", typeView);
			typeColumn.Alignment = Alignment.Start;
            typeColumn.CanResize = true;

            DescriptionField = new DataField<string>();

            TextCellView descriptionView = new TextCellView();
            descriptionView.TextField = DescriptionField;

            ListViewColumn descriptionColumn = new ListViewColumn ("Description", descriptionView);
            descriptionColumn.Alignment = Alignment.Start;
            descriptionColumn.CanResize = true;

			DataField = new DataField<string>();

            TextCellView dataView = new TextCellView();
            dataView.TextField = DataField;

            ListViewColumn dataColumn = new ListViewColumn ("Details", dataView);
            dataColumn.Alignment = Alignment.Start;
            dataColumn.CanResize = true;

            Columns.Add(timeColumn);
            Columns.Add(typeColumn);
            Columns.Add(descriptionColumn);
			Columns.Add(dataColumn);

            Store = new ListStore(AgentField, TimeField, TypeField, DescriptionField, DataField);

            DataSource = Store;
        }

        public void Reset()
        {
            Store.Clear();
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
            foreach (BindingSet binding in result.GetBindings())
            {
                int row = Store.AddRow();

                Store.SetValue(row, AgentField, binding["agent"].ToString());

                DateTime time = (DateTime)binding["influenceTime"];

                // Set the formatted date time.
                Store.SetValue(row, TimeField, time.ToString("HH:mm:ss")); 

                if (!(binding["influenceType"] is DBNull))
                {
                    Store.SetValue(row, TypeField, ToDisplayString(binding["influenceType"].ToString()));
                }
                    
                if (!(binding["description"] is DBNull))
                {
                    Store.SetValue(row, DescriptionField, binding["description"].ToString());
                }

                UriRef entityType = new UriRef(binding["entityType"].ToString());

                if (entityType == nfo.FileDataObject.Uri)
                {
                    string value = binding["value"].ToString();

                    Store.SetValue(row, DataField, value);
                }
                else
                {
                    UriRef entityUri = new UriRef(binding["entity"].ToString());

                    Store.SetValue(row, DataField, entityUri.Host);
                }
            }
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
