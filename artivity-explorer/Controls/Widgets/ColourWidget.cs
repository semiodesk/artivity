using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Explorer.Parsers;

namespace Artivity.Explorer.Controls
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
            Spacing = new Size(7, 7);

            Color color = Color.FromArgb(49, 55, 57);

            var icon = new ImageView() { Image = Bitmap.FromResource("colour") };
            var title = new Label() { Text = "Colour", TextColor = color };

            Rows.Add(new TableRow(new TableCell(icon), new TableCell(title, true), new TableCell()));

            _colours = new Label() { Text = "Colours", TextColor = color };
            _coloursCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_colours), new TableCell(_coloursCount)));

            _colourBox = new ColourBox();

            Rows.Add(new TableRow(new TableCell(), new TableCell(_colourBox)));
        }

        public void Update(SvgStats stats)
        {
            _coloursCount.Text = stats.Colours.Count().ToString();
            _colourBox.Update(stats.Colours);
        }

        #endregion
    }
}
