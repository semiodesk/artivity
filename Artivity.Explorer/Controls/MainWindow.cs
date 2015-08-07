using System;
using Xwt;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Window
    {
        #region Members

        private ActivitiesChart _activitiesChart = new ActivitiesChart();

        private ActivitiesLog _activitiesLog = new ActivitiesLog();

        private StatsPanel _statsPanel = new StatsPanel();

        private StatusBar _statusBar = new StatusBar();

        #endregion

        #region Constructors

        public MainWindow() : base()
        {
            InitializeComponent();

            Closed += HandleClosed;
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Padding = 0;

            // Initialize the main menu.
            MainMenu menu = new MainMenu();
            menu.FileSelected += HandleFileSelected;

            MainMenu = menu;

            // Initialize the widget layout.
            HBox content = new HBox();
            content.PackStart(_statsPanel);
            content.PackStart( _activitiesLog, true);

            VBox layout = new VBox();
            layout.Spacing = 0;
            layout.PackStart(_activitiesChart);
            layout.PackStart(content, true);
            layout.PackStart(_statusBar);

            Content = layout;
        }

        private void HandleFileSelected(object sender, FileSelectionEventArgs e)
        {
            SvgStats stats = SvgStatsParser.TryParse(e.FileName);

            _statsPanel.CompositionWidget.Update(stats);
            _statsPanel.ColourWidget.Update(stats);

            _statusBar.Update(e.FileName);
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}
