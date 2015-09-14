using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Xwt;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Window
    {
        #region Members

		private IModel _model;

        private ActivitiesChart _chart = new ActivitiesChart();

        private ActivitiesLog _log;

        private StatsPanel _statsPanel = new StatsPanel();

        private StatusBar _statusBar = new StatusBar();

        #endregion

        #region Constructors

        public MainWindow() : base()
        {
			InitializeModel();
            InitializeComponent();

            Closed += HandleClosed;
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			if (_model == null)
			{
				LogError("Model not initialized.");
			}

			_log = new ActivitiesLog(_model);

            Padding = 0;

            // Initialize the main menu.
            MainMenu menu = new MainMenu();
            menu.FileSelected += HandleFileSelected;

            MainMenu = menu;

            // Initialize the widget layout.
            HBox content = new HBox();
			content.Spacing = 0;
            content.PackStart(_statsPanel);
            content.PackStart( _log, true);

            VBox layout = new VBox();
            layout.Spacing = 0;
            layout.PackStart(_chart);
            layout.PackStart(content, true);
            layout.PackStart(_statusBar);

            Content = layout;
        }

		private void InitializeModel()
		{
			IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

			Uri activities = new Uri("http://localhost:8890/artivity/1.0/activities");

			if (store.ContainsModel(activities))
			{
				_model = store.GetModel(activities);
			}
			else
			{
				_model = store.CreateModel(activities);
			}

			if(_model == null)
			{
				Console.WriteLine("ERROR: Could not establish connection to model <{0}>", activities);
			}
		}

        private void HandleFileSelected(object sender, FileSelectionEventArgs e)
        {
            SvgStats stats = SvgStatsParser.TryParse(e.FileName);

            _statsPanel.CompositionWidget.Update(stats);
            _statsPanel.ColourWidget.Update(stats);

            _statusBar.Update(e.FileName);

			if (_model == null)
				return;

			LoadActivities(e.FileName);
        }

		private void LoadActivities(string file)
		{
			_log.Store.Clear();

			DateTime firstTime = GetFirstEventTime(file);
			DateTime lastTime = GetLastEventTime(file);

			if (firstTime == DateTime.MinValue || lastTime == DateTime.MinValue)
			{
				LogInfo(string.Format("Did not find any events for file: {0}", file)); return;
			}

			ResourceQuery query = new ResourceQuery();
			query.Where(rdf.type, prov.Activity);
			query.Where(prov.startedAtTime).GreaterOrEqual(firstTime);
			query.Where(prov.startedAtTime).LessOrEqual(lastTime);

			List<Activity> activities = _model.GetResources<Activity>(query, true).OrderByDescending(a => a.StartTime).ToList();

			foreach (Activity activity in activities)
			{
				TreeNavigator node = _log.Store.AddNode();
				node.SetValue(_log.UriField, activity.Uri);
				node.SetValue(_log.TimeField, activity.StartTime);

				if (activity.GetTypes().Any())
				{
					node.SetValue(_log.TypeField, ToDisplayString(activity.GetTypes().First().Uri));
				}

				if (activity.Associations.OfType<SoftwareAssociation>().Any())
				{
					SoftwareAssociation association = activity.Associations.OfType<SoftwareAssociation>().First();

					if (association.Viewbox != null)
					{
						node.SetValue(_log.ZoomField, Math.Round(association.Viewbox.ZoomFactor * 100, 0) + "%");
					}
				}

				if (activity.InvalidatedEntities.OfType<FileDataObject>().Any())
				{
					FileDataObject f = activity.InvalidatedEntities.OfType<FileDataObject>().First();

					node.SetValue(_log.ModificationFromField, f.RevisedValue.ToString());
				}

				if (activity.GeneratedEntities.OfType<FileDataObject>().Any())
				{
					FileDataObject f = activity.GeneratedEntities.OfType<FileDataObject>().First();

					node.SetValue(_log.ModificationToField, f.RevisedValue.ToString());

					if (f.RevisedLocation is XmlAttribute)
					{
						XmlAttribute a = f.RevisedLocation as XmlAttribute;

						node.SetValue(_log.TargetField, a.LocalName);
					}
				}

				if (activity.UsedEntities.OfType<WebDataObject>().Any())
				{
					WebDataObject w = activity.GeneratedEntities.OfType<WebDataObject>().First();

					node.SetValue(_log.TargetField, w.Uri.OriginalString);
				}

				/* TODO: Node type not supported yet. Add to modelling.
				if (activity.Modifications.Any())
				{
					Modification m = activity.Modifications.First();
					string fromValue = m.FromValue != null ? m.FromValue.ToString() : "";
					string toValue = m.ToValue != null ? m.ToValue.ToString() : "";

					node.SetValue(_log.ModificationTypeField, ToDisplayString(m.GetTypes().FirstOrDefault().Uri));
				}
				*/
}

			_chart.Update(activities);
		}

		private DateTime GetFirstEventTime(string file)
		{
			string queryString = @"
				PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
				PREFIX as: <http://www.w3.org/ns/activitystreams#>

				SELECT ?time WHERE { ?s a as:Activity . ?s as:target <" + file + "> . ?s as:startTime ?time . } ORDER BY ASC(?time) LIMIT 1";

			SparqlQuery query = new SparqlQuery(queryString);
			ISparqlQueryResult result = _model.ExecuteQuery(query, true);

			return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
		}

		private DateTime GetLastEventTime(string file)
		{
			string queryString = @"
				PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
				PREFIX as: <http://www.w3.org/ns/activitystreams#>

				SELECT ?time WHERE { ?s a as:Activity . ?s as:target <" + file + "> . ?s as:startTime ?time . } ORDER BY DESC(?time) LIMIT 1";

			SparqlQuery query = new SparqlQuery(queryString);
			ISparqlQueryResult result = _model.ExecuteQuery(query, true);

			return result.Count() > 0 ? (DateTime)result.GetBindings().First()["time"] : DateTime.MinValue;
		}

		private string ToDisplayString(Uri uri)
		{
			string result = uri.AbsoluteUri;

			if (result.Contains('#'))
			{
				result = result.Substring(result.LastIndexOf('#') + 1);
			}
			else if(uri.AbsoluteUri.Contains('/'))
			{
				result = result.Substring(result.LastIndexOf('/') + 1);
			}

			return result.TrimEnd('>');
		}

        private void HandleClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }

		private void LogInfo(string msg)
		{
			Console.WriteLine("[{0}] Info: {1}", DateTime.Now, msg);
		}

		private void LogError(string msg)
		{
			Console.WriteLine("[{0}] Error: {1}", DateTime.Now, msg);
		}

        #endregion
    }
}
