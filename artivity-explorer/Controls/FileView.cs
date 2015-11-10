using System;
using Artivity.Explorer.Controls;
using Eto.Forms;
using System.IO;
using Artivity.Explorer.Parsers;

namespace Artivity.Explorer
{
    public class FileView : StackLayout
    {
        #region Members

        private ActivityChart _chart = new ActivityChart();

        private ActivityLog _log = new ActivityLog();

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
            Orientation = Orientation.Vertical;
            Spacing = 0;

            // Initialize the content layout.
            StackLayout content = new StackLayout();
            content.Orientation = Orientation.Horizontal;
            content.Items.Add(new StackLayoutItem(_statsPanel, VerticalAlignment.Stretch, false));
            content.Items.Add(new StackLayoutItem( _log, VerticalAlignment.Stretch, true));
            content.Spacing = 0;

            Items.Add(new StackLayoutItem(_chart, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(content, HorizontalAlignment.Stretch, true));
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

