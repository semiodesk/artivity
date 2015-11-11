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

        private data.Canvas _canvas;

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

        private double _scalingFactor;

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
            DrawCanvas(e);

            if (HighlightedRegion != null)
            {
                DrawHighlightedRegion(e);
            }
        }

        public void DrawCanvas(PaintEventArgs e)
        {
            _scalingFactor = e.ClipRectangle.GetScalingFactor(_canvas);

            Color shadeColor = Palette.ShadeColor;
            Color canvasFill = Palette.LightColor;
            Color canvasBorder = Palette.DarkColor;

            if (_canvas == null)
            {
                shadeColor.A = shadeColor.A * 0.5f;
                canvasBorder.A = canvasBorder.A * 0.5f;
            }

            PointF location = e.ClipRectangle.Location;
            location.X += Padding.Left;
            location.Y += Padding.Top;

            RectangleF canvas = new RectangleF(location, e.ClipRectangle.Fit(_canvas).Size);
            canvas.Width -= (_shadeOffset.X + Padding.Left + Padding.Right);
            canvas.Height -= (_shadeOffset.Y + Padding.Top + Padding.Bottom);

            RectangleF shade = new RectangleF(canvas.Location + _shadeOffset, canvas.Size);

            e.Graphics.FillRectangle(shadeColor, shade);
            e.Graphics.FillRectangle(canvasFill, canvas);
            e.Graphics.DrawRectangle(canvasBorder, canvas);

            if (_canvas == null)
            {
                string text = "?";
                SizeF textSize = e.Graphics.MeasureString(SystemFonts.Bold(12), text);

                e.Graphics.DrawText(SystemFonts.Bold(12), canvasBorder, e.ClipRectangle.Center - textSize / 2, "?");
            }
        }

        private void DrawHighlightedRegion(PaintEventArgs e)
        {
            Color highlightColor = Palette.AccentColor;
            highlightColor.A = highlightColor.A * 0.9f;

            float x = Convert.ToSingle(Math.Round(HighlightedRegion.Position.X * _scalingFactor, 0));
            float y = Convert.ToSingle(Math.Round(HighlightedRegion.Position.Y * _scalingFactor, 0));

            PointF location = new PointF(x, y);
            location.X += Padding.Left;
            location.Y += Padding.Top;

            RectangleF region = e.ClipRectangle.Fit(HighlightedRegion);
            region.Location = location;
            region.Width -= (_shadeOffset.X + Padding.Left + Padding.Right);
            region.Height -= (_shadeOffset.Y + Padding.Top + Padding.Bottom);

            e.Graphics.FillRectangle(highlightColor, region);
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
                       ?file nfo:fileUrl ""file://" + filePath + @""" .
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

