﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;
using ArtivityExplorer.Parsers;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using System.Collections.ObjectModel;

namespace ArtivityExplorer.Controls
{
    public class EditingWidget : TableLayout
    {
        #region Members

        private Label _sessions;

        private Label _sessionsCount;

        private Label _steps;

        private Label _stepsCount;

        private Label _undos;

        private Label _undosCount;

        private Label _redos;

        private Label _redosCount;

        private Label _confidence;

        private Label _confidenceValue;

        #endregion

        #region Constructors

        public EditingWidget()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			Color color = new Color(49, 55, 57);

            var icon = new ImageView() { Image = Bitmap.FromResource("edit") };
            var title = new Label() { Text = "Editing", TextColor = color };

            Rows.Add(new TableRow(new TableCell(icon), new TableCell(title)));

            _sessions = new Label() { Text = "Sessions", TextColor = color };
            _sessionsCount = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_sessions), new TableCell(_sessionsCount)));

            _steps = new Label() { Text = "Steps", TextColor = color };
            _stepsCount = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_steps), new TableCell(_stepsCount)));

            _undos = new Label() { Text = "Undos", TextColor = color };
            _undosCount = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_undos), new TableCell(_undosCount)));

            _redos = new Label() { Text = "Redos", TextColor = color };
            _redosCount = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_redos), new TableCell(_redosCount)));

            _confidence = new Label() { Text = "Confidence", TextColor = color };
            _confidenceValue = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_confidence), new TableCell(_confidenceValue)));
        }

		public void Update(string fileUrl)
		{
            IModel model = Models.GetAllActivities();

            double sessionCount = GetSessionCount(model, fileUrl);
            double stepCount = GetStepCount(model, fileUrl);
            double undoCount = GetUndoCount(model, fileUrl);
            double redoCount = GetRedoCount(model, fileUrl);

			_sessionsCount.Text = sessionCount.ToString();
			_stepsCount.Text = stepCount.ToString();
			_undosCount.Text = undoCount.ToString();
			_redosCount.Text = redoCount.ToString();

			if (stepCount > 0)
			{
				_confidenceValue.Text = Math.Round((stepCount - undoCount + redoCount) / stepCount, 2).ToString();
			}
			else
			{
				_confidenceValue.Text = "0";
			}
		}

        private int GetSessionCount(IModel model, string fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                select count(distinct ?activity) as ?activities where
                {
                    ?activity prov:used ?file .

                    ?file nfo:fileUrl """ + fileUrl + @""" .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["activities"] : 0;
        }

        private int GetStepCount(IModel model, string fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                select count(distinct ?version) as ?steps where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl + @""" .

                    ?version prov:qualifiedGeneration ?generation .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["steps"] : 0;
        }

        private int GetUndoCount(IModel model, string fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                select count(distinct ?version) as ?undos where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl + @""" .

                    ?version prov:qualifiedGeneration ?generation .

                    ?generation a art:Undo .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["undos"] : 0;
        }

        private int GetRedoCount(IModel model, string fileUrl)
        {
            string queryString = @"PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                select count(distinct ?version) as ?redos where
                {
                    ?activity prov:used ?file .
                    ?activity prov:generated ?version .

                    ?file nfo:fileUrl """ + fileUrl + @""" .

                    ?version prov:qualifiedGeneration ?generation .

                    ?generation a art:Redo .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = model.ExecuteQuery(query).GetBindings();

            return bindings.Any() ? (int)bindings.First()["redos"] : 0;
        }

        #endregion
    }
}
