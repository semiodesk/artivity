using System;
using ArtivityExplorer.Controls;
using Eto.Forms;
using System.IO;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer
{
    public class FileView : StackLayout
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
            Orientation = Orientation.Horizontal;
            Spacing = 0;

            // Initialize the content layout.
            StackLayout content = new StackLayout();
            content.Orientation = Orientation.Horizontal;
            content.Items.Add(new StackLayoutItem(_statsPanel));
            content.Items.Add(new StackLayoutItem( _log, true));
            content.Spacing = 0;

            //Items.Add(new StackLayoutItem(_chart));
            Items.Add(new StackLayoutItem(content, true));
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

            _log.LoadInfluences(FileUrl);

            _chart.Reset();
            _chart.LoadActivities(FileUrl);
            _chart.LoadActivityInfluences(FileUrl);
        }

        #endregion
    }
}

