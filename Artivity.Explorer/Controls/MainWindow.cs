using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Xwt;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Parsers;
using System.IO;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Window
    {
        #region Members

		private IModel _model;

        private HeaderMenu _headerMenu;

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
            if (!SetupHelper.HasModels() && !SetupHelper.InstallModels())
            {
                throw new Exception("Failed to setup RDF datamodels.");
            }

            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            IModelGroup activities = store.CreateModelGroup();
            activities.Add(store.GetModel(Models.Agents));
            activities.Add(store.GetModel(Models.Activities));
            activities.Add(store.GetModel(Models.WebActivities));

            _model = activities;
        }

        private void InitializeComponent()
        {
            Padding = 0;

            _headerMenu = new HeaderMenu(this);

            // Initialize the acitivites log.
            _log = new ActivitiesLog();

            // Initialize the content layout.
            HBox content = new HBox();
			content.Spacing = 0;
			content.PackStart(_statsPanel);
			content.PackStart( _log, true);

            VBox layout = new VBox();
            layout.Spacing = 0;
            layout.PackStart(_headerMenu);
            layout.PackStart(_chart);
            layout.PackStart(content, true);

            Content = layout;

            Content.SetFocus();
        }

        public void HandleFileSelected(object sender, FileSelectionEventArgs e)
        {
            bool fileExists = File.Exists(e.FileName);

            _headerMenu.ExportButton.Sensitive = fileExists;

            if (!fileExists)
            {
                return;
            }

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

            _log.LoadInfluences(_model, fileUrl);
            _chart.LoadActivities(_model, fileUrl);
            _chart.LoadActivityInfluences(_model, fileUrl);
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
