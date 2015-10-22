﻿using System;
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

			DataField = new DataField<string>();

            TextCellView dataView = new TextCellView();
            dataView.TextField = DataField;

            ListViewColumn dataColumn = new ListViewColumn ("Details", dataView);
            dataColumn.Alignment = Alignment.Start;
            dataColumn.CanResize = true;

            Columns.Add(timeColumn);
            Columns.Add(typeColumn);
			Columns.Add(dataColumn);

            Store = new ListStore(AgentField, TimeField, TypeField, DataField);

            DataSource = Store;
        }

        public void Reset()
        {
            Store.Clear();
        }

        public void LoadInfluences(IModel model, string fileUrl)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?agent ?time ?activityType ?entity ?entityType ?generation ?generationTime ?value  WHERE 
                {
                    ?activity 
                        prov:qualifiedAssociation ?association ;
                        prov:startedAtTime ?time .

                    ?association prov:agent ?agent .

                    {
                        SELECT ?startTime ?endTime
                        {
                            ?activity prov:used ?file;
                                prov:startedAtTime ?startTime ;
                                prov:endedAtTime ?endTime .

                            ?file nfo:fileUrl """ + fileUrl + @""" .
                        }
                    }

                    {
                        ?activity prov:used ?entity .
                        ?activity prov:generated ?version .

                        ?entity a ?entityType .

                        ?version prov:qualifiedGeneration ?generation .

                        ?generation rdf:type ?activityType .
                        ?generation prov:atTime ?generationTime .

                        OPTIONAL { ?generation prov:value ?value . }
                    }
                    UNION
                    {
                        ?activity prov:qualifiedUsage ?usage .

                        ?usage a ?activityType .
                        ?usage prov:entity ?entity .

                        ?entity a ?entityType .
                    }


                    FILTER(?startTime <= ?time && ?time <= ?endTime) .
                }
                ORDER BY DESC(?time)";

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

                DateTime time;

                if(binding["generationTime"] is DBNull)
                {
                    time = (DateTime)binding["time"];
                }
                else
                {
                    time = (DateTime)binding["generationTime"];
                }

                // Set the formatted date time.
                Store.SetValue(row, TimeField, time.ToString("HH:mm:ss")); 

                if (!(binding["activityType"] is DBNull))
                {
                    Store.SetValue(row, TypeField, ToDisplayString(binding["activityType"].ToString()));
                }
                    
                UriRef entityType = new UriRef(binding["entityType"].ToString());

                if (entityType == nfo.FileDataObject.Uri)
                {
                    string value = binding["value"].ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        Store.SetValue(row, DataField, value);
                    }
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

		protected override void OnKeyReleased(KeyEventArgs args)
		{
			base.OnKeyReleased(args);

			if (args.Key == Key.Delete)
			{
				IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

                foreach(int row in SelectedRows)
				{
//					Activity activity = Store.GetValue(row, ActivityField);
//
//					IModel model;
//
//					if (activity.UsedEntities.OfType<WebDataObject>().Any())
//					{
//						model = store.GetModel(Models.WebActivities);
//					}
//					else
//					{
//						model = store.GetModel(Models.Activities);
//					}
//
//					model.DeleteResource(activity.Uri);
//
//                    Store.RemoveRow(row);
				}
			}
		}

        #endregion
    }
}
