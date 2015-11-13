using System;
using System.IO;
using System.Linq;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using data = Artivity.DataModel;
using Artivity.Explorer.Controls;
using Artivity.Explorer.Parsers;

namespace Artivity.Explorer
{
    public class FileView : View
    {
        #region Members

        private FileViewHeader _header = new FileViewHeader();

        private ActivityChart _chart = new ActivityChart();

        private ActivityLog _log = new ActivityLog();

        private FileStatsPanel _statsPanel = new FileStatsPanel();

        private TabControl _tabs = new TabControl();

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;

                Refresh();
            }
        }

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
            Model = data.Models.GetActivities();

            Orientation = Orientation.Vertical;
            Spacing = 14;

            StackLayout _logLayout = new StackLayout();
            _logLayout.Spacing = 7;
            _logLayout.Orientation = Orientation.Vertical;
            _logLayout.Items.Add(new StackLayoutItem(_chart, HorizontalAlignment.Stretch, false));
            _logLayout.Items.Add(new StackLayoutItem(_log, HorizontalAlignment.Stretch, true));

            _tabs.TabPosition = DockPosition.Top;
            _tabs.Pages.Add(new TabPage(_statsPanel) { Text = " Statistics  ", Image = Bitmap.FromResource("PieChart.png"), Padding = new Padding(0) });
            _tabs.Pages.Add(new TabPage(_logLayout) { Text = " History    ", Image = Bitmap.FromResource("History.png"), Padding = new Padding(0) });
            _tabs.Pages.Add(new TabPage() { Text = " Notes     ", Image = Bitmap.FromResource("Note.png"), Padding = new Padding(0) });

            Items.Add(new StackLayoutItem(_header, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(_tabs, HorizontalAlignment.Stretch, true));

            _log.SelectedItemsChanged += OnActivityLogSelectedItemsChanged;
        }

        private void Refresh()
        {
            bool fileExists = File.Exists(_filePath);

            if (!fileExists)
            {
                return;
            }

            _header.FilePath = _filePath;

            SvgStats stats = SvgStatsParser.TryParse(_filePath);

            if (stats != null)
            {
                _statsPanel.CompositionWidget.Update(stats);
                _statsPanel.ColourWidget.Update(stats);
            }

            string fileUrl = new Uri("file://" + FilePath).AbsoluteUri;

            _statsPanel.EditingWidget.Update(fileUrl);

            _log.LoadInfluences(fileUrl);
            _chart.LoadInfluences(fileUrl);

            if (_log.DataStore.Any())
            {
                ActivityLogItem item = _log.DataStore.First() as ActivityLogItem;

                _chart.SetTitle(item.Date);

                data.Activity activity = Model.GetResource<data.Activity>(item.Activity);

                _chart.Zoom(activity.StartTime, activity.EndTime);
            }

            _tabs.SelectedIndex = 1;
        }

        private void OnActivityLogSelectedItemsChanged(object sender, EventArgs e)
        {
            ActivityLogItem selectedItem = _log.SelectedItem as ActivityLogItem;

            if (selectedItem == null || string.IsNullOrEmpty(selectedItem.InfluencedRegion))
            {
                return;
            }
                
            UriRef regionUri = new UriRef(selectedItem.InfluencedRegion);

            data.Rectangle region = Model.GetResource<data.Rectangle>(regionUri);

            _header.Thumbnail.HighlightColour = Color.Parse(selectedItem.AgentColour);
            _header.Thumbnail.HighlightedRegion = region;

            data.Activity activity = Model.GetResource<data.Activity>(selectedItem.Activity);

            _chart.Zoom(activity.StartTime, activity.EndTime);

            _chart.SetTitle(selectedItem.Date);
            _chart.SetPositionMarker(selectedItem.Date);
        }

        #endregion
    }
}

