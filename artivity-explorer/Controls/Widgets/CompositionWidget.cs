using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class CompositionWidget : Table
    {
        #region Members

        private readonly Label _layersLabel = new Label("Layers");

        private readonly Label _layersCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _groupsLabel = new Label("Groups");

        private readonly Label _groupsCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _elementLabel = new Label("Elements");

        private readonly Label _elementCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _maskLabel = new Label("  Masked");

        private readonly Label _maskCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _clipLabel = new Label("  Clipped");

        private readonly Label _clipCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        #endregion

        #region Constructors

        public CompositionWidget()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			Color color = Color.FromBytes(49, 55, 57);

			ImageView icon = new ImageView(BitmapImage.FromResource("composition"));

			Add(icon, 0, 0);

            Label title = new Label("Composition");
            title.Font = Font.WithWeight(FontWeight.Semibold);
            title.TextColor = color;

            Add(title, 1, 0);

            _layersLabel.TextColor = color;
            _layersCountLabel.TextColor = color;

            Add(_layersLabel, 1, 1, 1, 1, true);
            Add(_layersCountLabel, 2, 1);

            _groupsLabel.TextColor = color;
            _groupsCountLabel.TextColor = color;

            Add(_groupsLabel, 1, 2, 1, 1, true);
            Add(_groupsCountLabel, 2, 2);

            _elementLabel.TextColor = color;
            _elementCountLabel.TextColor = color;

            Add(_elementLabel, 1, 3, 1, 1, true);
            Add(_elementCountLabel, 2, 3);

            _maskLabel.TextColor = color;
            _maskCountLabel.TextColor = color;

            Add(_maskLabel, 1, 4, 1, 1, true);
            Add(_maskCountLabel, 2, 4);

            _clipLabel.TextColor = color;
            _clipCountLabel.TextColor = color;

            Add(_clipLabel, 1, 5, 1, 1, true);
            Add(_clipCountLabel, 2, 5);
        }

        public void Update(SvgStats stats)
        {
            _layersCountLabel.Text = stats.LayerCount.ToString();
            _groupsCountLabel.Text = stats.GroupCount.ToString();
            _elementCountLabel.Text = stats.ElementCount.ToString();
            _maskCountLabel.Text = stats.MaskCount.ToString();
            _clipCountLabel.Text = stats.ClipCount.ToString();
        }

        #endregion
    }
}
