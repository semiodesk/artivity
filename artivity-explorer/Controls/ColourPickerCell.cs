using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer
{
    [Handler(typeof(IHandler))]
    public class ColourPickerCell : DrawableCell
    {
        #region Members

        /// <summary>
        /// Gets or sets the binding to get/set the value of the cell.
        /// </summary>
        /// <value>The cell's binding.</value>
        public IIndirectBinding<string> Binding { get; set; }

        #endregion

        #region Constructors

        public ColourPickerCell() {}

        #endregion

        #region Methods

        protected override void OnPaint(DrawableCellPaintEventArgs e)
        {
            base.OnPaint(e);

            string colourCode = Binding.GetValue(e.Item);

            if (!string.IsNullOrEmpty(colourCode))
            {
                Color colour = Color.Parse(colourCode);

                PointF location = new PointF(e.ClipRectangle.Location.X, e.ClipRectangle.Location.Y);
                SizeF size = new SizeF(e.ClipRectangle.Width, e.ClipRectangle.Height);

                e.Graphics.FillRectangle(colour, new RectangleF(location, size));
            }
        }

        #endregion
    }
}

