// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.Linq;
using System.Collections.Generic;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.OxyPlot;
using Eto.Drawing;
using Artivity.DataModel;
using System.Timers;

namespace Artivity.Explorer
{
    public class DatabaseSettingsControl : StackLayout
    {
        #region Members

        private Timer _timer;

        private Database _database;

        private TextBox _filePathBox = new TextBox() { Enabled = false };

        private Button _filePickerButton = new Button() { Width = 30 };

        private Label _fileSizeLabel = new Label();

        private DatabaseSizeChart _fileSizePlot = new DatabaseSizeChart() { Height = 130 };

        private Label _factsCountLabel = new Label();

        private DatabaseFactsChart _factsCountPlot = new DatabaseFactsChart() { Height = 130 };

        private CheckBox _monitoringEnabledBox = new CheckBox() { Text = "Enable database monitoring" };

        #endregion

        #region Constructors

        public DatabaseSettingsControl()
        {
            if (!Models.Exists(Models.Monitoring))
            {
                Setup.InstallMonitoring();
            }

            IModel monitoring = Models.GetMonitoring();

            if (monitoring.IsEmpty)
            {
                Setup.InstallMonitoring();
            }

            _database = monitoring.GetResources<Database>().FirstOrDefault();

            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Spacing = 14;
            Orientation = Orientation.Vertical;

            _filePickerButton.Image = Bitmap.FromResource("Folder.png");

            _fileSizePlot.Update();
            _factsCountPlot.Update();

            if (_database != null)
            {
                _filePathBox.Text = new Uri(_database.Url).LocalPath;

                string label0 = "Total Facts:  {0}        Avg. Increase:  {1:0.##} / day";

                _factsCountLabel.Text = string.Format(label0, _database.GetFactsCount(), _factsCountPlot.AverageDelta);
                _factsCountLabel.TextColor = Palette.TextColor;

                string label1 = "Total Size:  {0} MB        Avg. Increase:  {1:0.##} kB / day";

                _fileSizeLabel.Text = string.Format(label1, Math.Round((_database.GetFileSize() / 1024) / 1024f, 2), _fileSizePlot.AverageDelta);
                _fileSizeLabel.TextColor = Palette.TextColor;

                _monitoringEnabledBox.Checked = _database.IsMonitoringEnabled;
            }

            StackLayout fileLayout = new StackLayout();
            fileLayout.Spacing = 7;
            fileLayout.Orientation = Orientation.Horizontal;
            fileLayout.Items.Add(new StackLayoutItem(_filePathBox, true));
            fileLayout.Items.Add(new StackLayoutItem(_filePickerButton, false));

            StackLayout fileStatsLayout = new StackLayout();
            fileStatsLayout.Spacing = 7;
            fileStatsLayout.Orientation = Orientation.Vertical;
            fileStatsLayout.Items.Add(new StackLayoutItem(_factsCountLabel, false));
            fileStatsLayout.Items.Add(new StackLayoutItem(_factsCountPlot, HorizontalAlignment.Stretch));
            fileStatsLayout.Items.Add(new StackLayoutItem(_fileSizeLabel, false));
            fileStatsLayout.Items.Add(new StackLayoutItem(_fileSizePlot, HorizontalAlignment.Stretch));

            Items.Add(new StackLayoutItem(fileLayout, HorizontalAlignment.Stretch));
            Items.Add(new StackLayoutItem(fileStatsLayout, HorizontalAlignment.Stretch, true));
            Items.Add(new StackLayoutItem(_monitoringEnabledBox));

            _monitoringEnabledBox.CheckedChanged += OnMonitoringEnabledBoxCheckedChanged;
            _filePickerButton.Click += OnFilePickerButtonClick;

            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);

            _timer.Enabled = false;
            _timer.Elapsed -= OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //_fileSizePlot.Update();
            //_factsCountPlot.Update();
        }

        private void OnMonitoringEnabledBoxCheckedChanged(object sender, EventArgs e)
        {
            _database.IsMonitoringEnabled = Convert.ToBoolean(_monitoringEnabledBox.Checked);
            _database.Commit();

            _fileSizePlot.Enabled = _database.IsMonitoringEnabled;
            _factsCountPlot.Enabled = _database.IsMonitoringEnabled;
        }

        private void OnFilePickerButtonClick(object sender, EventArgs e)
        {
            if (_database == null)
                return;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = new Uri(_database.Url).LocalPath;
            dialog.CheckFileExists = true;
            dialog.MultiSelect = false;

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                _database.Url = "file://" + dialog.FileName;
                _database.Commit();
            }
        }

        #endregion
    }
}