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
using Artivity.DataModel;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer
{
    [Handler(typeof(IHandler))]
    public class CanvasThumbnailCell : DrawableCell
    {
        #region Members

        private CanvasThumbnailRenderer _thumbnailRenderer = new CanvasThumbnailRenderer() { Padding = new Padding(10, 0, 0, 0) };

        /// <summary>
        /// Gets or sets the binding to get/set the value of the cell.
        /// </summary>
        /// <value>The cell's binding.</value>
        public IIndirectBinding<string> Binding { get; set; }

        #endregion

        #region Constructors

        public CanvasThumbnailCell() {}

        public CanvasThumbnailCell(string property)
        {
            Binding = new PropertyBinding<string>(property);
        }

        #endregion

        #region Methods

        protected override void OnPaint(DrawableCellPaintEventArgs e)
        {
            base.OnPaint(e);

            string filePath = Binding.GetValue(e.Item);

            _thumbnailRenderer.FilePath = filePath;
            _thumbnailRenderer.RenderCanvas(e.Graphics, e.ClipRectangle);
        }

        #endregion
    }
}

