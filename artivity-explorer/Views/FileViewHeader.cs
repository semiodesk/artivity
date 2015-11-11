﻿// LICENSE:
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
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Explorer.Controls;

namespace Artivity.Explorer
{
    public class FileViewHeader : StackLayout
    {
        #region Members

        private Button _homeButton;

        public CanvasThumbnailRenderer Thumbnail { get; private set; }

        private StackLayout _titleLayout;

        private Label _titleLabel;

        private Label _pathLabel;

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;

                _titleLabel.Text = Path.GetFileName(_filePath);
                _pathLabel.Text = Path.GetDirectoryName(_filePath);
                Thumbnail.FilePath = _filePath;
            }
        }

        #endregion

        #region Constructors

        public FileViewHeader()
        {
            Orientation = Orientation.Horizontal;
            Padding = new Padding(7, 7, 7, 0);
            Spacing = 14;

            _homeButton = new Button();
            _homeButton.Image = Bitmap.FromResource("ArrowBack.png");
            _homeButton.Width = 32;
            _homeButton.Height = 40;
            _homeButton.Click += OnHomeButtonClick;

            Thumbnail = new CanvasThumbnailRenderer();
            Thumbnail.Width = 150;
            Thumbnail.Height = 100;

            _titleLabel = new Label();
            _titleLabel.TextColor = Color.Parse("#42484a");
            _titleLabel.Font = SystemFonts.Label(16);

            _pathLabel = new Label();
            _pathLabel.TextColor = Color.Parse("#42484a");

            _titleLayout = new StackLayout();
            _titleLayout.Orientation = Orientation.Vertical;
            _titleLayout.Padding = new Padding (0, 10);
            _titleLayout.Spacing = 7;
            _titleLayout.Items.Add(_titleLabel);
            _titleLayout.Items.Add(_pathLabel);

            Items.Add(new StackLayoutItem(_homeButton, HorizontalAlignment.Left) { VerticalAlignment = VerticalAlignment.Center });
            Items.Add(new StackLayoutItem(Thumbnail, HorizontalAlignment.Left) { VerticalAlignment = VerticalAlignment.Center });
            Items.Add(new StackLayoutItem(_titleLayout, HorizontalAlignment.Left, true));
        }

        #endregion

        #region Methods

        private void OnHomeButtonClick(object sender, EventArgs e)
        {
            MainWindow.Navigate<JournalView>();
        }

        #endregion
    }
}

