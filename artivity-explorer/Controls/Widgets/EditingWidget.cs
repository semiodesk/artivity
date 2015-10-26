using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Parsers;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer.Controls
{
    public class EditingWidget : Table
    {
        #region Members

        private readonly Label _sessionLabel = new Label("Sessions");

        private readonly Label _sessionCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _stepLabel = new Label("Steps");

        private readonly Label _updateCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _undoLabel = new Label("  Undos");

        private readonly Label _undoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _redoLabel = new Label("  Redos");

        private readonly Label _redoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _confidenceLabel = new Label("Confidence");

        private readonly Label _confidenceValueLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

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
			Color color = Color.FromBytes(49, 55, 57);

			ImageView icon = new ImageView(BitmapImage.FromResource("edit"));

			Add(icon, 0, 0);

            Label title = new Label("Editing");
            title.Font = Font.WithWeight(FontWeight.Semibold);
            title.TextColor = color;

            Add(title, 1, 0);

            _sessionLabel.TextColor = color;
            _sessionCountLabel.TextColor = color;

            Add(_sessionLabel, 1, 1, 1, 1, true);
            Add(_sessionCountLabel, 2, 1);

            _stepLabel.TextColor = color;
            _updateCountLabel.TextColor = color;

            Add(_stepLabel, 1, 2, 1, 1, true);
            Add(_updateCountLabel, 2, 2);

            _undoLabel.TextColor = color;
            _undoCountLabel.TextColor = color;

            Add(_undoLabel, 1, 3, 1, 1, true);
            Add(_undoCountLabel, 2, 3);

            _redoLabel.TextColor = color;
            _redoCountLabel.TextColor = color;

            Add(_redoLabel, 1, 4, 1, 1, true);
            Add(_redoCountLabel, 2, 4);

            _confidenceLabel.TextColor = color;
            _confidenceValueLabel.TextColor = color;

            Add(_confidenceLabel, 1, 5, 1, 1, true);
            Add(_confidenceValueLabel, 2, 5);
        }

		public void Update(string fileUrl)
		{
            IModel model = Models.GetAllActivities();

            double sessionCount = GetSessionCount(model, fileUrl);
            double stepCount = GetStepCount(model, fileUrl);
            double undoCount = GetUndoCount(model, fileUrl);
            double redoCount = GetRedoCount(model, fileUrl);

			_sessionCountLabel.Text = sessionCount.ToString();
			_updateCountLabel.Text = stepCount.ToString();
			_undoCountLabel.Text = undoCount.ToString();
			_redoCountLabel.Text = redoCount.ToString();

			if (stepCount > 0)
			{
				_confidenceValueLabel.Text = Math.Round((stepCount - undoCount + redoCount) / stepCount, 2).ToString();
			}
			else
			{
				_confidenceValueLabel.Text = "0";
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
