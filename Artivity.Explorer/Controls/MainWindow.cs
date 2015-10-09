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

        private void InitializeModel()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            IModelGroup activities = store.CreateModelGroup();

            if (store.ContainsModel(Models.Agents))
            {
                activities.Add(store.GetModel(Models.Agents));
            }
            else
            {
                Console.WriteLine("ERROR: Could not establish connection to model <{0}>", Models.Agents);
            }

            if (store.ContainsModel(Models.Activities))
            {
                activities.Add(store.GetModel(Models.Activities));
            }
            else
            {
                Console.WriteLine("ERROR: Could not establish connection to model <{0}>", Models.Activities);
            }

            if (store.ContainsModel(Models.WebActivities))
            {
                activities.Add(store.GetModel(Models.WebActivities));
            }
            else
            {
                Console.WriteLine("ERROR: Could not establish connection to model <{0}>", Models.WebActivities);
            }

            _model = activities;
        }

        private void InitializeComponent()
        {
			if (_model == null)
			{
				LogError("Model not initialized.");
			}

            Padding = 0;

            // Initialize the main menu.
            MainMenu menu = new MainMenu();
            menu.FileSelected += HandleFileSelected;

            MainMenu = menu;

            // Initialize the acitivites log.
            _log = new ActivitiesLog();

            // Initialize the content layout.
            HBox content = new HBox();
			content.Spacing = 0;
			content.PackStart(_statsPanel);
			content.PackStart( _log, true);

            VBox layout = new VBox();
            layout.Spacing = 0;
            layout.PackStart(new HeaderMenu(this));
            layout.PackStart(_chart);
            layout.PackStart(content, true);

            Content = layout;
        }

        public void HandleFileSelected(object sender, FileSelectionEventArgs e)
        {
            SvgStats stats = SvgStatsParser.TryParse(e.FileName);

            if (stats != null)
            {
                _statsPanel.CompositionWidget.Update(stats);
                _statsPanel.ColourWidget.Update(stats);
            }

			_statsPanel.EditingWidget.Update(_model, e.FileName);

            _statusBar.Update(e.FileName);

			LoadActivities(e.FileName);
        }

		private void LoadActivities(string file)
		{
            string fileUrl = file.StartsWith("file://") ? file : "file://" + file;

            _log.Reset();
            _chart.Reset();

            AsyncLoadMethodCaller activities = new AsyncLoadMethodCaller(_chart.LoadActivities);
            AsyncLoadMethodCaller influences = new AsyncLoadMethodCaller(_chart.LoadActivityInfluences);
            AsyncLoadMethodCaller log = new AsyncLoadMethodCaller(_log.LoadInfluences);

            IAsyncResult activitiesResult = activities.BeginInvoke(_model, fileUrl, null, null);
            IAsyncResult influencesResult = influences.BeginInvoke(_model, fileUrl, null, null);
            IAsyncResult logResult = log.BeginInvoke(_model, fileUrl, null, null);

            activities.EndInvoke(activitiesResult);
            influences.EndInvoke(influencesResult);
            log.EndInvoke(logResult);
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
