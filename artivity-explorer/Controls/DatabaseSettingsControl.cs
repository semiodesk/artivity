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

namespace Artivity.Explorer
{
    public class DatabaseSettingsControl : StackLayout
    {
        #region Members

        private Database _database;

        private TextBox _filePathBox = new TextBox() { Enabled = false };

        private Button _filePickerButton = new Button() { Width = 30 };

        private Label _fileSizeLabel = new Label();

        private DatabaseSizeChart _fileSizePlot = new DatabaseSizeChart() { Height = 130 };

        private Label _factsCountLabel = new Label();

        private DatabaseFactsChart _factsCountPlot = new DatabaseFactsChart() { Height = 130 };

        private CheckBox _monitoringEnabledBox = new CheckBox() { Text = "Enable database growth monitoring" };

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

            if (_database != null)
            {
                _filePathBox.Text = new Uri(_database.Url).AbsolutePath;

                _factsCountLabel.Text = "Facts:\t" + _database.GetFactsCount();
                _factsCountLabel.TextColor = Palette.TextColor;

                _fileSizeLabel.Text = "Size:\t" + Math.Round((_database.GetFileSize() / 1024f) / 1024f, 2) + " MB";
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

            _fileSizePlot.LoadMetrics();
            _factsCountPlot.LoadMetrics();

            _monitoringEnabledBox.CheckedChanged += OnMonitoringEnabledBoxCheckedChanged;
            _filePickerButton.Click += OnFilePickerButtonClick;
        }

        private void OnMonitoringEnabledBoxCheckedChanged(object sender, EventArgs e)
        {
            _database.IsMonitoringEnabled = Convert.ToBoolean(_monitoringEnabledBox.Checked);
            _database.Commit();
        }

        private void OnFilePickerButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = new Uri(_database.Url).AbsolutePath;
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