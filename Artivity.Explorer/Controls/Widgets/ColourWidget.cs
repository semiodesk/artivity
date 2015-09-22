using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class ColourWidget : Table
    {
        #region Members

        private readonly Label _colourLabel = new Label("Colours");

        private readonly Label _colourCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly ColourBox _colourBox = new ColourBox();

        #endregion

        #region Constructors

        public ColourWidget()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			Color color = Color.FromBytes(49, 55, 57);

			ImageView icon = new ImageView(BitmapImage.FromResource("colour"));

			Add(icon, 0, 0);

            Label title = new Label("Colour");
            title.Font = Font.WithWeight(FontWeight.Semibold);
            title.TextColor = color;

            Add(title, 1, 0);

            _colourLabel.TextColor = color;
            _colourCountLabel.TextColor = color;

            Add(_colourLabel, 1, 1, 1, 1, true);
            Add(_colourCountLabel, 2, 1);
            Add(_colourBox, 1, 2, 2, 2, true);
        }

        public void Update(SvgStats stats)
        {
            _colourCountLabel.Text = stats.Colours.Count().ToString();
            _colourBox.Update(stats.Colours);
        }

        #endregion
    }
}
