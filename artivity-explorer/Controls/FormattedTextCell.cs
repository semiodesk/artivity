using System;
using System.ComponentModel;
using System.Collections.Generic;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer
{
    [Handler(typeof(IHandler))]
    public class FormattedTextCell : DrawableCell
    {
        #region Members

        /// <summary>
        /// Gets or sets the binding to get/set the value of the cell.
        /// </summary>
        /// <value>The cell's binding.</value>
        public IIndirectBinding<string> Binding { get; set; }

        public int MaxWidth { get; set; }

        public Padding Padding { get; set; }

        private Font _font;

        public Font Font
        {
            get { return _font; }
            set
            {
                if (value != null)
                {
                    _font = new Font(value.Typeface, _fontSize, _fontDecoration);
                }
                else
                {
                    _font = value;
                }
            }
        }

        private float _fontSize;

        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;

                if (_font != null)
                {
                    _font = new Font(_font.Typeface, value, _fontDecoration);
                }
            }
        }

        private FontDecoration _fontDecoration;

        public FontDecoration FontDecoration
        {
            get{ return _fontDecoration; }
            set
            {
                _fontDecoration = value;

                if (_font != null)
                {
                    _font = new Font(_font.Typeface, _fontSize, value);
                }
            }
        }

        public Color Color { get; set; }

        public Color SelectedColor { get; set; }

        public HorizontalAlignment HorizontalAlign { get; set; }

        public VerticalAlignment VerticalAlign { get; set; }

        #endregion

        #region Constructors

        public FormattedTextCell()
        {
            Font font = SystemFonts.Label();

            Font = SystemFonts.Label();
            FontSize = font.Size;
            FontDecoration = font.FontDecoration;

            Color = Palette.TextColor;
            SelectedColor = Palette.LightColor;
            HorizontalAlign = HorizontalAlignment.Left;
            VerticalAlign = VerticalAlignment.Center;
            Padding = new Padding(-3, -3, 3, -6);
        }

        public FormattedTextCell(string property)
        {
            Font font = SystemFonts.Label();

            Font = SystemFonts.Label();
            FontSize = font.Size;
            FontDecoration = font.FontDecoration;

            Color = Palette.TextColor;
            SelectedColor = Palette.LightColor;
            HorizontalAlign = HorizontalAlignment.Left;
            VerticalAlign = VerticalAlignment.Center;
            Padding = new Padding(-3, -3, 3, -6);
            Binding = new PropertyBinding<string>(property);
        }

        #endregion

        #region Methods

        protected override void OnPaint(DrawableCellPaintEventArgs e)
        {
            base.OnPaint(e);

            Color color = e.CellState == DrawableCellStates.Selected ? SelectedColor : Color;

            string bindingValue = Binding.GetValue(e.Item);

            SizeF totalSize = e.Graphics.MeasureString(_font, bindingValue);

            string[] lines = bindingValue.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                SizeF lineSize = e.Graphics.MeasureString(_font, line);

                PointF position = new PointF();

                if (VerticalAlign == VerticalAlignment.Center)
                {
                    position.Y = e.ClipRectangle.MiddleY - (float)Math.Ceiling(totalSize.Height) / 2;
                }
                else if (VerticalAlign == VerticalAlignment.Top)
                {
                    position.Y = e.ClipRectangle.Top - Padding.Top;
                }
                else
                {
                    position.Y = e.ClipRectangle.Bottom - Padding.Bottom;
                }

                position.Y += i * _font.LineHeight;

                if (HorizontalAlign == HorizontalAlignment.Left)
                {
                    position.X = e.ClipRectangle.Left - Padding.Left;
                }
                else if (HorizontalAlign == HorizontalAlignment.Right)
                {
                    position.X = e.ClipRectangle.Right - (float)Math.Ceiling(lineSize.Width) - Padding.Right;
                }
                else
                {
                    position.X = e.ClipRectangle.MiddleX - (float)Math.Ceiling(lineSize.Width) / 2;
                }

                TextLayout layout = new TextLayout();
                layout.Text = line;
                layout.Position = position;

                e.Graphics.DrawText(_font, color, layout.Position, layout.Text);
            }
        }

        #endregion
    }

    internal struct TextLayout
    {
        public PointF Position;

        public string Text;
    }
}

