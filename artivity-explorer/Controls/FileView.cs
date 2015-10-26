using System;
using ArtivityExplorer.Controls;
using Xwt;
using System.IO;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer
{
    public class FileView : VBox
    {
        #region Members

        private ActivitiesChart _chart = new ActivitiesChart();

        private ActivitiesLog _log = new ActivitiesLog();

        private FileStatsPanel _statsPanel = new FileStatsPanel();

        public string FileUrl;

        #endregion

        #region Constructors

        public FileView()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = 0;
            Spacing = 0;

            // Initialize the content layout.
            HBox content = new HBox();
            content.PackStart(_statsPanel);
            content.PackStart( _log, true);
            content.Spacing = 0;
            content.Margin = 0;

            PackStart(_chart);
            PackStart(content, true);
        }

        public void Update()
        {
            string fileName = new Uri(FileUrl).AbsolutePath;

            bool fileExists = File.Exists(fileName);

            if (!fileExists)
            {
                return;
            }

            SvgStats stats = SvgStatsParser.TryParse(fileName);

            if (stats != null)
            {
                _statsPanel.CompositionWidget.Update(stats);
                _statsPanel.ColourWidget.Update(stats);
            }

            _statsPanel.EditingWidget.Update(FileUrl);

            _log.Reset();
            _log.LoadInfluences(FileUrl);

            _chart.Reset();
            _chart.LoadActivities(FileUrl);
            _chart.LoadActivityInfluences(FileUrl);
        }

        #endregion
    }
}

