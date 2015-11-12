using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using System.ComponentModel;

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

        public int MaxWidth { get; set; }

        #endregion

        #region Constructors

        public ColourPickerCell() {}

        public ColourPickerCell(string property)
        {
            Binding = new PropertyBinding<string>(property);
        }

        #endregion

        #region Methods

        protected override void OnPaint(DrawableCellPaintEventArgs e)
        {
            base.OnPaint(e);

            string colourCode = Binding.GetValue(e.Item);

            if (!string.IsNullOrEmpty(colourCode))
            {
                Color colour = Color.Parse(colourCode);

                float x = e.ClipRectangle.Location.X - 3;
                float y = e.ClipRectangle.Location.Y - 3;
                float w = MaxWidth > 0 ? MaxWidth : e.ClipRectangle.Width + 3;
                float h = e.ClipRectangle.Height + 6;

                e.Graphics.FillRectangle(colour, new RectangleF(x, y, w, h));
            }
        }

        #endregion
    }
}

