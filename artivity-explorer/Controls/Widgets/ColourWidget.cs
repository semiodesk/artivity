using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class ColourWidget : TableLayout
    {
        #region Members

        private Label _colours;

        private Label _coloursCount;

        private ColourBox _colourBox;

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
            Color color = new Color(49, 55, 57);

            var icon = new ImageView() { Image = Bitmap.FromResource("colour") };
            var title = new Label() { Text = "Colour", TextColor = color };

            Rows.Add(new TableRow(new TableCell(icon), new TableCell(title)));

            _colours = new Label() { Text = "Colours", TextColor = color };
            _coloursCount = new Label() { Text = "0", TextColor = color };

            Rows.Add(new TableRow(new TableCell(_colours), new TableCell(_coloursCount)));

            _colourBox = new ColourBox();

            Rows.Add(new TableRow(new TableCell(_colourBox, true)));
        }

        public void Update(SvgStats stats)
        {
            _coloursCount.Text = stats.Colours.Count().ToString();
            _colourBox.Update(stats.Colours);
        }

        #endregion
    }
}
