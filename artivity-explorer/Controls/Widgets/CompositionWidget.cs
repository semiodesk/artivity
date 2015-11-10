using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Explorer.Parsers;

namespace Artivity.Explorer.Controls
{
    public class CompositionWidget : TableLayout
    {
        #region Members

        private Label _layers;

        private Label _layersCount;

        private Label _groups;

        private Label _groupsCount;

        private Label _elements;

        private Label _elementsCount;

        private Label _masked;

        private Label _maskedCount;

        private Label _clipped;

        private Label _clippedCount;

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
            Spacing = new Size(7, 7);

            Color color = Color.FromArgb(49, 55, 57);

            var icon = new ImageView() { Image = Bitmap.FromResource("composition") };
            var title = new Label() { Text = "Composition", TextColor = color };

            Rows.Add(new TableRow(new TableCell(icon), new TableCell(title, true), new TableCell()));

            _layers = new Label() { Text = "Layers", TextColor = color };
            _layersCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_layers), new TableCell(_layersCount)));

            _groups = new Label() { Text = "Groups", TextColor = color };
            _groupsCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_groups), new TableCell(_groupsCount)));

            _elements = new Label() { Text = "Elements", TextColor = color };
            _elementsCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_elements), new TableCell(_elementsCount)));

            _masked = new Label() { Text = "  Masked", TextColor = color };
            _maskedCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_masked), new TableCell(_maskedCount)));

            _clipped = new Label() { Text = "  Clipped", TextColor = color };
            _clippedCount = new Label() { Text = "0", TextColor = color, TextAlignment = TextAlignment.Right };

            Rows.Add(new TableRow(new TableCell(), new TableCell(_clipped), new TableCell(_clippedCount)));
        }

        public void Update(SvgStats stats)
        {
            _layersCount.Text = stats.LayerCount.ToString();
            _groupsCount.Text = stats.GroupCount.ToString();
            _elementsCount.Text = stats.ElementCount.ToString();
            _maskedCount.Text = stats.MaskCount.ToString();
            _clippedCount.Text = stats.ClipCount.ToString();
        }

        #endregion
    }
}
