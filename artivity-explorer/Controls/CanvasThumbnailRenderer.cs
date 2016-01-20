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
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using data = Artivity.DataModel;

namespace Artivity.Explorer
{
    public class CanvasThumbnailRenderer : Drawable
    {
        #region Members

        private double _scalingFactor;

        // The original document canvas dimensions.
        private data.Canvas _canvas;

        // The canvas transformed into screen coordinates.
        private RectangleF _canvasScaled;

        // The canvas shadow transformed into screen coordinates.
        private RectangleF _canvasShadow;

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                _canvas = TryGetCanvas(value);
            }
        }

        private data.Rectangle _highlightedRegion;

        public data.Rectangle HighlightedRegion
        {
            get { return _highlightedRegion; }
            set
            {
                _highlightedRegion = value;

                Invalidate();
            }
        }

        private Color _highlightColour = Palette.AccentColor;

        public Color HighlightColour
        {
            get { return _highlightColour; }
            set { _highlightColour = value; }
        }

        private PointF _shadeOffset = new PointF(3, 3);

        public PointF ShadeOffset
        {
            get { return _shadeOffset; }
            set
            {
                _shadeOffset = value;

                Invalidate();
            }
        }

        public Size RenderSize { get; private set; }

        #endregion

        #region Constructors

        public CanvasThumbnailRenderer() {}

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Draw(e);
        }

        public void Draw(PaintEventArgs e)
        {
            RenderCanvas(e.Graphics, e.ClipRectangle);

            if (HighlightedRegion != null)
            {
                RenderHighlightedRegion(e.Graphics, e.ClipRectangle);
            }
        }

        public Size Measure(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                Graphics graphics = new Graphics(new Bitmap(new Size(width, height), PixelFormat.Format24bppRgb));

                RenderCanvas(graphics, new RectangleF(0, 0, width, height));
            }
            return RenderSize;
        }

        public void RenderCanvas(Graphics graphics, RectangleF targetRegion)
        {
            _scalingFactor = _canvas != null ? _canvas.GetScalingFactor(targetRegion) : 1;

            Color shadeColor = Palette.ShadeColor;
            Color canvasFill = Palette.LightColor;
            Color canvasBorder = Palette.DarkColor;

            if (_canvas == null)
            {
                shadeColor.A = shadeColor.A * 0.5f;
                canvasBorder.A = canvasBorder.A * 0.5f;
            }

            PointF location = targetRegion.Location;
            location.X += Padding.Left;
            location.Y += Padding.Top;

            SizeF size = _canvas != null ? _canvas.Fit(targetRegion).Size : targetRegion.Size;
            size.Width -= (_shadeOffset.X + Padding.Left + Padding.Right);
            size.Height -= (_shadeOffset.Y + Padding.Top + Padding.Bottom);

            _canvasScaled = new RectangleF(location, size);
            _canvasShadow = new RectangleF(location + _shadeOffset, size);

            graphics.FillRectangle(shadeColor, _canvasShadow);
            graphics.FillRectangle(canvasFill, _canvasScaled);
            graphics.DrawRectangle(canvasBorder, _canvasScaled);

            if (_canvas == null)
            {
                string text = "?";
                SizeF textSize = graphics.MeasureString(SystemFonts.Bold(12), text);

                graphics.DrawText(SystemFonts.Bold(12), canvasBorder, _canvasScaled.Center - textSize / 2, "?");
            }

            Point topLeft = _canvasScaled.TopLeft.ToPoint();
            Point bottomRight = _canvasShadow.BottomRight.ToPoint();
                
            RenderSize = new Rectangle(topLeft, bottomRight).Size;
        }

        public void RenderHighlightedRegion(Graphics graphics, RectangleF targetRegion)
        {
            if (HighlightedRegion == null)
            {
                return;
            }

            Color fillColour = HighlightColour;
            fillColour.A = fillColour.A * 0.75f;

            float x = Convert.ToSingle(Math.Round(HighlightedRegion.Position.X * _scalingFactor, 0));
            float y = Convert.ToSingle(Math.Round(HighlightedRegion.Position.Y * _scalingFactor, 0));
            float w = Convert.ToSingle(Math.Round(HighlightedRegion.Width * _scalingFactor, 0));
            float h = Convert.ToSingle(Math.Round(HighlightedRegion.Height * _scalingFactor, 0));

            PointF location = _canvasScaled.Location;
            location.X += x;
            location.Y += y;

            SizeF size = new SizeF();
            size.Width = w;
            size.Height = h;

            // TODO: Fit into viewfield.
            RectangleF region = new RectangleF(location, size);

            graphics.FillRectangle(fillColour, region);
        }

        private data.Canvas TryGetCanvas(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            IModel model = data.Models.GetActivities();

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                SELECT DISTINCT ?canvas WHERE
                {
                       ?activity prov:used ?file .
                       ?activity prov:startedAtTime ?startTime .

                       ?file rdf:type nfo:FileDataObject .
                       ?file nfo:fileUrl ""file://" + Uri.EscapeUriString(filePath) + @""" .
                       ?file art:canvas ?canvas .
                }
                ORDER BY DESC(?startTime) LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            BindingSet bindings = result.GetBindings().FirstOrDefault();

            if (bindings == null || !bindings.ContainsKey("canvas"))
            {
                return null;
            }

            UriRef canvasUri = new UriRef(bindings["canvas"].ToString());

            return model.GetResource<data.Canvas>(canvasUri);
        }

        #endregion
    }
}

